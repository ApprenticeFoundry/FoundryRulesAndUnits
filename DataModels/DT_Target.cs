using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Models;


namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public abstract class DT_NetworkItem : DT_Searchable
{
    public string? system;
    public bool IsVisited = false;
}

[System.Serializable]
public class DT_TargetLink : DT_NetworkItem
{
    public string? SourceGuid;
    public string? SinkGuid;

    public DT_TargetLink()
    {
        this.Type = "DT_TargetLink";
    }

    public static DT_TargetLink Create(DT_Target from, DT_Target to)
    {
        var link = new DT_TargetLink();
        return link.Link(from, to);
    }

    public DT_TargetLink Link(DT_Target from, DT_Target to)
    {
        from.LinkCount++;
        to.LinkCount++;

        this.SourceGuid = from.Guid;
        this.SinkGuid = to.Guid;
        this.Name = $"{from.Address} -- {to.Address}";
        this.Title = $"{from.GetKey()} == {to.GetKey()}";
        return this;
    }
    public bool IsValid()
    {
        return SourceGuid != null && SinkGuid != null;
    }
    public bool IncludesTarget(DT_Target target)
    {
        if (IsValid())
            return SourceGuid == target.Guid || SinkGuid == target.Guid;
        return false;
    }
    public string? OtherTarget(DT_Target target)
    {
        if (SourceGuid == target.Guid)
            return SinkGuid;
        if (SinkGuid == target.Guid)
            return SourceGuid;
        return null;
    }

}

[System.Serializable]
public class DT_Target : DT_NetworkItem
{
    public string Address = "";
    public string Domain = "";
    public int LinkCount;

    public DT_Part Part = new();
    public DT_HeroReference HeroReference = new();
    public DT_AssetFile Asset = new();


    public int X;
    public int Y;
    public int Z;


    public DT_Target()
    {
        Type = "DT_Target";
        LinkCount = 0;
    }
    public DT_Part CopyFrom(DT_Part source)
    {
        source.CopyNonNullFields(this.Part);
        return this.Part;
    }

    public DT_Part GetPart()
    {
        Part ??= new DT_Part() { PartNumber = Address };
        return Part;
    }

    public string GetKey()
    {
        return $"{Domain}:{Address}";
    }


    public string FullKey()
    {
        var part = GetPart();
        return $"{Domain}:{part.ComputeTitle()}";
    }

    public bool MatchPart(DT_Part other)
    {
        if (other == null) return false;
        var part = GetPart();
        if (part.PartNumber != other.PartNumber) return false;
        if (part.SerialNumber != other.SerialNumber) return false;
        if (part.Version != other.Version) return false;
        if (part.ReferenceDesignation != other.ReferenceDesignation) return false;
        return true;
    }




}