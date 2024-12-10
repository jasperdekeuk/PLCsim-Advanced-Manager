using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.InputNode;

public abstract class InputNodeModel(Point position) : BaseNodeModel(position)
{
}
public class InputNodeModel<T> : InputNodeModel
{
    public T? Value { get; set; }

    public InputNodeModel(Point position) : base(position)
    {
        AddPort(new OutputPortModel<T>(this));
    }

    public override void Calculate()
    {
        throw new NotImplementedException();
    }
}