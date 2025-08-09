namespace FoundryRulesAndUnits.Models;

public class UDTO_ActionStatus : UDTO_Base
{
    public string? Status { get; set; }
    public string? Message { get; set; }


    public UDTO_ActionStatus() : base()
    {
    }

    public static UDTO_ActionStatus info(string message)
    {
        return new UDTO_ActionStatus()
        {
            Status = "INFO",
            Message = message
        };
    }

    public static UDTO_ActionStatus success(string message)
    {
        return new UDTO_ActionStatus()
        {
            Status = "SUCCESS",
            Message = message
        };
    }
    public static UDTO_ActionStatus warning(string message)
    {
        return new UDTO_ActionStatus()
        {
            Status = "WARNING",
            Message = message
        };
    }

    public static UDTO_ActionStatus error(string message)
    {
        return new UDTO_ActionStatus()
        {
            Status = "ERROR",
            Message = message
        };
    }
    public static UDTO_ActionStatus taskComplete(string message)
    {
        return new UDTO_ActionStatus()
        {
            Status = "INFO",
            Message = message
        };
    }
}