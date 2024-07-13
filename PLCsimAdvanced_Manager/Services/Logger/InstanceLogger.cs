using MudBlazor;
using Siemens.Simatic.Simulation.Runtime;


namespace PLCsimAdvanced_Manager.Services.Logger;

public class InstanceLogger
{
    public InstanceLogger()
    {
        var instancesInfo = SimulationRuntimeManager.RegisteredInstanceInfo;
        foreach (var info in instancesInfo)
        {
            try
            {
                IInstance inst = SimulationRuntimeManager.CreateInterface(info.Name);
                inst.OnOperatingStateChanged += OnOperatingStateChanged;
                inst.OnIPAddressChanged += OnIpAddressChanged;
            }
            catch (Exception e)
            {
                // Snackbar.Add($"Issue with registered instance: {e.Message}", Severity.Error);
            }
        }
    }
    
    private void OnOperatingStateChanged(IInstance inst, ERuntimeErrorCode error, DateTime dateTime,
        EOperatingState operatingState, EOperatingState operatingState2)
    {
        Console.WriteLine($"{inst.Name} changed from {operatingState} to {operatingState2}", Severity.Info);
    }

    private void OnIpAddressChanged(IInstance inst, ERuntimeErrorCode error, DateTime dateTime, byte inInterfaceId,
        SIPSuite4 inSip)
    {
        Console.WriteLine($"{inst.Name} IP setting changed", Severity.Success);
    }
}