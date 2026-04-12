using FoundryMicroCore.Core.Extensions;

namespace FoundryRulesAndUnits.Extensions;

/// <summary>
/// Manufacturing and logistics domain-specific string extensions.
/// Handles part numbers, serial numbers, addresses, and internal naming conventions.
/// General-purpose string extensions are in FoundryMicroCore.Core.Extensions.
/// </summary>
public static class ManufacturingStringExtensions
{
    /// <summary>
    /// Clean part number for manufacturing domain use
    /// </summary>
    /// <param name="partNumber">The part number to clean</param>
    /// <returns>Cleaned part number in uppercase with special characters removed</returns>
    public static string CleanPartNumber(this string? partNumber) => partNumber switch
    {
        null or "" => string.Empty,
        _ => partNumber.ToUpper()
                      .Replace("-", "")
                      .Replace("_", "")
                      .Replace(" ", "")
                      .Replace(".", "")
    };

    /// <summary>
    /// Clean address for manufacturing/logistics domain use
    /// </summary>
    /// <param name="address">The address to clean</param>
    /// <returns>Cleaned address with normalized spacing</returns>
    public static string CleanAddress(this string? address) => address switch
    {
        null or "" => string.Empty,
        _ => address.Replace("  ", " ").Trim()
    };

    /// <summary>
    /// Insert serial number into description for manufacturing domain
    /// </summary>
    /// <param name="description">The base description</param>
    /// <param name="serialNumber">The serial number to insert</param>
    /// <returns>Description with serial number appended</returns>
    public static string InsertSerialNumber(this string? description, string? serialNumber) =>
        (description, serialNumber) switch
        {
            (null or "", _) => description ?? string.Empty,
            (_, null or "") => description,
            _ => $"{description} (SN: {serialNumber})"
        };

    /// <summary>
    /// Create internal name following domain-specific conventions
    /// </summary>
    /// <param name="name">The source name to convert</param>
    /// <returns>Internal name with underscores replacing special characters</returns>
    public static string CreateInternalName(this string? name) => name switch
    {
        null or "" => string.Empty,
        _ => name.ToUpper()
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace(".", "_")
                .Split('_', StringSplitOptions.RemoveEmptyEntries)
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .Aggregate(string.Empty, (acc, part) => string.IsNullOrEmpty(acc) ? part : $"{acc}_{part}")
    };
}
