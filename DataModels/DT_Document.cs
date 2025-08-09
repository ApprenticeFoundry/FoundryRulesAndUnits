using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Models;




[System.Serializable]
public class DT_Document : DT_Hero, ISystem
{
    public int memberCount;


    public string? referenceDesignation;
    public string? itemDescription;
    public string? itemDescriptionRevision;
    public List<DT_Document>? children;


    public DT_Document()
    {
    }

    public DT_Document AddChild(DT_Document child)
    {
        children ??= new List<DT_Document>();
        child.parentGuid = this.guid;

        children.Add(child);
        this.memberCount = children.Count;
        return child;
    }

    public override List<DT_Hero> Children()
    {
        if (children == null) return base.Children();
        return children.Cast<DT_Hero>().ToList();
    }

    public override List<DT_AssetFile> CollectAssetFiles(List<DT_AssetFile> list, bool deep)
    {
        base.CollectAssetFiles(list, deep);
        if (!deep) return list;

        children?.ForEach(step =>
        {
            step.CollectAssetFiles(list, deep);
        });
        return list;
    }

    public override List<DT_AssetReference> CollectAssetReferences(List<DT_AssetReference> list, bool deep)
    {
        base.CollectAssetReferences(list, deep);
        if (!deep) return list;

        children?.ForEach(step =>
        {
            step.CollectAssetReferences(list, deep);
        });

        return list;
    }

    public override List<DT_HeroReference> CollectHeroReferences(List<DT_HeroReference> list, bool deep)
    {
        base.CollectHeroReferences(list, deep);
        if (!deep) return list;

        children?.ForEach(step =>
        {
            step.CollectHeroReferences(list, deep);
        });
        return list;
    }

    public override List<DT_Comment> CollectComments(List<DT_Comment> list)
    {
        base.CollectComments(list);

        children?.ForEach(step =>
        {
            step.CollectComments(list);
        });
        return list;
    }

    public override List<DT_QualityAssurance> CollectQualityAssurance(List<DT_QualityAssurance> list)
    {
        base.CollectQualityAssurance(list);

        children?.ForEach(step =>
        {
            step.CollectQualityAssurance(list);
        });
        return list;
    }
    public DT_Document ShallowCopy()
    {
        var result = (DT_Document)this.MemberwiseClone();
        result.children = null;

        return result;
    }

    public List<DT_Document> ShallowSteps()
    {
        var result = children?.Select(obj => obj.ShallowCopy()).ToList();
        return result;
    }

}


