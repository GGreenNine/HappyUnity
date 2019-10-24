using FlowCanvas;
using HappyUnity.Singletons;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

namespace HappyUnity.GameDesignPatterns.StateMachine
{
    [RequireComponent(typeof(FlowScriptController), typeof(NodeCanvas.Framework.Blackboard))]
    public class ScenarioManager : Singleton<ScenarioManager>
    {
        [NotNull] public FlowScriptController graph;

        public static StateNode current;
        public static StateNode other;

        [Tooltip("On - Scenario starts at scene Start, Off - Scenario start manualy")]
        public bool autoStartScenario;

        private bool scenarioIsRunning;

        public delegate void ScenarioStateDelegate();

        public event ScenarioStateDelegate On_ScenarioStarts;
        public event ScenarioStateDelegate On_ScenarioEnds;


        protected override void Awake()
        {
            base.Awake();
            graph = GetComponent<FlowScriptController>();
        }

        public void Reset()
        {
            graph.RestartBehaviour();
        }

        private void Start()
        {
            if (autoStartScenario)
                StartScenario();
        }

        public void EndScenario()
        {
            On_ScenarioEnds?.Invoke();
        }

        public void StartScenario()
        {
            if (scenarioIsRunning)
                return;
        
            On_ScenarioStarts?.Invoke();
            scenarioIsRunning = true;
        }


        /*
     * Настройка цвета нод в скрипте:
     * Editor.Node line = 378-397
     */
        public static Color currentNodeColor = Color.green;
        public static Color otherNodeColor = Color.red;
        public static Color defaultNodeColor = Color.white;

        //  public delegate void ContinueDelegate();

        // public event ContinueDelegate OnContinue;
        public UnityEvent OnContinue;

        /// <summary>
        /// Продолжить прохождение по графу
        /// </summary>
        public void Continue()
        {
            current.MoveNext();
            print("OnContinue");
            OnContinue?.Invoke();
        }

        public void RemovePlayerFromGraph(UserProfile profile)
        {
            if (!graph.graph.isRunning)
            {
                Debug.LogError("You are trying to set player to the node while graph is not running?");
                return;
            }

            var stateNodes = graph.graph.GetAllNodesOfType<StateNode>();
            foreach (var node in stateNodes)
            {
                node.RemoveUser(profile);
            }
        }
    
    }
}