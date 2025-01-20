using System.Text.Json;
using System.Text.Json.Serialization;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Services.Persistence;

public class PersistenceHandler
{
    private Persistence _persistence = new Persistence();
    private PersistenceSettings _settings = new PersistenceSettings();
    private NodegraphJson _nodegraph = new NodegraphJson();
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
            _nodegraph = new NodegraphJson();
            // create persistence.json file
            _persistence = new Persistence
            {
                PersistenceSettings = _settings,
                NodegraphJson = _nodegraph
            };
            var jsonString = JsonSerializer.Serialize(_persistence, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }
        else
        {
            var settingContent = File.ReadAllText(_filePath);
            _persistence = JsonSerializer.Deserialize<Persistence>(settingContent);
            _settings = _persistence.PersistenceSettings;
            _nodegraph = _persistence.NodegraphJson;
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
    
    public void SaveNodegraphJson(string nodegraphJson,string directory)
    {
        setStuffRight(directory);

        _persistence.NodegraphJson = new NodegraphJson { NodegraphJsonString = nodegraphJson };
        var jsonString = JsonSerializer.Serialize(_persistence, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonString);
    }
    
    public string ReadNodegraphJson(string directory)
    {
        setStuffRight(directory);
        
        if (File.Exists(_filePath))
        {
            var settingContent = File.ReadAllText(_filePath);
            var persistence = JsonSerializer.Deserialize<Persistence>(settingContent);
            return persistence?.NodegraphJson?.NodegraphJsonString;
        }

        return string.Empty;
    }
}

public class PersistenceSettings
{
    [JsonPropertyName("RegisterOnStartup")]
    public bool RegisterOnStartup { get; set; } = false;

    [JsonPropertyName("PowerOnOnStartup")] public bool PowerOnOnStartup { get; set; } = false;
}

public class NodegraphJson
{
    [JsonPropertyName("NodegraphJson")] public string NodegraphJsonString { get; set; } = string.Empty;
}

public class Persistence
{
    [JsonPropertyName("Settings")] public PersistenceSettings PersistenceSettings { get; set; } = null;
    [JsonPropertyName("NodegraphJson")] public NodegraphJson NodegraphJson { get; set; } = null;
}