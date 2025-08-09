namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_ComputePair : UDTO_Base
{
    public string? Type { get; set; }
    public string? Name { get; set; }
    public string? Active { get; set; }
    public string? Version { get; set; }
    public int CallCount { get; set; }
    public double KbTransfered { get; set; }
    public string? Info { get; set; }
    public string? Container { get; set; }
    public string? SourceURL { get; set; }
    public string? TargetURL { get; set; }
    public string? LastEvent { get; set; }
    public string? LastError { get; set; }
    public string? Purpose { get; set; }


    public UDTO_ComputePair() : base()
    {
    }
}
