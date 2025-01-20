using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

public abstract class InputPortModel(NodeModel parent)
    : Blazor.Diagrams.Core.Models.PortModel(parent, PortAlignment.Left, null, null);

public class InputPortModel<T>(NodeModel parent): BasePortModel(parent, PortAlignment.Left, null, null)
{
    public T? Value { get; set; }
    


    public override bool CanAttachTo(ILinkable other)
    {
        if (!base.CanAttachTo(other))
            return false;
        if (Links.Count>0)
            return false;

        return other is OutputPortModel<T>;
    }
}