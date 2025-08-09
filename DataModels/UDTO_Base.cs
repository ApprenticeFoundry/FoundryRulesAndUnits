using System;
using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Models;


// https://khalidabuhakmeh.com/convert-a-csharp-object-to-almost-any-format
namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_Base
{
    static Guid defaultSourceGuid = Guid.NewGuid();
    public string? UdtoTopic { get; set; }
    public string? SourceGuid { get; set; }
    public string? RefGuid { get; set; }
    public string? TimeStamp { get; set; }
    public string? PanID { get; set; }
    public ControlParameters? Metadata { get; set; }
    public UDTO_Base()
    {
        this.initialize(null);
    }
    public T Duplicate<T>() where T : UDTO_Base
    {
        var dupe = this.MemberwiseClone() as T;
        return dupe ?? throw new InvalidOperationException("Failed to clone object");
    }

    public virtual string compress(char d = '\u002C')
    {
        return this.EncodeFieldDataAsCSV(d);
    }

    public virtual int decompress(string[] data)
    {
        return this.DecodeFieldDataAsCSV(data);
    }
    public string resetTimeStamp()
    {
        this.TimeStamp = DateTime.UtcNow.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        return this.TimeStamp;
    }

    public static string asTopic(string name)
    {
        return name.Replace("UDTO_", "");
    }

    public static string asTopic<T>() where T : UDTO_Base
    {
        return UDTO_Base.asTopic(typeof(T).Name);
    }

    public static string asTopicLower<T>() where T : UDTO_Base
    {
        return UDTO_Base.asTopic(typeof(T).Name).ToLower();
    }
    public T sync<T>() where T : UDTO_Base
    {
        if (String.IsNullOrEmpty(UdtoTopic))
        {
            UdtoTopic = UDTO_Base.asTopic(this.GetType().Name);
        }
        return (this as T) ?? throw new InvalidOperationException($"Failed to cast to type {typeof(T).Name}");
    }
    public UDTO_Base initialize(string? defaultPanID)
    {
        if (String.IsNullOrEmpty(UdtoTopic))
        {
            UdtoTopic = UDTO_Base.asTopic(this.GetType().Name);
        }
        if (String.IsNullOrEmpty(TimeStamp))
        {
            resetTimeStamp();
        }
        if (String.IsNullOrEmpty(SourceGuid))
        {
            SourceGuid = UDTO_Base.defaultSourceGuid.ToString();
        }
        if (String.IsNullOrEmpty(RefGuid))
        {
            RefGuid = Guid.NewGuid().ToString();
        }
        if (String.IsNullOrEmpty(PanID))
        {
            PanID = defaultPanID;
        }
        return this;
    }
    public virtual string UniqueCode()
    {
        return RefGuid ?? "";
    }


    public virtual bool matches(UDTO_Base other)
    {
        return this.UniqueCode() == other.UniqueCode();
    }
    public virtual bool matchesPanSource(UDTO_Base other)
    {
        return this.PanID == other.PanID && this.SourceGuid == other.SourceGuid;
    }
    public ControlParameters MetaData()
    {
        Metadata ??= new ControlParameters();
        return Metadata;
    }


    public bool HasMetaData()
    {
        return Metadata != null;
    }

    public bool HasMetaDataKey(string key)
    {
        if (Metadata != null)
        {
            return Metadata.Find(key) != null;
        }
        return false;
    }
    public ControlParameters AddMetaData(string key, string value)
    {
        MetaData().Establish(key, value);
        return Metadata ?? new ControlParameters();
    }

    public object GetMetaData(string key)
    {
        return Metadata?.Find(key) ?? "";
    }
}