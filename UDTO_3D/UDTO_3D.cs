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
            PartNumber = Name,
            Version = "1.0"
        };
        return Part;
    }

    public DT_Hero AsHero()
    {
        var hero = new DT_Hero()
        {
            Guid = UniqueGuid,
            Name = Name,
            Title = PartName()
        };
        return hero;
    }
    public string PartName()
    {
        if (Part == null)
            return Name ?? "";

        return $"{Name}_{Part.StructureReference}";
    }
    public bool IsDelete()
    {
        return this.Type == "Command:DELETE";
    }

    public bool IsVisible()
    {
        return this.Visible;
    }

    public UDTO_3D SetVisible(bool visible)
    {
        this.Visible = visible;
        return this;
    }
}
