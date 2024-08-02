using System.Text.Json;
using System.Text.Json.Serialization;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Services.Persistence;

public class PersistenceHandler
{
    private Persistence _persistence = new Persistence();
    private PersistenceSettings _settings = new PersistenceSettings();
    private string _filePath;

    public PersistenceHandler()
    {
    }

    private void setStuffRight(string directory)
    {
        var managerDirectory = Path.Combine(directory, "manager");
        if (!Directory.Exists(managerDirectory))
        {
            Directory.CreateDirectory(managerDirectory);
        }

        _filePath = Path.Combine(managerDirectory, "persistence.json");
        if (!File.Exists(_filePath))
        {
            _settings = new PersistenceSettings();
            // create persistence.json file
            _persistence = new Persistence
            {
                PersistenceSettings = _settings
            };
            var jsonString = JsonSerializer.Serialize(_persistence, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }
    }

    public void UpdateSettings(string directory, bool registerOnStartup , bool powerOnOnStartup)
    {
        setStuffRight(directory);
        _settings.RegisterOnStartup = registerOnStartup;
        _settings.PowerOnOnStartup = powerOnOnStartup;
        SaveSettings();
    }

    public PersistenceSettings? ReadSettings(string directory)
    {
        setStuffRight(directory);

        PersistenceSettings? settings = new PersistenceSettings();
        if (File.Exists(_filePath))
        {
            var settingContent = File.ReadAllText(_filePath);
            var persistence = JsonSerializer.Deserialize<Persistence>(settingContent);
            settings = persistence?.PersistenceSettings;
        }

        return settings;
    }

    private void SaveSettings()
    {
        _persistence.PersistenceSettings = _settings;
        var jsonString = JsonSerializer.Serialize(_persistence, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonString);
    }
}

public class PersistenceSettings
{
    [JsonPropertyName("RegisterOnStartup")]
    public bool RegisterOnStartup { get; set; } = false;

    [JsonPropertyName("PowerOnOnStartup")] public bool PowerOnOnStartup { get; set; } = false;
}

public class Persistence
{
    [JsonPropertyName("Settings")] public PersistenceSettings PersistenceSettings { get; set; } = null;
}