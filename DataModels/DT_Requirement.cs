using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Models;




[System.Serializable]
public class DT_Requirement : DT_Hero, ISystem
{
    public int memberCount;

    public string? category;    
    public string? phase;       
    public string? timing;
    public string? release;
    public string? evidence;
    public string? acceptanceCriteria;

    public List<DT_Requirement>? details;


    public DT_Requirement()
    {
    }

    public override List<DT_Hero> Children()
    {
        if (details == null) return base.Children();
        return details.Select(item => (DT_Hero)item).ToList();
    }

    public DT_Requirement AddDetail(DT_Requirement detail)
    {
        details ??= new List<DT_Requirement>();
        detail.parentGuid = this.guid;

        details.Add(detail);
        this.memberCount = details.Count;
        return detail;
    }


    public override List<DT_AssetFile> CollectAssetFiles(List<DT_AssetFile> list, bool deep)
    {
        base.CollectAssetFiles(list, deep);
        if (!deep) return list;

        details?.ForEach(step =>
        {
            step.CollectAssetFiles(list, deep);
        });
        return list;
    }

    public override List<DT_AssetReference> CollectAssetReferences(List<DT_AssetReference> list, bool deep)
    {
        base.CollectAssetReferences(list, deep);
        if (!deep) return list;

        details?.ForEach(step =>
        {
            step.CollectAssetReferences(list, deep);
        });

        return list;
    }

    public override List<DT_HeroReference> CollectHeroReferences(List<DT_HeroReference> list, bool deep)
    {
        base.CollectHeroReferences(list, deep);
        if (!deep) return list;

        details?.ForEach(step =>
        {
            step.CollectHeroReferences(list, deep);
        });
        return list;
    }

    public override List<DT_Comment> CollectComments(List<DT_Comment> list)
    {
        base.CollectComments(list);

        details?.ForEach(step =>
        {
            step.CollectComments(list);
        });
        return list;
    }

    public override List<DT_QualityAssurance> CollectQualityAssurance(List<DT_QualityAssurance> list)
    {
        base.CollectQualityAssurance(list);

        details?.ForEach(step =>
        {
            step.CollectQualityAssurance(list);
        });
        return list;
    }
    public DT_Requirement ShallowCopy()
    {
        var result = (DT_Requirement)this.MemberwiseClone();
        result.details = null;

        return result;
    }

    public List<DT_Requirement> ShallowDetails()
    {
        var result = details?.Select(obj => obj.ShallowCopy()).ToList();
        return result;
    }

}


