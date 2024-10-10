namespace PLCsimAdvanced_Manager.Services;

public class ManagerFacade: IHostedService, IDisposable
{
    private  EventDispatchService _eventDispatchService;
    public  InstanceHandler InstanceHandler;
    


    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.InstanceHandler = new InstanceHandler();
        this._eventDispatchService = new EventDispatchService(this.InstanceHandler);    
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // throw new NotImplementedException();
    }
}