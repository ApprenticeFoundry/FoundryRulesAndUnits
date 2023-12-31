using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Extensions;

public static class FileHelpers
{
    public static Stream GenerateStream(this string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static bool FileExist(string path)
    {
        var found = File.Exists(path);

        if (found)
        {
            // $"exist {path}".WriteTrace();
        }
        else
        {
            $"DOES NOT exist {path}".WriteTrace();
        }

        return found;
    }

    public static string LocalPath(string directory, string filename)
    {
        string filePath = Path.Combine(directory, filename);
        return filePath;
    }

    public static string FullPath(string directory, string filename)
    {
        string path = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(path, directory, filename);
        return filePath;
    }

    public static string WriteData(string folder, string filename, string data)
    {
        try
        {
            //$"WriteData local {folder.ToUpper()}: {filename}".WriteTrace();

            string filePath = FullPath(folder, filename);
            File.WriteAllText(filePath, data);
            return data;
        }
        catch (Exception ex)
        {
            $"Error WriteData {filename}| {ex.Message}".WriteError()    ;
            throw;
        }
    }
    public static string ReadData(string folder, string filename)
    {
        try
        {
            $"ReadData local {folder.ToUpper()}: {filename}".WriteTrace();

            string filePath = FullPath(folder, filename);
            string data = File.ReadAllText(filePath);
            return data;
        }
        catch (Exception ex)
        {
            $"Error ReadData {filename}| {ex.Message}".WriteError();
            throw;
        }
    }

    public static List<T> ReadListFromFile<T>(string filename, string directory) where T : class
    {
        try
        {
            string filePath = FullPath(directory, filename);

            string text = File.ReadAllText(filePath);
            var result = CodingExtensions.HydrateList<T>(text, true);

            return result;
        }
        catch (Exception ex)
        {
            $"Error ReadListFromFile {filename}| {ex.Message}".WriteError();
            throw;
        }
    }

    public static List<T> WriteListToFile<T>(List<T> data, string filename, string directory) where T : class
    {
        try
        {
            string filePath = FullPath(directory, filename);

            var result = CodingExtensions.DehydrateList<T>(data, true);
            File.WriteAllText(filePath, result);

            return data;
        }
        catch (Exception ex)
        {
            $"Error WriteListToFile | {ex.Message}".WriteError();
            throw;
        }
    }

    public static T ReadObjectFromFile<T>(string filename, string directory) where T : class
    {
        try
        {
            string filePath = FullPath(directory, filename);

            string text = File.ReadAllText(filePath);
            var result = CodingExtensions.Hydrate<T>(text, true);

            return result;
        }
        catch (Exception ex)
        {
            $"Error ReadObjectFromFile | {ex.Message}".WriteError();
            throw;
        }
    }

    public static void WriteSetting<T>(T value) where T : class
    {
        var key = typeof(T).Name;
        var filename = $"{key}.json";

        try
        {
            string filePath = FullPath("config", filename);

            var result = CodingExtensions.Dehydrate<T>(value, false);
            File.WriteAllText(filePath, result);
        }
        catch (Exception ex)
        {
            $"Error WriteSetting | {ex.Message}".WriteError();
            throw;
        }
    }
}
