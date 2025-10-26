using FoundryRulesAndUnits.Extensions;
namespace FoundryRulesAndUnits.Models;

public record TreeNodeAction(string Name, string Style, Action Action);

public record TreeNodeReference(StatusBitArray Status, ITreeNode Node);

public interface ITreeNode
{
    public string GetUniqueId(); // Stable unique identifier for caching

    public bool GetIsExpanded();
    public bool SetExpanded(bool value);

    public bool GetIsSelected();
    public bool SetSelected(bool value);

    public string GetTreeNodeTitle();
    public IEnumerable<TreeNodeAction>? GetTreeNodeActions();
    public IEnumerable<ITreeNode> GetTreeChildren();

    public bool HasChildren() => GetTreeChildren()?.Any() ?? false;
    public bool IsCollapsed() => !GetIsExpanded();
    public bool ToggleExpanded() => SetExpanded(!GetIsExpanded());
    public bool ToggleSelected() => SetSelected(!GetIsSelected());

    public void PrintToConsole(int indent = 1)
    {
        $"{GetTreeNodeTitle()}".WriteSuccess(indent);
        foreach (var child in GetTreeChildren())
            child.PrintToConsole(indent + 1);
    }
}

// public static class TreeNodeExtensions
// {
//     public static IEnumerable<ITreeNode> GetAllDescendants(this ITreeNode node)
//     {
//         var stack = new Stack<ITreeNode>();
//         stack.Push(node);
//         while (stack.Any())
//         {
//             var current = stack.Pop();
//             yield return current;
//             foreach (var child in current.GetTreeChildren())
//                 stack.Push(child);
//         }
//     }

//     public static TreeNodeAction AddAction(this List<TreeNodeAction> list, string name, string style, Action action)
//     {
//         var nodeaction = new TreeNodeAction(name, style, action);
//         list.Add(nodeaction);
//         return nodeaction;
//     }

// }



