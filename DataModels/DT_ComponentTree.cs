

using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;

#nullable enable

namespace FoundryRulesAndUnits.Models;

public class DT_ComponentTree<V> : DT_Title where V : DT_Ingredient
{
    public V? Item = null;
    public List<DT_ComponentTree<V>>? Children = null;

    private readonly List<V> _sourceChildren;
    private DT_ComponentTree<V>? _parent = null;

    public int Level = 0;
    public int Index = 0;
    public string Path = "";
    public string Indent = "";
    public double Key;



    public DT_ComponentTree(V node, List<V> sourceChildren, int index, int level)
    {
        this.Item = node;
        this._sourceChildren = sourceChildren;

        this.Guid = node.Guid;
        this.Level = level;
        this.Index = index;
        this.Name = Item?.Part?.ComputeTitle() ?? "Unnamed";
        this.Key = level + index / 100;

        var spaces = "_________________________________________________________________________"[..(2 * (level - 1))];
        this.Indent = $"{spaces}{level}.{index}";
        this.Title = $"{spaces}{this.Name}";
    }

    public List<DT_ComponentTree<V>>? GetChildren()
    {
        return Children;
    }
    public DT_ComponentTree<V>? GetParent()
    {
        return _parent;
    }
    public string ComputePath()
    {
        if (!string.IsNullOrWhiteSpace(Path)) return Path;

        Path = Index.ToString().PadLeft(2, '0');
        if (_parent != null)
        {
            Path = $"{_parent.ComputePath()}.{Path}";
        }
        return Path;
    }
    public V? AsBOM()
    {
        var result = Item?.ShallowCopy() as V;
        if (result == null) return null;
        result.Title = Title;
        result.Description = $"{ComputePath()}: {Item?.Part?.ComputeTitle()}";
        return result;
    }

    public bool MatchesNode(DT_ComponentTree<V> node)
    {
        var myPart = Item?.Part;
        var otherPart = node.Item?.Part;
        if (myPart == null || otherPart == null) return false;
        if (myPart.PartNumber == null || otherPart.PartNumber == null) return false;

        return myPart.PartNumber.Matches(otherPart.PartNumber);
    }

    //  https://github.com/force-net/DeepCloner
    public List<V>? SourceChildren(bool clone)
    {
        if (clone)
        {
            var result = _sourceChildren.Select(item => item.ShallowCopy()).ToList();
            return result as List<V>;
        }
        return _sourceChildren;
    }

    public DT_ComponentTree<V> AddChildNode(DT_ComponentTree<V> child)
    {
        this.Children ??= new List<DT_ComponentTree<V>>();
        this.Children.Add(child);
        child._parent = this;
        return child;
    }

    public List<DT_ComponentTree<V>> CollectLeafNodes(List<DT_ComponentTree<V>> list)
    {
        if (Children == null || Children.Count == 0)
            list.Add(this);
        else
            Children?.ForEach(child => child.CollectLeafNodes(list));

        return list;
    }

    public DT_ComponentTree<V> SimplifyForExport()
    {
        Path = ComputePath();
        Children?.ForEach(child => child.SimplifyForExport());

        return this;
    }

    public List<DT_ComponentTree<V>> CollectAllNodes(List<DT_ComponentTree<V>> list)
    {
        list.Add(this);
        Children?.ForEach(child => child.CollectAllNodes(list));

        return list;
    }

    public List<V> CollectAllComponents(List<V> list)
    {
        if (Item != null)
            list.Add(this.Item);

        Children?.ForEach(child => child.CollectAllComponents(list));

        return list;
    }

    public DT_ComponentTree<V>? ApplyNodeAsTemplate(DT_ComponentTree<V> root, int depth)
    {
        //DO NOT ADD CHILD FROM TEMPLATE IF IT ALREADY EXIST !!

        if (root == null) return null;
        Children ??= new List<DT_ComponentTree<V>>();

        var found = Children.Find(child => child.MatchesNode(root));
        if (found == null)
        {
            var newestChild = root.Item?.ShallowCopy() as V;
            if (newestChild != null)
            {
                newestChild.Guid = System.Guid.NewGuid().ToString();
                newestChild.ParentName = this.Item?.Part?.PartNumber;

                var more = root.SourceChildren(false);
                var node = new DT_ComponentTree<V>(newestChild, more!, Children.Count + 1, depth + 1);
                AddChildNode(node);
            }

            //root.Children?.ForEach(child => node.ApplyNodeAsTemplate(child, depth + 1));
        }

        return root;
    }
}
