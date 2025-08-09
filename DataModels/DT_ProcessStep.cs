using System.Collections.Generic;
using System.Linq;
namespace FoundryRulesAndUnits.Models;



[System.Serializable]
public class DT_ProcessStep : DT_Hero
{

    public int memberCount;
    public int stepNumber;

    public List<DT_StepItem>? details;



    public DT_ProcessStep() : base()
    {
    }

    public override List<DT_Hero> Children()
    {
        if (details == null) return base.Children();
        return details.Cast<DT_Hero>().ToList();
    }

    public T AddStepDetail<T>(T detail) where T : DT_StepItem
    {
        details ??= new List<DT_StepItem>();
        detail.parentGuid = this.guid;

        details.Add(detail);
        this.memberCount = details.Count;

        details = details.OrderBy(x => x.itemNumber).ToList();
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
    public DT_ProcessStep ShallowCopy()
    {
        var result = (DT_ProcessStep)this.MemberwiseClone();

        result.details = result.details?.Select(obj => obj.ShallowCopy()).ToList();

        return result;
    }

}
