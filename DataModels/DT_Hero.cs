using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class DT_Hero : DT_Title
{
    public DT_InfoCard? InfoCard { get; set; } = null;
    public DT_AssetFile? HeroImage { get; set; } = null;
    public List<DT_AssetReference>? AssetReferences { get; set; } = null;
    public List<DT_HeroReference>? HeroReferences { get; set; } = null;


    public DT_Hero() : base()
    {
    }

    //call field name from property
    public string Key
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public virtual List<DT_Hero> Children()
    {
        return new List<DT_Hero>();
    }
    public T AddAssetReference<T>(T item) where T : DT_AssetReference
    {
        AssetReferences ??= new List<DT_AssetReference>();

        if (AssetReferences.IndexOf(item) == -1)
        {
            item.HeroGuid = this.guid;
            AssetReferences.Add(item);
        }
        else
        {
            $"AddAssetReference Duplicate Item".WriteSuccess();
        }

        return item;
    }

    public T AddHeroReference<T>(T item) where T : DT_HeroReference
    {
        HeroReferences ??= new List<DT_HeroReference>();

        if (HeroReferences.IndexOf(item) == -1)
        {
            HeroReferences.Add(item);
        }
        else
        {
            $"AddHeroReference Duplicate Item".WriteSuccess();
        }
        return item;
    }

    public virtual List<DT_AssetFile> CollectAssetFiles(List<DT_AssetFile> list, bool deep)
    {
        if (HeroImage != null)
            list.Add(HeroImage); AssetReferences?.ForEach(assetRef =>
        {
            if (assetRef?.Asset != null)
                list.Add(assetRef.Asset);
        });
        return list;
    }

    public virtual List<DT_AssetReference> CollectAssetReferences(List<DT_AssetReference> list, bool deep)
    {

        if (AssetReferences != null)
            list.AddRange(AssetReferences);

        return list;
    }

    public virtual List<DT_HeroReference> CollectHeroReferences(List<DT_HeroReference> list, bool deep)
    {

        HeroReferences?.ForEach(compRef =>
        {
            list.Add(compRef);
        });
        return list;
    }

}
