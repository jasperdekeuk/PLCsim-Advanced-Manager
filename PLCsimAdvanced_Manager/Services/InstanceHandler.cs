using System.Collections.Concurrent;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Services;

public class InstanceChangedEventArgs : EventArgs
{
    public string Message { get; }

    public InstanceChangedEventArgs(string message)
    {
        Message = message;
    }
}

public class InstanceHandler
{
    public List<IInstance> _instances = new(); // todo: concurrentbag might make sense here?

    public event EventHandler<InstanceChangedEventArgs> OnInstanceChanged;
    public event EventHandler<Exception> OnIssue;
    public event EventHandler OnLogsUpdated;

    static string SnapshotFolder = "Snapshots";


    public record _log(DateTime Timestamp, string Message);

    private ConcurrentDictionary<int, List<_log>> logs = new();

    public InstanceHandler()
    {
        GetExistingInstances();
    }

    private void GetExistingInstances()
    {
        try
        {
            foreach (var instanceInfo in SimulationRuntimeManager.RegisteredInstanceInfo)
            {
                InstanceRegisteredCallback(instanceInfo.ID);
            }
        }
        catch (Exception e)
        {
            OnIssue?.Invoke(this, e);
        }
    }

    public void InstanceRegisteredCallback(int id)
    {
        var instance = SimulationRuntimeManager.CreateInterface(id);
        _instances.Add(instance);
        OnInstanceChanged?.Invoke(this, new InstanceChangedEventArgs($"Instance {instance.Name} registered"));

        instance.OnOperatingStateChanged += OnOperatingStateChanged;
        instance.OnIPAddressChanged += OnIpAddressChanged;
        instance.OnHardwareConfigChanged += OnHardwareConfigChanged;
        instance.OnSoftwareConfigurationChanged += OnSoftwareConfigurationChanged;
    }

    private void OnSoftwareConfigurationChanged(IInstance instance, SOnSoftwareConfigChangedParameter event_param)
    {
        OnInstanceChanged?.Invoke(this, new InstanceChangedEventArgs($"instance {instance.Name} software config changed"));
        AddLog(instance.ID, $"{instance.Name} software config changed");
    }

    private void OnHardwareConfigChanged(IInstance in_sender, ERuntimeErrorCode in_errorcode, DateTime in_datetime)
    {
        OnInstanceChanged?.Invoke(this, new InstanceChangedEventArgs($"instance {in_sender.Name} hardware config changed"));
        AddLog(in_sender.ID, $"{in_sender.Name} hardware config changed");
    }

    private void OnOperatingStateChanged(IInstance inst, ERuntimeErrorCode error, DateTime dateTime,
        EOperatingState operatingState, EOperatingState operatingState2)
    {
        OnInstanceChanged?.Invoke(this,
            new InstanceChangedEventArgs($"{inst.Name} changed from {operatingState} to {operatingState2}"));
        AddLog(inst.ID, $"{inst.Name} changed from {operatingState} to {operatingState2}");
    }

    private void OnIpAddressChanged(IInstance inst, ERuntimeErrorCode error, DateTime dateTime, byte inInterfaceId,
        SIPSuite4 inSip)
    {
        OnInstanceChanged?.Invoke(this, new InstanceChangedEventArgs($"{inst.Name} IP setting changed"));
        AddLog(inst.ID, $"{inst.Name} IP setting changed");
    }
    
    public void MemoryReset(int id)
    {
        var instance = _instances.SingleOrDefault(v => id == v.ID);
        if (instance != null)
        {
            instance.MemoryReset();
        }
    }

    public void InstanceUnregisteredCallback(int id)
    {
        var instance = _instances.SingleOrDefault(v => id == v.ID || v.ID == -1);
        _instances.Remove(instance);
        OnInstanceChanged?.Invoke(this, new InstanceChangedEventArgs($"Instance {id} unregistered"));
    }

    public void UnregisterInstance(IInstance instance, bool CleanupStoragePath)
    {
        try
        {
            if (CleanupStoragePath)
            {
                CheckAndRemoveSnapshotDirectory(instance.StoragePath);

                instance.CleanupStoragePath();
            }

            _instances.Remove(_instances.SingleOrDefault(v => instance.Name == v.Name));
        }
        catch (Exception e)
        {
            OnIssue?.Invoke(this, e);
        }

        instance.UnregisterInstance();
    }

    void CheckAndRemoveSnapshotDirectory(string storagePath)
    {
        var path = Path.Combine(storagePath, SnapshotFolder);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    private void AddLog(int id, string logMessage)
    {
        _log log = new _log(DateTime.Now, logMessage);
        logs.AddOrUpdate(id, new List<_log> { log }, (key, oldValue) =>
        {
            // Ensure the list acts as a FIFO queue with a max capacity of 15
            if (oldValue.Count >= 15)
            {
                oldValue.RemoveAt(0); // Remove the oldest log
            }

            oldValue.Add(log);
            return oldValue;
        });
        OnLogsUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void GetLogs(int id, out List<_log> logList)
    {
        if (logs.TryGetValue(id, out var tempList))
        {
            logList = tempList.AsEnumerable().Reverse().ToList();
        }
        else
        {
            logList = new List<_log>();
        }
    }
}