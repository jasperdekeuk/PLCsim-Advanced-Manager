using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using PLCsimAdvanced_Manager.Services.Nodegraph.InputNode;
using PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.BasicLib.BoolInverse;

public class BoolInverseNodeModel : BaseNodeModel<bool>
{
    private OutputPortModel<bool> outputPort;
    public BoolInverseNodeModel(Point position) : base(position)
    {
        AddPort(new InputPortModel<bool>(this));
        outputPort = new OutputPortModel<bool>(this);
        AddPort(outputPort);
    }

    public override void Calculate()
    {
        var source = PortLinks.First().Source as SinglePortAnchor;
        if (source == null)
            return;
        var val = (source.Port as OutputPortModel<bool>).Value;
        outputPort.Value = !val;
    }
}

