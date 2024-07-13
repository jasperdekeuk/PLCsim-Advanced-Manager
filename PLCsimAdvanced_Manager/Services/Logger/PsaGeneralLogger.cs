using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Services.Logger;

public class PsaGeneralLogger
{
    public PsaGeneralLogger()
    {
        SimulationRuntimeManager.OnRunTimemanagerLost += OnRunTimemanagerLost;
        SimulationRuntimeManager.OnConfigurationChanged += OnConfigurationChanged;
        SimulationRuntimeManager.OnAutodiscoverData += OnAutodiscoverData;
    }
    
    private void OnRunTimemanagerLost()
    {
        throw new NotImplementedException();
    }
    private void OnConfigurationChanged(ERuntimeConfigChanged in_runtimeconfigchanged, uint in_param1, uint in_param2, int in_param3)
    {
        throw new NotImplementedException();
    }
    
    private void OnAutodiscoverData(EAutodiscoverType in_autodiscovertype, SAutodiscoverData in_autodiscoverdata)
    {
        throw new NotImplementedException();
    }

}