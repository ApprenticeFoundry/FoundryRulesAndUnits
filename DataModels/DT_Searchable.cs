using System;
using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;

public class DESIGN_Base
{
    public string guid { get; set; }
    public string parentGuid { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string url { get; set; }
    public string timeStamp { get; set; }
    public ControlParameters metadata { get; set; }
}


public interface ISystem
{

}


[System.Serializable]
public class DT_Searchable : DT_Base
{
    public string? Title { get; set; }  = null;
    public string? Description { get; set; } = null;
    public string? DisplayName { get; set; } = null;
    public string? CommonName { get; set; } = null;

    public DT_Searchable() : base()
    {
    }

}

[System.Serializable]
public class DT_QualityAssurance : DT_Searchable
{
    public string? Action { get; set; } = null;
    public string? Author { get; set; } = null;
    public string? ComponentGuid { get; set; } = null;


    public DT_QualityAssurance() : base()
    {
    }

}

[System.Serializable]
public class DT_Comment : DT_Searchable
{
    public string? Severity { get; set; } = null;
    public string? Author { get; set; } = null;

    public DT_Comment() : base()
    {
    }
    public DT_Comment OK()
    {
        Severity = "OK";
        return this;
    }
    public DT_Comment Error()
    {
        Severity = "Error";
        return this;
    }
    public DT_Comment Bug()
    {
        Severity = "Bug";
        return this;
    }
    public DT_Comment Comment()
    {
        Severity = "Comment";
        return this;
    }
    public DT_Comment MissingReference()
    {
        Severity = "Missing";
        return this;
    }
}
