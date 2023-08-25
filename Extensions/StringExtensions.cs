

namespace FoundryRulesAndUnits.Extensions;

public static class StringExtensions
{

    public static T GetValue<T>(this String value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }
    // public static T? Blank<T>(this T me)
    // {
    //     var tot = typeof(T);
    //     return tot.IsValueType
    //       ? default
    //       : (T)Activator.CreateInstance(tot)
    //       ;
    // }

	public static bool IsNullOrEmpty(this string str1)
	{
		var result = str1 == null || string.IsNullOrWhiteSpace(str1);
		return result;
	}

    public static string RemoveSpace(this string str) 
    {
        return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }



    public static bool Matches(this string str1, string str2)
    {
        if (str1.IsNullOrEmpty() && str2.IsNullOrEmpty()) return true;
        var result = str1?.ToLower() == str2?.ToLower();
        return result;
    }



    public static string CleanPartNumber(this string str1)
    {
        if ( string.IsNullOrEmpty(str1) ) return "";  
        var part = str1.Trim();
        part = part.Replace("}", "");
        part = part.Replace("]", "");
        part = part.Replace(")", "");
        part = part.Replace(";", "");
        part = part.Replace(",", "");
        part = part.Replace(".", "");
        return part;
    }

    public static string CleanAddress(this string str1)
    {
        if ( string.IsNullOrEmpty(str1) ) return "";  
        var part = str1.Trim();
        part = part.Replace("'", "");
        part = part.Replace(";", "");
        part = part.Replace(",", "");
        return part;
    }

    public static string CleanToFilename(this string source)
    {
        if ( string.IsNullOrEmpty(source) ) return "";  
        var filename = source.Trim();

        filename = filename.Replace('/', '-');
        filename = filename.Replace('"', '-');
        filename = filename.Replace(' ', '-');
        filename = filename.Replace(',', '-');
         filename = filename.Replace(':', '-');
        filename = filename.Trim(Path.GetInvalidFileNameChars());
        filename = filename.Trim(Path.GetInvalidPathChars());
        return filename;
    }

    public static string InsertSerialNumber(this string description, string serialNumber)
    {
        if ( !description.Contains("(SN:"))
            return description;

        var list = description.Split("(SN:");
        var rest = list[1].Split(")");
        var result = $"{list[0]} (SN: {serialNumber}) {rest[1]}";
        return result;
    }

    public static bool BeginsWith(this string str1, string str2)
    {
		if (str1.IsNullOrEmpty() && str2.IsNullOrEmpty()) return true;
        var result = str1.ToLower().StartsWith(str2.ToLower());
        return result;
    }

     public static bool ContainsNoCase(this string str1, string str2)
    {
		if (str1.IsNullOrEmpty() && str2.IsNullOrEmpty()) return true;
        var result = str1.ToLower().Contains(str2.ToLower());
        return result;
    }   

    public static bool ContainsAny(this string str1, List<string> collection)
    {
		if (str1.IsNullOrEmpty() && collection.Count == 0) return true;
        if (str1.IsNullOrEmpty()) return false;
        foreach (var item in collection)
        {
            var result = str1.ContainsNoCase(item);
            if ( result ) return true;
        }
        return false;
    } 


    public static string CreateInternalName(this string sName)
    {
        string sAllow = @".[]_";  //for names of Visio Cells and references

        var sText = new System.Text.StringBuilder();
        foreach (char c in sName.Trim().ToCharArray())
        {
            if (char.IsLetterOrDigit(c))
                sText.Append(c);
            else if (c == (char)' ')
                sText.Append('_');
            else if (sAllow.IndexOf(c) != -1)
                sText.Append(c);
        }
        string sString = sText.ToString();
        return sString;
    }
}
