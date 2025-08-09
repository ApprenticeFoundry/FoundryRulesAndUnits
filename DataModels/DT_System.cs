using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;


namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class DT_System : DT_Hero, ISystem
{
    public List<DT_Thread>? threads;
    public List<DT_Target>? targets;
    public List<DT_TargetLink>? links;

    public Dictionary<string, DT_Target> lookup = new();

    public DT_System()
    {
    }
    public DT_System ShallowCopy()
    {
        var result = (DT_System)this.MemberwiseClone();
        result.threads = null;
        result.targets = null;
        result.links = null;
        result.AssetReferences = null;
        result.HeroImage = this.HeroImage;

        return result;
    }

    public void MergeMembers(DT_System obj)
    {
        foreach (var target in obj.Targets())
        {
            AddTarget(target);
        }
        foreach (var link in obj.Links())
        {
            AddLink(link);
        }
    }

    public void Flush()
    {
        targets = new();
        links = new();
    }
    public DT_Target AddTarget(DT_Target target)
    {
        targets ??= new List<DT_Target>();
        if (target != null)
            targets.Add(target);

        return target;
    }

    public List<DT_Target> RemoveUnlinkedTargets()
    {
        var unlinked = Targets().Where(t => t.linkCount == 0).ToList();
        var linked = Targets().Where(t => t.linkCount > 0).ToList();

        foreach (var item in unlinked)
        {
            $"Unlinked: {item.GetKey()}".WriteLine(System.ConsoleColor.DarkYellow);
        }

        // foreach (var item in linked)
        // {
        // 	$"Linked: {item.GetKey()} {item.linkCount}".WriteLine(System.ConsoleColor.DarkGreen);
        // }

        // foreach (var item in Links())
        // {
        // 	$"Edge: {item.title}".WriteLine(System.ConsoleColor.Green);
        // }

        targets = linked;
        return targets;
    }

    public List<DT_TargetLink> RemoveInvalidLinks()
    {
        var validLinks = Links().Where(l => l.IsValid()).ToList();

        links = validLinks;
        return validLinks;
    }
    public List<DT_Target> Targets()
    {
        targets ??= new List<DT_Target>();
        return targets;
    }
    public List<DT_TargetLink> Links()
    {
        links ??= new List<DT_TargetLink>();
        return links;
    }
    public List<DT_Thread> Threads()
    {
        threads ??= new List<DT_Thread>();
        return threads;
    }
    public DT_Target? LookupTarget(string key)
    {
        lookup.TryGetValue(key, out DT_Target? found);
        return found;
    }

    public DT_Target? FindTarget(string key)
    {
        var found = targets?.FirstOrDefault(t => t.GetKey().Matches(key));
        return found;
    }
    public DT_Target? FindTargetGuid(string guid)
    {
        var found = targets?.FirstOrDefault(t => t.guid == guid);
        return found;
    }
    public DT_Target? FindTarget(string type, string controlNumber)
    {
        var found = targets?.FirstOrDefault(t => t.domain.Matches(type) && t.address.Matches(controlNumber));
        return found;
    }

    public (DT_Target?, DT_Target?) ResolveLink(DT_TargetLink link)
    {
        var source = targets?.FirstOrDefault(t => (link.sourceGuid ?? "").Matches(t.guid ?? ""));
        var sink = targets?.FirstOrDefault(t => (link.sinkGuid ?? "").Matches(t.guid ?? ""));
        return (source, sink);
    }

    public DT_Target CreateTarget(string type, string address)
    {
        var target = new DT_Target()
        {
            address = address,
            domain = type,
            guid = System.Guid.NewGuid().ToString()
        };
        AddTarget(target);
        AddToLookup(target);
        return target;
    }

    private DT_Target AddToLookup(DT_Target target)
    {
        if (lookup.ContainsKey(target.GetKey()))
        {
            lookup.Remove(target.GetKey());
        }
        lookup.Add(target.GetKey(), target);
        return target;
    }


    public DT_TargetLink CreateLink(DT_Target t1, DT_Target t2)
    {
        var link = DT_TargetLink.Create(t1, t2);
        AddLink(link);
        return link;
    }
    public DT_TargetLink? AddLink(DT_TargetLink? link)
    {
        links ??= new List<DT_TargetLink>();
        if (link != null)
            links.Add(link);

        return link;
    }

    public DT_Thread? AddThread(DT_Thread? thread)
    {
        threads ??= new List<DT_Thread>();
        if (thread != null)
            threads.Add(thread);

        return thread;
    }

    public DT_System? ExtractSubSystem(string domain, string address, string label)
    {
        var target = FindTarget(domain, address);
        if (target == null)
        {
            $"ExtractSubSystem Target not found: {domain} {address}".WriteError();
            return null;
        }
        return ExtractSubSystem(target, label);
    }

    public DT_System ExtractSubSystem(DT_Target target, string label)
    {
        foreach (var item in Targets())
            item.IsVisited = false;

        foreach (var item in Links())
            item.IsVisited = false;

        var system = new DT_System();
        ExtractSubSystemLinks(target, system);

        foreach (var item in system.Targets())
            item.system = label;

        foreach (var item in system.Links())
            item.system = label;

        return system;
    }

    private void ExtractSubSystemLinks(DT_Target target, DT_System system)
    {
        if (target == null || target.IsVisited)
            return;

        target.IsVisited = true;
        system.AddTarget(target);

        var links = Links().Where(link => link.IncludesTarget(target)).ToList();

        foreach (var link in links)
        {
            if (link.IsVisited)
                continue;

            link.IsVisited = true;
            var found = system.Links().FirstOrDefault(l => l.guid == link.guid);
            if (found != null)
                continue;

            system.AddLink(link); var otherguid = link.OtherTarget(target);
            if (otherguid != null)
            {
                var otherTarget = FindTargetGuid(otherguid);
                if (otherTarget != null)
                    ExtractSubSystemLinks(otherTarget, system);
            }
        }
    }
}


