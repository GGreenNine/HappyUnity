using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Category("Flow Controllers")]
    [Color("bf7ffb")]
    [ContextDefinedInputs(typeof(Flow))]
    [ContextDefinedOutputs(typeof(Flow))]
    abstract public class FlowControlNode : FlowNode { }
}