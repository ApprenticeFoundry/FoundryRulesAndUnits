using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Models;




[System.Serializable]
public class DT_Link : DT_Hero, ISystem
{


    public string? referenceDesignation;

    public string? sourceHeroGuid;
    public string? targetHeroGuid;

    public DT_Link()
    {
    }

    public DT_Link ShallowCopy()
    {
        var result = (DT_Link)this.MemberwiseClone();

        return result;
    }
}


