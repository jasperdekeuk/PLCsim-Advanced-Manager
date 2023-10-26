using MudBlazor;
using PLCsimAdvanced_Manager.Components;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Pages;

public partial class PlcOverview
{
    private List<IInstance>? instances;
    private IInstance? _selectedInstance;
    private MudTable<IInstance> mudTable;
    private bool IsEditingCommunicationInterface = false;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SimulationRuntimeManager.OnConfigurationChanged += OnSoftwareConfigurationChanged;
        instances = new List<IInstance>();
        var instancesInfo = SimulationRuntimeManager.RegisteredInstanceInfo;
        foreach (var info in instancesInfo)
        {
            try
            {
                IInstance inst = SimulationRuntimeManager.CreateInterface(info.Name);
                inst.OnOperatingStateChanged += OnOperatingStateChanged;
                instances.Add(inst);
            }
            catch (Exception e)
            {
                Snackbar.Add($"Issue with registered instance: {e.Message}");
            }
        }
    }

    private void OpenDialogNewPLC()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, FullWidth = true };

        DialogService.Show<NewPlcDialog>("Add PLC Instance", closeOnEscapeKey);
    }

    private void OpenDialogPLCSettings(IInstance selectedInstance)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
        var parameters = new DialogParameters();
        parameters.Add("selectedInstance", selectedInstance);

        DialogService.Show<PlcSettings>($"PLC Settings: {selectedInstance.Name}", parameters, closeOnEscapeKey);
    }

    private void OnOperatingStateChanged(IInstance inst, ERuntimeErrorCode error, DateTime dateTime,
        EOperatingState operatingState, EOperatingState operatingState2)
    {
        Snackbar.Add($"{inst.Name} changed from {operatingState} to {operatingState2}", Severity.Success,
            config => { config.HideIcon = true; });
        InvokeAsync(() => StateHasChanged());
    }

    private void OnSoftwareConfigurationChanged(ERuntimeConfigChanged e, uint p1, uint p2, int p3)
    {
        switch (e)
        {
            case ERuntimeConfigChanged.InstanceRegistered:
                var inst = SimulationRuntimeManager.CreateInterface(p3);
                instances.Add(inst);
                inst.OnOperatingStateChanged += OnOperatingStateChanged;
                Snackbar.Add($"{inst.Name} is registered", Severity.Success, config => { config.HideIcon = true; });
                break;
            case ERuntimeConfigChanged.InstanceUnregistered:
                instances.Remove(instances.SingleOrDefault(v => p3 == v.ID || v.ID == -1));
                Snackbar.Add($"{p3} is unregistered", Severity.Info, config => { config.HideIcon = true; });
                break;
            case ERuntimeConfigChanged.ConnectionOpened:
                Snackbar.Add($"Connection Opened to {p1}:{p2}", Severity.Success,
                    config => { config.HideIcon = true; });
                var ipRemoteRuntimeManager = p1;
                var portRemoteRuntimeManager = p2;
                break;
            case ERuntimeConfigChanged.ConnectionClosed:
                Snackbar.Add($"Connection closed {p1}:{p2}", Severity.Success, config => { config.HideIcon = true; });
                var ipRemoteRuntimeManager_x = p1;
                var portRemoteRuntimeManager_x = p2;
                break;
            case ERuntimeConfigChanged.PortOpened:
                Snackbar.Add($"Runtime Manager Port Opened {p1}", Severity.Success,
                    config => { config.HideIcon = true; });
                var openPort = p1;
                break;
            case ERuntimeConfigChanged.PortClosed:
                Snackbar.Add($"Runtime Manager Port Closed", Severity.Success, config => { config.HideIcon = true; });
                break;
            case ERuntimeConfigChanged.NetworkModeChanged:
                Snackbar.Add($"Network mode changed", Severity.Success, config => { config.HideIcon = true; });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e, null);
        }

        InvokeAsync(() => StateHasChanged());
    }

    public void RemoveInstance(IInstance instance)
    {
        {
            try
            {
                instances.Remove(instances.SingleOrDefault(v => instance.ID == v.ID));
                instance.UnregisterInstance();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Issue with unregistering instance: {ex.Message}");
            }
        }
    }


    private string selectedRowStyleFunc(IInstance i, int rowNumber)
    {
        if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(i))
        {
            return "background-color: lightgrey";
        }

        return string.Empty;
    }
}