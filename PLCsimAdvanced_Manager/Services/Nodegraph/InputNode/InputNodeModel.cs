using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.InputNode;

public class InputNodeModel(Point position) : BaseNodeModel(position)
{
    public override void Calculate()
    {
        throw new NotImplementedException();
    }
}

public class InputNodeModel<T>: InputNodeModel
{
    public OutputPortModel<T> OutputPort { get; set; }
    public InputNodeModel(Point position) : base(position)
    {
        OutputPort = new OutputPortModel<T>(this);
        AddPort(OutputPort);
    }

    public override void Calculate()
    {
        throw new NotImplementedException();
    }
}