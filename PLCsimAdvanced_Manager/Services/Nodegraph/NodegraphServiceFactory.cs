using System.Collections.Concurrent;
using Blazor.Diagrams.Core;

namespace PLCsimAdvanced_Manager.Services.Nodegraph;

public class NodegraphServiceFactory
{
    private readonly ConcurrentDictionary<string, NodegraphService> _services = new ConcurrentDictionary<string, NodegraphService>();

    public NodegraphService GetOrCreateService(string plcInstanceName)
    {
        return _services.GetOrAdd(plcInstanceName, id => new NodegraphService(id));
    }
    
    public void RemoveService(string plcInstanceName)
    {
        _services.TryRemove(plcInstanceName, out _);
    }
}