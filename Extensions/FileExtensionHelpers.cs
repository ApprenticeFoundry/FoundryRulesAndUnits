using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;

namespace DTARServer.Helpers;

public static class FileExtensionHelpers
{
    private static FileExtensionContentTypeProvider _provider;
    public static FileExtensionContentTypeProvider MIMETypeProvider()
    {
        if (_provider == null)
        {
            FileExtensionHelpers._provider = new FileExtensionContentTypeProvider();
            _provider.Mappings[".babylon"] = "application/babylon";
            _provider.Mappings[".pdf"] = "application/pdf";
            _provider.Mappings[".obj"] = "model/obj";
            _provider.Mappings[".mtl"] = "model/mtl";
            _provider.Mappings[".fbx"] = "model/fbx";
            _provider.Mappings[".gltf"] = "model/gltf";
            _provider.Mappings[".glb"] = "model/glb";
            _provider.Mappings[".txt"] = "text/plain";
            _provider.Mappings[".mp4"] = "video/mp4";
            _provider.Mappings[".mov"] = "video/mov";
            _provider.Mappings[".mp3"] = "video/mp3";
            _provider.Mappings[".pdf"] = "application/pdf";
            _provider.Mappings[".doc"] = "application/vnd.ms-word";
            _provider.Mappings[".docx"] = "application/vnd.ms-word";
            _provider.Mappings[".xls"] = "application/vnd.ms-excel";
            _provider.Mappings[".xlsx"] = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            _provider.Mappings[".png"] = "image/png";
            _provider.Mappings[".jpg"] = "image/jpeg";
            _provider.Mappings[".jpeg"] = "image/jpeg";
            _provider.Mappings[".gif"] = "image/gif";
            _provider.Mappings[".csv"] = "text/csv";
        }

        return _provider;
    }

    public static string GetMIMEType(this string fileName)
    {
        var provider = FileExtensionHelpers.MIMETypeProvider();

        if (!provider.TryGetContentType(fileName, out string? contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    public static async Task<string> ReadAsStringAsync(this IFormFile file)
    {
        var result = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(await reader.ReadLineAsync());
        }
        return result.ToString();
    }
}
