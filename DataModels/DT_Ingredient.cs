using System.Text.Json.Serialization;
using FoundryRulesAndUnits.Models;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Models;



//[JsonDerivedType(typeof(DT_Geometry))]
[JsonDerivedType(typeof(DT_Component))]
[JsonDerivedType(typeof(DT_Sensor))]
[System.Serializable]
public class DT_Ingredient : DT_Hero, ISystem
{
    public DT_Part? Part { get; set; } = null;
    public string? ParentName { get; set; } = null;
    public string? SystemName { get; set; } = null;
    public string? Category { get; set; } = null;

    public DT_Ingredient() : base()
    {
    }


    public DT_Part GetPart()
    {
        Part ??= new DT_Part() { partNumber = name };
        return Part;
    }
    public string ComputeTitle()
    {
        var title = GetPart().ComputeTitle();
        return title;
    }
    public virtual DT_Ingredient ShallowCopy()
    {
        var result = (DT_Ingredient)this.MemberwiseClone();
        if (Part != null)
            result.Part = (DT_Part)Part.ShallowCopy();

        result.ParentName = this.ParentName;
        result.SystemName = this.SystemName;
        result.Category = this.Category;

        return result;
    }

    public string SetCatagory(string elementType)
    {
        Category = elementType.ToUpper();
        return Category;
    }
    public bool IsCatagory(string elementType)
    {
        return Category?.Matches(elementType) ?? false;
    }
}
