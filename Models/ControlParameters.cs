using System.Collections.Generic;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class ControlParameters
{
	public Dictionary<string, object>? Lookup = null;


	public ControlParameters() : base()
	{
	}
	public void Establish(string key, object value)
	{
		Lookup ??= new Dictionary<string, object>();
		Lookup[key] = value;
	}
	public string GetValue(string key, string def = "")
	{
		if (Lookup?.TryGetValue(key, out object? value) == true) return value?.ToString() ?? def;
		return def;
	}

	public object Find(string key)
	{
		if (Lookup?.TryGetValue(key, out object? value) == true)
			return value;
		return null!;
	}

	public ControlParameters CloneFrom(ControlParameters others)
	{
		Lookup ??= new Dictionary<string, object>();
		if (others.Lookup != null)
			foreach (var item in others.Lookup)
				Lookup[item.Key] = item.Value;

		return this;
	}

}
