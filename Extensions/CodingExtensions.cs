using System.Text.Json;
using System.Text.Json.Nodes;
using FoundryMicroCore.Core.Extensions;
using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Extensions;

/// <summary>
/// Domain-specific coding extensions for FoundryRulesAndUnits.
/// General-purpose extensions have been moved to FoundryMicroCore.Core.Extensions.
/// </summary>
public static class CodingExtensions
{
	/// <summary>
	/// Domain-specific wrapper hydration for FoundryRulesAndUnits types
	/// </summary>
	public static ContextWrapper<T> HydrateWrapper<T>(string target, bool includeFields) where T : class
	{
		using var stream = new MemoryStream();
		using var writer = new Utf8JsonWriter(stream);
		var node = JsonNode.Parse(target);
		node?.WriteTo(writer);
		writer.Flush();

		var options = JsonUtilities.CreateOptions(includeFields);
		var result = JsonSerializer.Deserialize<ContextWrapper<T>>(stream.ToArray(), options) as ContextWrapper<T>;

		return result!;
	}

	/// <summary>
	/// Domain-specific wrapper dehydration for FoundryRulesAndUnits types
	/// </summary>
	public static string DehydrateWrapper<T>(ContextWrapper<T> target, bool includeFields) where T : class
	{
		var options = JsonUtilities.CreateOptions(includeFields);
		var result = JsonSerializer.Serialize(target, options);
		return result!;
	}

	/// <summary>
	/// Copy non-null fields from source to target (domain-specific for DT_Part)
	/// </summary>
	public static void CopyNonNullFields(this DT_Part source, DT_Part target)
	{
		if (source.Title != null) target.Title = source.Title;
		if (source.PartType != null) target.PartType = source.PartType;
		if (source.PartNumber != null) target.PartNumber = source.PartNumber;
		if (source.PromiseReference != null) target.PromiseReference = source.PromiseReference;
		if (source.StructureReference != null) target.StructureReference = source.StructureReference;
		if (source.ReferenceDesignation != null) target.ReferenceDesignation = source.ReferenceDesignation;
		if (source.SerialNumber != null) target.SerialNumber = source.SerialNumber;
		if (source.Version != null) target.Version = source.Version;
		// Add other fields as needed
	}

	// Additional domain-specific methods can be added here as needed
}
