using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;


public class DT_Sensor : DT_Ingredient
{
    public string? Topic { get; set; }
    public string? SourceGuid { get; set; }
    public string? SourceURL { get; set; }
    public string? Data { get; set; }  //place holder for raw data

    public List<DT_Sensor>? members;

    public DT_Sensor() : base()
    {
    }

    public override List<DT_Hero> Children()
    {
        if (members == null) return base.Children();
        return members.Select(item => (DT_Hero)item).ToList();
    }

    public DT_Sensor AddMember(DT_Sensor child)
    {
        members ??= new List<DT_Sensor>();
        child.parentGuid = this.guid;
        members.Add(child);
        return child;
    }

    public override DT_Sensor ShallowCopy()
    {
        var result = (DT_Sensor)this.MemberwiseClone();
        result.members = null;

        if (Part != null)
            result.Part = (DT_Part)Part.ShallowCopy();

        return result;
    }

    public List<DT_Sensor> ShallowMembers()
    {
        var result = members?.Select(obj => obj.ShallowCopy()).ToList();
        return result ?? new List<DT_Sensor>();
    }

}