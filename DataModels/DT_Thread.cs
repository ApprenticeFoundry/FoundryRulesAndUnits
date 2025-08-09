using System;
using System.Collections.Generic;
using System.Linq;



namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class DT_Thread : DT_Searchable
{
    public string sheetname { get; set; } = "";
    public string thread { get; set; } = "";
    public string domain { get; set; } = "";
    public string resDes { get; set; } = "";
    public string address { get; set; } = "";
    public string serialNumber { get; set; } = "";
    public string addressAsset { get; set; } = "";
    public string version { get; set; } = "";

    public string GetKey()
    {
        return $"{domain}:{address}";
    }

    public List<string> GetElements()
    {
        var elements = thread.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
        return elements;
    }

}