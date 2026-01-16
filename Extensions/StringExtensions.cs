using FoundryMicroCore.Core.Extensions;

namespace FoundryRulesAndUnits.Extensions;

/// <summary>
/// Domain-specific string extensions for FoundryRulesAndUnits.
/// General-purpose string extensions have been moved to FoundryMicroCore.Core.Extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Clean part number for manufacturing domain use
    /// </summary>
    public static string CleanPartNumber(this string str1)
    {
        if (str1.IsNullOrEmpty())
            return str1;

        var result = str1.ToUpper();
        result = result.Replace("-", "");
        result = result.Replace("_", "");
        result = result.Replace(" ", "");
        result = result.Replace(".", "");

        return result;
    }

    /// <summary>
    /// Clean address for manufacturing/logistics domain use
    /// </summary>
    public static string CleanAddress(this string str1)
    {
        if (str1.IsNullOrEmpty())
            return str1;
        
        var result = str1.Replace("  ", " ").Trim();
        return result;
    }

    /// <summary>
    /// Insert serial number into description for manufacturing domain
    /// </summary>
    public static string InsertSerialNumber(this string description, string serialNumber)
    {
        if (description.IsNullOrEmpty() || serialNumber.IsNullOrEmpty())
            return description;
        
        return $"{description} (SN: {serialNumber})";
    }

    /// <summary>
    /// Create internal name following domain-specific conventions
    /// </summary>
    public static string CreateInternalName(this string sName)
    {
        if (sName.IsNullOrEmpty())
            return sName;

        var result = sName.ToUpper();
        result = result.Replace(" ", "_");
        result = result.Replace("-", "_");
        result = result.Replace(".", "_");
        
        // Remove consecutive underscores
        while (result.Contains("__"))
        {
            result = result.Replace("__", "_");
        }
        
        return result.Trim('_');
    }
}
