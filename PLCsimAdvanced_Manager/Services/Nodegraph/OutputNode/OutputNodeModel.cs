using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using PLCsimAdvanced_Manager.Services.Nodegraph.BasicLib.BoolInverse;
using PLCsimAdvanced_Manager.Services.Nodegraph.InputNode;
using PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.OutputNode;

public abstract class OutputNodeModel(Point position) : BaseNodeModel(position)
{
}
public class OutputNodeModel<T> : OutputNodeModel
{
    public InputPortModel<T> InputPort { get; set; }
    public T? Value { get; set; }
    public OutputNodeModel(Point position) : base(position)
    {
        InputPort = new InputPortModel<T>(this);
        AddPort(InputPort);
    }

    public override void Calculate()
    {
        var source = PortLinks.First().Source as SinglePortAnchor;
        if (source == null)
            return;
        Value = (source.Port as OutputPortModel<T>).Value;
        
    }
}