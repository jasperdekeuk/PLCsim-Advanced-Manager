using MudBlazor;
using PLCsimAdvanced_Manager.Components;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Pages;

public partial class Instance
{
    private void OpenDialogSetIPSettings(IInstance selectedInstance)
    {
        DialogOptions closeOnEscapeKey = new DialogOptions()
            { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, CloseButton = true };
        var parameters = new DialogParameters();
        parameters.Add("selectedInstance", selectedInstance);
        DialogService.Show<SetIPSettingsDialog>($"IP Settings: {selectedInstance.Name}", parameters, closeOnEscapeKey);
    }
} 