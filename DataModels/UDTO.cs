

using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;



namespace FoundryRulesAndUnits.Models;


public static class UDTO
{
    private static readonly Dictionary<string, Type> udtoTypes = new Dictionary<string, Type>();

    public static string asTopic<T>() where T : UDTO_Base
    {
        return UDTO_Base.asTopic(typeof(T).Name);
    }
    public static bool matchTopic<T>(UDTO_Base obj) where T : UDTO_Base
    {
        return obj.UdtoTopic == asTopic<T>();
    }

    public static bool matchTopic<T>(string topic) where T : UDTO_Base
    {
        return topic == asTopic<T>();
    }
    public static T decodePayload<T>(UDTO_ServerSync transport) where T : UDTO_Base
    {
        T? obj = Activator.CreateInstance(typeof(T)) as T;
        if (obj != null)
        {
            transport.decodePayload<T>(obj);
            //use sync to fix any shorthand issues with guid or timedate
            return obj.sync<T>();
        }
        throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}");
    }


    public static Type LookupType(string name)
    {
        if (udtoTypes.Keys.Count == 0)
        {
            var assembly = typeof(UDTO_Base).Assembly;
            foreach (var type in assembly.DefinedTypes.Where(item => item.Name.StartsWith("UDTO_")))
            {
                udtoTypes.Add(type.Name, type);
                var shortname = type.Name.Replace("UDTO_", "");
                udtoTypes.Add(shortname, type);
            }
        }
        if (udtoTypes.TryGetValue(name, out Type? found) && found != null)
        {
            return found;
        }
        throw new ArgumentException($"type not found {name}");
    }

    //public static UDTO_Base Hydrate(string target)
    //{
    //	using var stream = new MemoryStream();
    //	using var writer = new Utf8JsonWriter(stream);
    //	var node = JsonNode.Parse(target);
    //	node.WriteTo(writer);
    //	writer.Flush();

    //	var topic = node["udtoTopic"].ToString();
    //	Type type = LookupType(topic);
    //	var result = JsonSerializer.Deserialize(stream.ToArray(), type) as UDTO_Base;

    //	return result;
    //}

    //public static string Dehydrate(UDTO_Base target)
    //{
    //	var result = JsonSerializer.Serialize(target, target.GetType());
    //	return result;
    //}
}