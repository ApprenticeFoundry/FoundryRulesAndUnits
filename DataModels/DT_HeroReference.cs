using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class DT_HeroReference : DT_Title, IDT_Reference
{
    public DT_Part? Part { get; set; } = null;

    public DT_Hero? Hero { get; set; } = null;

    public DT_HeroReference() : base()
    {
    }
    public DT_Part? MarkAsTemplate()
    {
        Part ??= new DT_Part();
        Part.serialNumber = "TBD";
        return Part;
    }

    public string ComputeTitle()
    {
        var title = Part?.ComputeTitle() ?? "";

        // if (promise != null)
        // 	title = $"[{promise.key}|{title}]";

        return title;
    }

    public DT_HeroReference ShallowCopy()
    {
        var result = (DT_HeroReference)this.MemberwiseClone();
        if (Part != null)
            result.Part = (DT_Part)Part.ShallowCopy();

        result.Hero = null;
        return result;
    }
}