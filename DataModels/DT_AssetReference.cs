using FoundryRulesAndUnits.Models;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Models;

public interface IDT_Reference
{
    //string targetGuid;
}

[System.Serializable]
public class DT_AssetReference : DT_Title, IDT_Reference
{
    public string? AssetGuid { get; set; }
    public string? HeroGuid { get; set; }
    public DT_AssetFile? Asset { get; set; }
    public HighResPosition? Position { get; set; }


    public DT_AssetReference() : base()
    {
    }
    public DT_AssetReference AttachDocument(DT_AssetFile doc)
    {
        Asset = doc;
        AssetGuid = doc.Guid;
        return this;
    }

    public DT_AssetReference ClearDocument()
    {
        AssetGuid = Asset?.Guid;
        Asset = null;
        return this;
    }

    public DT_AssetReference ShallowCopy()
    {
        var result = (DT_AssetReference)this.MemberwiseClone();
        result.ClearDocument();
        return result;
    }
}