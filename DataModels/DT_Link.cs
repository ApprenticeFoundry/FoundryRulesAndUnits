using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Models;




[System.Serializable]
public class DT_Link : DT_Hero, ISystem
{


    public string? ReferenceDesignation { get; set; } = null;

    public string? SourceHeroGuid { get; set; } = null;
    public string? TargetHeroGuid { get; set; } = null;

    public DT_Link()
    {
    }

    public DT_Link ShallowCopy()
    {
        var result = (DT_Link)this.MemberwiseClone();

        return result;
    }
}


