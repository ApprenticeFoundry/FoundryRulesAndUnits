namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_ServerSync
{
    public string Payload { get; set; } = "";  //payload
    public string Command { get; set; } = "";  //command
    public string History { get; set; } = "";  //history


    public UDTO_ServerSync()
    {
    }
    public void encodePayload<T>(T obj) where T : UDTO_Base
    {
        Payload = obj.compress();
    }
    public T decodePayload<T>(T obj) where T : UDTO_Base
    {
        var list = Payload.Split(",");
        obj.decompress(list);
        return obj;
    }


    public string udtoTopic()
    {
        return UDTO_Base.asTopic(this.GetType().Name);
    }
    public bool isNodeInHistory(string name, bool add = false)
    {
        if (History.Contains(name))
        {
            return true;
        }
        else if (add)
        {
            addToHistory(name);
        }
        return false;
    }

    public bool addToHistory(string name)
    {
        if (History.Contains(name))
        {
            return true;
        }
        if (History.Length == 0)
        {
            History = $"{name}";
        }
        else
        {
            History = $"{History},{name}";
        }
        return true;
    }

}
[System.Serializable]
public class UDTO_PubSubSync : UDTO_ServerSync
{
    public string Topic { get; set; } = ""; //topic

    public UDTO_PubSubSync() : base()
    {
    }
}