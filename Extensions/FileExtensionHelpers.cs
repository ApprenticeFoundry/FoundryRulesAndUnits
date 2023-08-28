using System.Text;
// using Microsoft.AspNetCore.StaticFiles;
// using Microsoft.AspNetCore.Http;

namespace FoundryRulesAndUnits.Extensions;

public static class FileExtensionHelpers
{
    // private static FileExtensionContentTypeProvider _provider;
    // public static FileExtensionContentTypeProvider MIMETypeProvider()
    // {
    //     if (_provider == null)
    //     {
    //         FileExtensionHelpers._provider = new FileExtensionContentTypeProvider();

    //         foreach (KeyValuePair<string,string> c in MIMETypeData())
    //         {
    //             _provider.Mappings[c.key] = c.Value;
    //         }
    //     }

    //     return _provider;
    // }

    public static Dictionary<string,string> MIMETypeData()
    {
        var data = new Dictionary<string, string>()
        {
            { ".babylon","application/babylon" },
            { ".pdf","application/pdf" },
            { ".obj", "model/obj"},
            { ".mtl", "model/mtl"},
            { ".fbx", "model/fbx"},
            { ".gltf", "model/gltf"},
            { ".glb", "model/glb"},
            { ".txt", "text/plain"},
            { ".mp4", "video/mp4"},
            { ".mov", "video/mov"},
            { ".mp3", "video/mp3"},
            { ".doc", "application/vnd.ms-word"},
            { ".docx", "application/vnd.ms-word"},
            { ".xls", "application/vnd.ms-excel"},
            { ".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
            { ".png", "image/png"},
            { ".jpg", "image/jpeg"},
            { ".jpeg", "image/jpeg"},
            { ".gif", "image/gif"},
            { ".csv", "text/csv"}
        };
        return data;
    }

    // public static string GetMIMEType(this string fileName)
    // {
    //     var provider = FileExtensionHelpers.MIMETypeProvider();

    //     if (!provider.TryGetContentType(fileName, out string? contentType))
    //     {
    //         contentType = "application/octet-stream";
    //     }
    //     return contentType;
    // }

    // public static async Task<string> ReadAsStringAsync(this IFormFile file)
    // {
    //     var result = new StringBuilder();
    //     using (var reader = new StreamReader(file.OpenReadStream()))
    //     {
    //         while (reader.Peek() >= 0)
    //             result.AppendLine(await reader.ReadLineAsync());
    //     }
    //     return result.ToString();
    // }
}
