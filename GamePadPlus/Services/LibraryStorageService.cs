using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using GamePadPlus.Models;

namespace GamePadPlus.Services;

public class LibraryStorageService
{
    private const string ApplicationFolderName = "GamePadPlus";
    private const string LibraryFileName = "library.json";
    private const string CoversFolderName = "Covers";

    public string GetDataFolder()
    {
        string localAppDataFolder = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData
        );

        return Path.Combine(localAppDataFolder, ApplicationFolderName);
    }

    public string GetLibraryFilePath()
    {
        return Path.Combine(GetDataFolder(), LibraryFileName);
    }

    public string GetCoversFolder()
    {
        return Path.Combine(GetDataFolder(), CoversFolderName);
    }

    public void EnsureStorageExists()
    {
        Directory.CreateDirectory(GetDataFolder());
        Directory.CreateDirectory(GetCoversFolder());
    }

    public void SaveLibrary(IEnumerable<Game> games)
    {
        EnsureStorageExists();
        string json = JsonSerializer.Serialize(games, new JsonSerializerOptions
        {
            WriteIndented = true
        }
       );
        File.WriteAllText(GetLibraryFilePath(), json);
    }

    public List<Game> LoadLibrary()
    {
        EnsureStorageExists();

        string filePath = GetLibraryFilePath();

        if (!File.Exists(filePath))
        {
            return new List<Game>();
        }

        string json = File.ReadAllText(filePath);

        List<Game>? games = JsonSerializer.Deserialize<List<Game>>(json);

        return games ?? new List<Game>();
    }
}