using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_Pathway : UDTO_3D
{
    public List<UDTO_Body> Datums { get; set; } = new();

    public UDTO_Pathway() : base()
    {
        UniqueGuid = Guid.NewGuid().ToString();
        Type = UDTO_Base.asTopic<UDTO_Pathway>();
    }


    public UDTO_Body FindDatum(string name)
    {
        var found = Datums.Where(obj => name.Matches(obj.Name ?? "")).FirstOrDefault();
        return found;
    }

    public UDTO_Body EstablishDatum(string name, double x = 0.0, double y = 0.0, double z = 0.0)
    {
        var datum = FindDatum(name);
        if (datum == null)
        {
            datum = new UDTO_Body()
            {
                Name = name,
                Position = new UDTO_HighResPosition(x, y, z),
            };
            Datums.Add(datum);
        }

        return datum;
    }


    public UDTO_Pathway Flush()
    {
        Datums.Clear();
        return this;
    }

    public UDTO_Pathway AsShallowCopy()
    {
        var result = (UDTO_Pathway)this.MemberwiseClone();
        result.Flush();
        return result;
    }
}