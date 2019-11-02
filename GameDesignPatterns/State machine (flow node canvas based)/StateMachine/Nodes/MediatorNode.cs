using System.Collections.Generic;
using FlowCanvas;
using HappyUnity.GameDesignPatterns.StateMachine;

namespace StateMachine.Nodes
{
    public class MediatorNode : StateNode
    {
        public List<StateNode> waitingNodes = new List<StateNode>();

        public override void Enter(StateNode node)
        {
            if (!waitingNodes.Contains(node))
            {
                waitingNodes.Add(node);
            }

            SendNodeInformation();

            if (waitingNodes.Count < this.inConnections.Count) return;
            ScenarioManager.current = this;

            waitingNodes.Clear();
            MoveNext();
        }

        public override void OnEnterOther(NodeInformationModel model)
        {
            MainThreadDispatcher.Instance.Enqueue(delegate
            {
                base.OnEnterOther(model);
                waitingNodes.Add(new StateNode());
                
                if (waitingNodes.Count < this.inConnections.Count) return;
                ScenarioManager.current = this;

                waitingNodes.Clear();
                MoveNext();
            });
            

        }

        protected override void RegisterPorts()
        {
            input = AddFlowInput("Input States", delegate(Flow flow) { });
            output = AddFlowOutput("Output States");
        }
    }
}