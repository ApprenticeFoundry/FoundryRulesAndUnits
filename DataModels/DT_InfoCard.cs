using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class DT_InfoCard
{
    public ControlParameters? Targets { get; set; }

    public DT_InfoCard()
    {

    }

    public ControlParameters? SetTargets(ControlParameters source)
    {
        this.Targets = source;
        return this.Targets;
    }
}