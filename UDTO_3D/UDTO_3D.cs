using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_3D : UDTO_Base
{

    public bool Visible { get; set; } = true;
    public string? Type { get; set; }
    public string? Name { get; set; }
    public string? Material { get; set; }
    public string? UniqueGuid { get; set; }
    public string? ParentUniqueGuid { get; set; }

    public string? SourceURL { get; set; }
    public DT_Part? Part { get; set; }


    public UDTO_3D() : base()
    {
    }
    public virtual UDTO_3D CopyFrom(UDTO_3D obj)
    {
        UniqueGuid = obj.UniqueGuid;
        Type = obj.Type;
        Name = obj.Name;
        Part = obj.Part;
        return this;
    }

    public DT_Part GetPart()
    {
        Part ??= new DT_Part()
        {
            partNumber = Name,
            version = "1.0"
        };
        return Part;
    }

    public DT_Hero AsHero()
    {
        var hero = new DT_Hero()
        {
            guid = UniqueGuid,
            name = Name,
            Title = partName()
        };
        return hero;
    }
    public string partName()
    {
        if (Part == null)
            return Name ?? "";

        return $"{Name}_{Part.structureReference}";
    }
    public bool isDelete()
    {
        return this.Type == "Command:DELETE";
    }

    public bool isVisible()
    {
        return this.Visible;
    }

    public UDTO_3D setVisible(bool visible)
    {
        this.Visible = visible;
        return this;
    }
}
