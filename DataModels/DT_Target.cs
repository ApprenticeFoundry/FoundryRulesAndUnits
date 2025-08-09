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
    public string? sourceGuid;
    public string? sinkGuid;

    public DT_TargetLink()
    {
        this.type = "DT_TargetLink";
    }

    public static DT_TargetLink Create(DT_Target from, DT_Target to)
    {
        var link = new DT_TargetLink();
        return link.Link(from, to);
    }

    public DT_TargetLink Link(DT_Target from, DT_Target to)
    {
        from.linkCount++;
        to.linkCount++;

        this.sourceGuid = from.guid;
        this.sinkGuid = to.guid;
        this.name = $"{from.address} -- {to.address}";
        this.Title = $"{from.GetKey()} == {to.GetKey()}";
        return this;
    }
    public bool IsValid()
    {
        return sourceGuid != null && sinkGuid != null;
    }
    public bool IncludesTarget(DT_Target target)
    {
        if (IsValid())
            return sourceGuid == target.guid || sinkGuid == target.guid;
        return false;
    }
    public string? OtherTarget(DT_Target target)
    {
        if (sourceGuid == target.guid)
            return sinkGuid;
        if (sinkGuid == target.guid)
            return sourceGuid;
        return null;
    }

}
[System.Serializable]
public class DT_Target : DT_NetworkItem
{
    public string address = "";
    public string domain = "";
    public int linkCount;

    public DT_Part part = new();
    public DT_HeroReference heroReference = new();
    public DT_AssetFile asset = new();
    public List<string> threads = new();

    public int x;
    public int y;
    public int z;


    public DT_Target()
    {
        type = "DT_Target";
        linkCount = 0;
    }
    public DT_Part CopyFrom(DT_Part source)
    {
        source.CopyNonNullFields(this.part);
        return this.part;
    }

    public DT_Part GetPart()
    {
        part ??= new DT_Part() { partNumber = address };
        return part;
    }

    public string GetKey()
    {
        return $"{domain}:{address}";
    }


    public string FullKey()
    {
        var part = GetPart();
        return $"{domain}:{part.ComputeTitle()}";
    }

    public bool MatchPart(DT_Part other)
    {
        if (other == null) return false;
        var part = GetPart();
        if (part.partNumber != other.partNumber) return false;
        if (part.serialNumber != other.serialNumber) return false;
        if (part.version != other.version) return false;
        if (part.referenceDesignation != other.referenceDesignation) return false;
        return true;
    }

    public List<string> AddThread(string thread)
    {
        threads ??= new List<string>();
        if (thread != null)
            threads.Add(thread);

        return threads;
    }


}