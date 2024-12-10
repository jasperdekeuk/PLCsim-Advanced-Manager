using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

public abstract class OutputPortModel(NodeModel parent)
    : Blazor.Diagrams.Core.Models.PortModel(parent, PortAlignment.Right, null, null);
public class OutputPortModel<T>(NodeModel paretn): Blazor.Diagrams.Core.Models.PortModel(paretn, PortAlignment.Right, null, null)
{
    public T? Value { get; set; }

    public override bool CanAttachTo(ILinkable other)
    {
        if (!base.CanAttachTo(other))
            return false;
        
        if (other.Links.Count>0)
            return false;

        return other is InputPortModel<T>;
    }
    
}