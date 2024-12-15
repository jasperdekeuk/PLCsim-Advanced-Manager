using MudBlazor;
using PLCsimAdvanced_Manager.Components;
using PLCsimAdvanced_Manager.Components.Instance;
using PLCsimAdvanced_Manager.Services;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Pages;

public partial class PlcOverview
{
    // private List<IInstance>? instances;
    // private IInstance? _selectedInstance;
    private MudTable<IInstance> mudTable;
    private bool IsEditingCommunicationInterface = false;

    protected override void OnInitialized()
    {
        managerFacade.InstanceHandler.UpdateExistingInstances();
        managerFacade.InstanceHandler.OnInstanceChanged += OnInstanceChanged;
        managerFacade.InstanceHandler.OnIssue += OnIssue;
        base.OnInitialized();
    }

    private void OnInstanceChanged(object? sender, InstanceChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
        Snackbar.Add(e.Message, Severity.Success);
    }

    private void OnIssue(object? sender, Exception e)
    {
     Snackbar.Add(e.Message, Severity.Error);   
    }


    private void OpenDialogNewPLC()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, FullWidth = true };

        DialogService.Show<NewPlcDialog>("Add PLC Instance", closeOnEscapeKey);
    }

    private void OpenDialogStorage()
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, FullWidth = true, CloseButton = true };

        DialogService.Show<StorageDialog>("Storage", closeOnEscapeKey);
    }

    private void OpenDialogSetIPSettings(IInstance selectedInstance)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, CloseButton = true };
        var parameters = new DialogParameters();
        parameters.Add("selectedInstance", selectedInstance);
        DialogService.Show<SetIPSettingsDialog>($"IP Settings: {selectedInstance.Name}", parameters, closeOnEscapeKey);
    }

    private void OpenDialogPLCSettings(IInstance selectedInstance)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
        var parameters = new DialogParameters();
        parameters.Add("selectedInstance", selectedInstance);

        DialogService.Show<PlcSettings>($"PLC Settings: {selectedInstance.Name}", parameters, closeOnEscapeKey);
    }

    private void OpenDialogNetInterfaceMapping(IInstance selectedInstance)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
        var parameters = new DialogParameters();
        parameters.Add("selectedInstance", selectedInstance);

        DialogService.Show<NetInterfaceMappingSettings>($"Net Interface Mapping: {selectedInstance.Name}", parameters,
            closeOnEscapeKey);
    }

    private void OpenDialogSnapshots(IInstance selectedInstance)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, FullWidth = true, CloseButton = true };
        var parameters = new DialogParameters();
        parameters.Add("selectedInstance", selectedInstance);
        DialogService.Show<SnapshotsDialog>("Snapshots", parameters, closeOnEscapeKey);
    }

    public void RemoveInstance(IInstance instance)
    {
        var parameters = new DialogParameters<DeleteDialog>();
        parameters.Add(x => x.Instance, instance);
        parameters.Add(x => x.Instances, managerFacade.InstanceHandler._instances);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        DialogService.Show<DeleteDialog>("Delete Instance", parameters, options);
    }
    

    public void SeeSnapshots(IInstance instane)
    {
        OpenDialogSnapshots(instane);
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