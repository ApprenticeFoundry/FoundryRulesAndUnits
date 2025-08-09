using System.Collections.Generic;

namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_Relationship : UDTO_3D
{
    public string? Relationship { get; set; }
    public string? Source { get; set; }
    public List<string> Sink { get; set; } = new List<string>();


    public UDTO_Relationship() : base()
    {
    }
    public override UDTO_3D CopyFrom(UDTO_3D obj)
    {
        base.CopyFrom(obj);
        var rel = obj as UDTO_Relationship;
        this.Source = rel?.Source;
        this.Relationship = rel?.Relationship;
        return this;
    }

    public UDTO_Relationship Build(string source, string relationship, string target)
    {
        this.Source = source;
        this.Relationship = relationship;
        this.Sink.Add(target);
        return this;
    }

    public UDTO_Relationship Relate(string target)
    {
        this.Sink.Add(target);
        return this;
    }

    public UDTO_Relationship Unrelate(string target)
    {
        this.Sink.Remove(target);
        return this;
    }
}