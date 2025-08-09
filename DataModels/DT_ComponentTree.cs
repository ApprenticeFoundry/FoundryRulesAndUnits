

using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;

#nullable enable

namespace FoundryRulesAndUnits.Models;

public class DT_ComponentTree<V> : DT_Title where V : DT_Ingredient
{
    public V? item = null;
    public List<DT_ComponentTree<V>>? children = null;

    private readonly List<V> _sourceChildren;
    private DT_ComponentTree<V>? _parent = null;

    public int level = 0;
    public int index = 0;
    public string path = "";
    public string indent = "";
    public double key;



    public DT_ComponentTree(V node, List<V> sourceChildren, int index, int level)
    {
        this.item = node;
        this._sourceChildren = sourceChildren;

        this.guid = node.guid;
        this.level = level;
        this.index = index;
        this.name = item?.Part?.ComputeTitle() ?? "Unnamed";
        this.key = level + index / 100;

        var spaces = "_________________________________________________________________________"[..(2 * (level - 1))];
        this.indent = $"{spaces}{level}.{index}";
        this.Title = $"{spaces}{this.name}";
    }

    public List<DT_ComponentTree<V>>? GetChildren()
    {
        return children;
    }
    public DT_ComponentTree<V>? GetParent()
    {
        return _parent;
    }
    public string ComputePath()
    {
        if (!string.IsNullOrWhiteSpace(path)) return path;

        path = index.ToString().PadLeft(2, '0');
        if (_parent != null)
        {
            path = $"{_parent.ComputePath()}.{path}";
        }
        return path;
    }
    public V? AsBOM()
    {
        var result = item?.ShallowCopy() as V;
        if (result == null) return null;
        result.Title = Title;
        result.Description = $"{ComputePath()}: {item?.Part?.ComputeTitle()}";
        return result;
    }

    public bool MatchesNode(DT_ComponentTree<V> node)
    {
        var myPart = item?.Part;
        var otherPart = node.item?.Part;
        if (myPart == null || otherPart == null) return false;
        if (myPart.partNumber == null || otherPart.partNumber == null) return false;

        return myPart.partNumber.Matches(otherPart.partNumber);
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
        this.children ??= new List<DT_ComponentTree<V>>();
        this.children.Add(child);
        child._parent = this;
        return child;
    }

    public List<DT_ComponentTree<V>> CollectLeafNodes(List<DT_ComponentTree<V>> list)
    {
        if (children == null || children.Count == 0)
            list.Add(this);
        else
            children?.ForEach(child => child.CollectLeafNodes(list));

        return list;
    }

    public DT_ComponentTree<V> SimplifyForExport()
    {
        path = ComputePath();
        children?.ForEach(child => child.SimplifyForExport());

        return this;
    }

    public List<DT_ComponentTree<V>> CollectAllNodes(List<DT_ComponentTree<V>> list)
    {
        list.Add(this);
        children?.ForEach(child => child.CollectAllNodes(list));

        return list;
    }

    public List<V> CollectAllComponents(List<V> list)
    {
        if (item != null)
            list.Add(this.item);

        children?.ForEach(child => child.CollectAllComponents(list));

        return list;
    }

    public DT_ComponentTree<V>? ApplyNodeAsTemplate(DT_ComponentTree<V> root, int depth)
    {
        //DO NOT ADD CHILD FROM TEMPLATE IF IT ALREADY EXIST !!

        if (root == null) return null;
        children ??= new List<DT_ComponentTree<V>>();

        var found = children.Find(child => child.MatchesNode(root));
        if (found == null)
        {
            var newestChild = root.item?.ShallowCopy() as V;
            if (newestChild != null)
            {
                newestChild.guid = Guid.NewGuid().ToString();
                newestChild.ParentName = this.item?.Part?.partNumber;

                var more = root.SourceChildren(false);
                var node = new DT_ComponentTree<V>(newestChild, more!, children.Count + 1, level + 1);
                AddChildNode(node);
            }

            //root.children?.ForEach(child => node.ApplyNodeAsTemplate(child, depth + 1));
        }

        return root;
    }
}
