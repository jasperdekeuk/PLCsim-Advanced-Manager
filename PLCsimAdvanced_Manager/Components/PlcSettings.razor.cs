using Microsoft.AspNetCore.Components;
using MudBlazor;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Components;

public partial class PlcSettings
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter] public IInstance selectedInstance { get; set; }
    
    public string ip;
    public string subnet;
    public string gateway;
    public IMask ipv4Mask = RegexMask.IPv4();
    private SIPSuite4 in_IPSuite = new SIPSuite4();
    

    public void setIp()
    {
        selectedInstance.SetIPSuite((uint)selectedInstance.Info.ID, in_IPSuite, true);
    }
    
}
