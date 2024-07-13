namespace PLCsimAdvanced_Manager.Services;

public class ManagerFacade
{
    private readonly EventDispatchService _eventDispatchService;
    public readonly InstanceHandler InstanceHandler;
    
    public ManagerFacade()
    {
        this.InstanceHandler = new InstanceHandler();
        this._eventDispatchService = new EventDispatchService(this.InstanceHandler);
    }
}