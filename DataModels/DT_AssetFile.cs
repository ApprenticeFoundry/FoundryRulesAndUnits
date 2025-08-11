using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class DT_AssetFile : DT_Title
{
    public string? Status { get; set; }
    public string? Filename { get; set; }
    public string? DocType { get; set; }
    public string? Source { get; set; }

    public BoundingBox? Boundingbox { get; set; }



    public DT_AssetFile() : base()
    {
    }
    public bool IsReadyToUse()
    {
        if (Status == "LocalCache") return true;
        return false;
    }
    public string? MarkAsNotReferences()
    {
        Status = "NotReferenced";
        return Status;
    }
    public string? MarkAsNotFound()
    {
        Status = "NotFound";
        return Status;
    }
    public string? MarkAsLocalCache()
    {
        Status = "LocalCache";
        return Status;
    }
    public string? MarkAsBlobStorage()
    {
        Status = "BlobStorage";
        return Status;
    }
    public string? MarkAsExternalReference()
    {
        Status = "ExternalReference";
        return Status;
    }
}


