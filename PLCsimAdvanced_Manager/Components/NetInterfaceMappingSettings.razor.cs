using Microsoft.AspNetCore.Components;
using MudBlazor;
using Siemens.Simatic.Simulation.Runtime;


namespace PLCsimAdvanced_Manager.Components;

public partial class NetInterfaceMappingSettings
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter] public IInstance selectedInstance { get; set; }

    string emptyString = "";
    
    private string _interface_1;
    public string interface_1
    {
        get => SimulationRuntimeManager.NetInterfaces.FirstOrDefault(i => i.interfaceIndex == selectedInstance.GetNetInterfaceMapping(EPLCInterface.IE1)).interfaceName;
        set
        {
            try
            {
                if (value == emptyString)
                {
                    selectedInstance.SetNetInterfaceMapping(EPLCInterface.IE1, 0);
                }
                else
                {
                    selectedInstance.SetNetInterfaceMapping(EPLCInterface.IE1, value);
                }
                _interface_1 = SimulationRuntimeManager.NetInterfaces.FirstOrDefault(i => i.interfaceIndex == selectedInstance.GetNetInterfaceMapping(EPLCInterface.IE1)).interfaceName;
            }
            catch (Exception e)
            {
                Snackbar.Add($"Error setting interface mapping: {e.Message}");
            }
        }
    }

    private string _interface_2;
    public string interface_2
    {
        get => SimulationRuntimeManager.NetInterfaces.FirstOrDefault(i => i.interfaceIndex == selectedInstance.GetNetInterfaceMapping(EPLCInterface.IE2)).interfaceName;
        set
        {
            try
            {
                if (value == emptyString)
                {
                    selectedInstance.SetNetInterfaceMapping(EPLCInterface.IE2, 0);
                }
                else
                {
                    selectedInstance.SetNetInterfaceMapping(EPLCInterface.IE2, value);
                }

                _interface_2 = SimulationRuntimeManager.NetInterfaces.FirstOrDefault(i => i.interfaceIndex == selectedInstance.GetNetInterfaceMapping(EPLCInterface.IE2)).interfaceName;
            }
            catch (Exception e)
            {
                Snackbar.Add($"Error setting interface mapping: {e.Message}");
            }
        }
    }

    private string _interface_3;
    public string interface_3
    {
        get => SimulationRuntimeManager.NetInterfaces.FirstOrDefault(i => i.interfaceIndex == selectedInstance.GetNetInterfaceMapping(EPLCInterface.IE3)).interfaceName;
        set
        {
            try
            {
                if (value == emptyString)
                {
                    selectedInstance.SetNetInterfaceMapping(EPLCInterface.IE3, 0);
                }
                else
                {
                    selectedInstance.SetNetInterfaceMapping(EPLCInterface.IE3, value);
                }
                _interface_3 = SimulationRuntimeManager.NetInterfaces.FirstOrDefault(i => i.interfaceIndex == selectedInstance.GetNetInterfaceMapping(EPLCInterface.IE3)).interfaceName;
            }
            catch (Exception e)
            {
                Snackbar.Add($"Error setting interface mapping: {e.Message}");
            }
        }
    }
    
    public void setInterface(EPLCInterface interfaceID, uint value)
    {
        try{selectedInstance.SetNetInterfaceMapping(interfaceID, value);}
        catch (Exception e)
        {
            Snackbar.Add($"Error setting interface mapping: {e.Message}");
        }
    }    
    public void setInterface(EPLCInterface interfaceID, string value)
    {
        try{selectedInstance.SetNetInterfaceMapping(interfaceID, value);}
        catch (Exception e)
        {
            Snackbar.Add($"Error setting interface mapping: {e.Message}");
        }
    }
    
}
