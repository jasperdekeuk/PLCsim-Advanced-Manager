using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace PLCsimAdvanced_Manager.Services.Nodegraph;

public abstract class BaseNodeModel(Point position) : NodeModel(position)
{
    public abstract void Calculate();
}

public class BaseNodeModel<T>(Point position) : BaseNodeModel(position)
{
    public T? Value { get; set; }

    public override void Calculate()
    {
        ///    
    }
}