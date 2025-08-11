using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class DT_Part
{
	public string? Title { get; set; }
	public string? PartType { get; set; }
	public string? PromiseReference { get; set; }
	public string? StructureReference { get; set; }
	public string? ReferenceDesignation { get; set; }
	public string? PartNumber { get; set; }
	public string? SerialNumber { get; set; }
	public string? Version { get; set; }

	public DT_Part()
	{
	}

	public bool IsPartType(string type)
	{
		if (string.IsNullOrEmpty(PartType)) return false;
		return PartType.Matches(type);
	}

	public int StructureDepth()
	{
		if (string.IsNullOrEmpty(StructureReference)) return 0;
		var leg = StructureReference ?? "";
		return leg.Split('.').Length;
	}

	public static string ParentReference(string path)
	{
		if (string.IsNullOrEmpty(path)) return "";
		string[] list = path.Split('.');
		string result = string.Join(".", list, 0, list.Length - 1);
		return result;
	}
	public static string ParentName(string path)
	{
		if (string.IsNullOrEmpty(path)) return "";
		string[] list = path.Split('.') ?? new string[0];
		if (list.Length < 2) return "";
		return list[^2];
	}

	public static string Subpath(string path)
	{
		if (string.IsNullOrEmpty(path)) return "";
		string[] array = path.Split('.') ?? new string[0];
		if (array.Length < 2) return "";
		return string.Join(".", array.Skip(1));
	}

	public static string Parentpath(string path)
	{
		if (string.IsNullOrEmpty(path)) return "";
		string[] array = path.Split('.') ?? new string[0];
		if (array.Length < 2) return "";
		return string.Join(".", array.Take(array.Length - 1));
	}
	public static string RootName(string path)
	{
		if (string.IsNullOrEmpty(path)) return "";
		string[] list = path.Split('.') ?? new string[0]; ;
		return list[0];
	}
	public static string SelfName(string path)
	{
		if (string.IsNullOrEmpty(path)) return "";
		string[] list = path.Split('.') ?? new string[0]; ;
		if (list.Length < 1) return "";
		return list[^1];
	}

	public bool MatchStructure(DT_Part other)
	{
		return StructureReference == other.StructureReference;
	}

	public bool IsSubStructure(DT_Part parent)
	{
		if (string.IsNullOrEmpty(StructureReference)) return false;
		if (string.IsNullOrEmpty(parent.StructureReference)) return false;
		return StructureReference.StartsWith(parent.StructureReference);
	}

	public bool MatchPartNumber(DT_Part other)
	{
		return PartNumber == other.PartNumber;
	}

	public bool MatchSerialNumber(DT_Part other)
	{
		if (SerialNumber != other.SerialNumber) return false;
		return MatchPartNumber(other);
	}

	public bool MatchRefDes(DT_Part other)
	{
		return ReferenceDesignation == other.ReferenceDesignation;
	}

	public bool MatchVersion(DT_Part other)
	{
		if (Version != other.Version) return false;
		return MatchPartNumber(other);
	}

	public bool NearMatch(DT_Part other)
	{
		return MatchPartNumber(other) && MatchRefDes(other);
	}

	public bool CompleteMatch(DT_Part other)
	{
		return MatchVersion(other) && MatchRefDes(other);
	}

	public bool IsEmpty()
	{
		if (!string.IsNullOrEmpty(PartType)) return false;
		if (!string.IsNullOrEmpty(StructureReference)) return false;
		if (!string.IsNullOrEmpty(ReferenceDesignation)) return false;
		if (!string.IsNullOrEmpty(PartNumber)) return false;
		if (!string.IsNullOrEmpty(SerialNumber)) return false;
		if (!string.IsNullOrEmpty(Version)) return false;
		return true;
	}
	public DT_Part ShallowCopy()
	{
		var result = (DT_Part)this.MemberwiseClone();
		return result;
	}
	public string SerialName()
	{
		if (string.IsNullOrEmpty(SerialNumber))
			return PartName();

		return $"{PartName()} (SN:{SerialNumber})";
	}
	public string PartName()
	{
		if (string.IsNullOrEmpty(Version))
			return PartNumber ?? "";

		return $"{PartNumber}-{Version}";
	}

	public string RefName()
	{
		if (string.IsNullOrEmpty(ReferenceDesignation))
			return PartName();

		return $"{PartName()} ({ReferenceDesignation})";
	}

	public string PartTitle()
	{
		if (string.IsNullOrEmpty(Title))
			return RefName();

		return Title;
	}

	public string ComputeTitle()
	{
		var result = $"{Title} {PartName()}";

		if (!string.IsNullOrEmpty(ReferenceDesignation))
			result = $"{result} ({ReferenceDesignation})";

		if (!string.IsNullOrEmpty(SerialNumber))
			result = $"{result} (SN:{SerialNumber})";

		return result;
	}
}






