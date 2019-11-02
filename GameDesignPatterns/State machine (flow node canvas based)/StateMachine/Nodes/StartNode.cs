using System;
using System.Linq;
using FlowCanvas;

namespace HappyUnity.GameDesignPatterns.StateMachine.Nodes
{
    public class StartNode : FlowNode
    {
        public FlowOutput output;

        [ShowInGraphNodeInspector("Start scenario")]
        private void StartScenario()
        {
            var node = (StateNode) output.GetPortConnections().First().targetNode;
            node?.Enter(null);
        }

        /// <summary>
        /// Отрабатывает при активации графа
        /// </summary>
        public override void OnGraphStarted()
        {
            base.OnGraphStarted();
            ScenarioManager.Instance.On_ScenarioStarts += StartScenario;
        }

        public StateNode GetState()
        {
            return null;
        }

        protected override void RegisterPorts()
        {
            output = AddFlowOutput("Output");
        }
    }
}