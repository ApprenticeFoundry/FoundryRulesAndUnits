namespace FoundryRulesAndUnits.Extensions;

public static class DestructureListExtensions {
    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {
#pragma warning disable CS8601 // Possible null reference assignment.
        first = list.Count > 0 ? list[0] : default; // or throw
#pragma warning restore CS8601 // Possible null reference assignment.
        rest = list.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
#pragma warning disable CS8601 // Possible null reference assignment.
        first = list.Count > 0 ? list[0] : default; // or throw
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
        second = list.Count > 1 ? list[1] : default; // or throw
#pragma warning restore CS8601 // Possible null reference assignment.
        rest = list.Skip(2).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest) {
#pragma warning disable CS8601 // Possible null reference assignment.
        first = list.Count > 0 ? list[0] : default; // or throw
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
        second = list.Count > 1 ? list[1] : default; // or throw
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
        third = list.Count > 2 ? list[2] : default; // or throw
#pragma warning restore CS8601 // Possible null reference assignment.
        rest = list.Skip(3).ToList();
    }
}


