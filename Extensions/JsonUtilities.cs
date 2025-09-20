using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Extensions
{
    /// <summary>
    /// JSON serialization utilities for Foundry types
    /// Replaces UnitSpec.JsonHydrateOptions functionality
    /// </summary>
    public static class JsonUtilities
    {
        public class FoundryNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name;
        }

        private static readonly JsonSerializerOptions WithFields = new()
        {
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new FoundryNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        private static readonly JsonSerializerOptions WithoutFields = new()
        {
            IncludeFields = false,
            IgnoreReadOnlyFields = true,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new FoundryNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        /// <summary>
        /// Get JSON serialization options for Foundry types
        /// Replaces UnitSpec.JsonHydrateOptions(bool includeFields)
        /// </summary>
        /// <param name="includeFields">Whether to include fields in serialization</param>
        /// <returns>Configured JsonSerializerOptions</returns>
        public static JsonSerializerOptions CreateOptions(bool includeFields = false)
        {
            return includeFields ? WithFields : WithoutFields;
        }

        /// <summary>
        /// Legacy method name for backward compatibility
        /// </summary>
        [System.Obsolete("Use CreateOptions() instead")]
        public static JsonSerializerOptions JsonHydrateOptions(bool includeFields = false)
        {
            return CreateOptions(includeFields);
        }
    }
}