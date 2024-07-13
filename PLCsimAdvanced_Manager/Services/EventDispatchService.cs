using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Services;

public class EventDispatchService
{
    private readonly InstanceHandler _instanceHandler;
    private readonly ConnectionHandler _connectionHandler;
    private readonly PortHandler _portHandler;
    
    public EventDispatchService(InstanceHandler instanceHandler)
    {
        _instanceHandler = instanceHandler;
        SimulationRuntimeManager.OnConfigurationChanged += OnSoftwareConfigurationChanged;
    }
    
    private void OnSoftwareConfigurationChanged(ERuntimeConfigChanged e, uint p1, uint p2, int p3)
    {
        if (p3 == -1)
        {
            return;
        }

        switch (e)
        {
            case ERuntimeConfigChanged.InstanceRegistered:
                _instanceHandler.InstanceRegisteredCallback(p3);
                break;
            case ERuntimeConfigChanged.InstanceUnregistered:
                _instanceHandler.InstanceUnregisteredCallback(p3);
                break;
            case ERuntimeConfigChanged.ConnectionOpened:
                // Snackbar.Add($"Connection Opened to {p1}:{p2}", Severity.Success,
                // config => { config.HideIcon = true; });
                var ipRemoteRuntimeManager = p1;
                var portRemoteRuntimeManager = p2;
                break;
            case ERuntimeConfigChanged.ConnectionClosed:
                // Snackbar.Add($"Connection closed {p1}:{p2}", Severity.Success, config => { config.HideIcon = true; });
                var ipRemoteRuntimeManager_x = p1;
                var portRemoteRuntimeManager_x = p2;
                break;
            case ERuntimeConfigChanged.PortOpened:
                // Snackbar.Add($"Runtime Manager Port Opened {p1}", Severity.Success,
                // config => { config.HideIcon = true; });
                var openPort = p1;
                break;
            case ERuntimeConfigChanged.PortClosed:
                // Snackbar.Add($"Runtime Manager Port Closed", Severity.Success, config => { config.HideIcon = true; });
                break;
            case ERuntimeConfigChanged.NetworkModeChanged:
                // Snackbar.Add($"Network mode changed", Severity.Success, config => { config.HideIcon = true; });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e, null);
        }

    }
}