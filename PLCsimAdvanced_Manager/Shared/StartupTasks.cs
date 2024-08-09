using System.Text.Json;
using System.Text.Json.Serialization;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Shared;

public static class StartupTasks
{
    public static async Task GetPersistantSettings()
    {
        var directories = Directory.GetDirectories(@SimulationRuntimeManager.DefaultStoragePath);
        foreach (var directory in directories)
        {
            var managerDirectory = Path.Combine(directory, "manager");
            if (Directory.Exists(managerDirectory))
            {
                var persistentSettingsFile = Path.Combine(managerDirectory, "persistence.json");
                if (File.Exists(persistentSettingsFile))
                {
                    var settingContent = File.ReadAllText(persistentSettingsFile);
                    var persistence = JsonSerializer.Deserialize<Persistence>(settingContent);
                    var settings = persistence?.PersistenceSettings;

                    // Use the autoStartup boolean value in your logic
                    if (settings?.RegisterOnStartup == true)
                    {
                        var instanceName = Path.GetFileName(directory);
                        try
                        {
                            var instance = SimulationRuntimeManager.RegisterInstance(instanceName);

                            if (settings.PowerOnOnStartup)
                            {
                                instance.PowerOn();
                                
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"There was an issue with registering the instance [{instanceName}]: {e.Message}");
                        
                        }
                    }
                }
            }

        }
    }

}


public class PersistenceSettings
{
    [JsonPropertyName("RegisterOnStartup")] public bool RegisterOnStartup { get; set; } = false;
    [JsonPropertyName("PowerOnOnStartup")] public bool PowerOnOnStartup { get; set; } = false;
}

public class Persistence
{
    [JsonPropertyName("Settings")] public PersistenceSettings PersistenceSettings { get; set; } = null;

}