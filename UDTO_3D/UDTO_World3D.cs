// this is a tool to load/unload knowledge modules that define projects
using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Models;


namespace FoundryRulesAndUnits.Models;

public class UDTO_World : UDTO_3D, ISystem
{
    public string SystemName { get; set; }
    public string LengthUnits { get; set; } = "m";
    public string AngleUnits { get; set; } = "r";

    public List<UDTO_Body> Bodies { get; set; } = new();
    public List<UDTO_Pathway> Pathways { get; set; } = new();
    public List<UDTO_Label> Labels { get; set; } = new();
    public List<UDTO_Relationship> Relationships { get; set; } = new();

    public UDTO_World() : base()
    {
    }
    private List<T> FindLookup<T>() where T : UDTO_3D
    {
        if (typeof(T) == typeof(UDTO_Body)) return Bodies as List<T>;
        if (typeof(T) == typeof(UDTO_Label)) return Labels as List<T>;
        if (typeof(T) == typeof(UDTO_Pathway)) return Pathways as List<T>;
        if (typeof(T) == typeof(UDTO_Relationship)) return Relationships as List<T>;

        return null;
    }
    public T FindOrCreate<T>(string name, bool create = false) where T : UDTO_3D
    {
        var list = FindLookup<T>();
        var found = list?.FirstOrDefault(item => name.Matches(item.Name ?? string.Empty));
        if (found == null && create)
        {
            found = CreateItem<T>(name);
            list?.Add(found);
        }
        return found!;
    }
    public T CreateUsing<T>(string name, string guid = "") where T : UDTO_3D
    {
        var found = FindOrCreate<T>(name, true);
        if (!string.IsNullOrEmpty(guid))
        {
            found!.UniqueGuid = guid;
        }
        return found!;
    }


    private T CreateItem<T>(string name) where T : UDTO_3D
    {
        var found = Activator.CreateInstance<T>() as T;
        found.Name = name;
        found.PanID = PanID;
        found.UniqueGuid = Guid.NewGuid().ToString();
        return found;
    }





    public UDTO_World AsShallowCopy()
    {
        var result = (UDTO_World)this.MemberwiseClone();
        result.Flush();
        return result;
    }


    public UDTO_World Flush()
    {
        Bodies.Clear();
        Pathways.Clear();
        Labels.Clear();
        Relationships.Clear();
        return this;
    }



    public UDTO_World FillWorldFromWorld(UDTO_World world)
    {
        Bodies.AddRange(world.Bodies);
        Pathways.AddRange(world.Pathways);
        Labels.AddRange(world.Labels);
        Relationships.AddRange(world.Relationships);
        return RemoveDuplicates();
    }

    public UDTO_World RemoveDuplicates()
    {
        Bodies = Bodies.DistinctBy(i => i.UniqueGuid).ToList();
        Pathways = Pathways.DistinctBy(i => i.UniqueGuid).ToList();
        Labels = Labels.DistinctBy(i => i.UniqueGuid).ToList();
        Relationships = Relationships.DistinctBy(i => i.UniqueGuid).ToList();

        // platforms = platforms.GroupBy(i => i.uniqueGuid).Select(g => g.First()).ToList();
        // bodies = bodies.GroupBy(i => i.uniqueGuid).Select(g => g.First()).ToList();
        // labels = labels.GroupBy(i => i.uniqueGuid).Select(g => g.First()).ToList();
        // relationships = relationships.GroupBy(i => i.uniqueGuid).Select(g => g.First()).ToList();
        return this;
    }
    public T CreateUsingDTBASE<T>(DT_Base obj) where T : UDTO_3D
    {
        return CreateUsing<T>(obj.name ?? "", obj.guid ?? "");
    }



    public UDTO_Body CreateBoundingBox(DT_Base obj, double width = 1.0, double height = 1.0, double depth = 1.0)
    {
        var result = CreateUsingDTBASE<UDTO_Body>(obj);
        return result.EstablishBox(width, height, depth);
    }

}