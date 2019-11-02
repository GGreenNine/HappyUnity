using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlowCanvas;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using HappyUnity.GameDesignPatterns.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using Object = UnityEngine.Object;

namespace HappyUnity.GameDesignPatterns.StateMachine
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ShowInGraphNodeInspector : Attribute
    {
        public string name;

        public ShowInGraphNodeInspector(string name)
        {
            this.name = name;
        }
    }
    
    /// <summary>
    /// State node class
    /// todo Maybe make it abstract?
    /// </summary>
    public class StateNode : FlowControlNode, IEqualityComparer
    {
        [HideInInspector] // not redundand
        public FlowInput input;

        [HideInInspector] // not redundand
        public FlowOutput output;

        public GameObject stateDelegate;

        public ConcurrentDictionary<string, UserProfile> UsersOnThisNode_Dictionary =
            new ConcurrentDictionary<string, UserProfile>();
        //public List<UserProfile> UsersOnxThisNode;

        /// <summary>
        /// Name of the start events on object delegate
        /// * Важно
        /// Имена должны совпадать
        /// Имена должны начинаться с верхнего и продолжаться с нижнего
        /// </summary>
        public UnityEvent[] Startevents;

        public UnityEvent[] Disableevents;

        protected bool isInitialized = false;

        public void AddUser(UserProfile user)
        {
            if (UsersOnThisNode_Dictionary == null)
                UsersOnThisNode_Dictionary = new ConcurrentDictionary<string, UserProfile>();
            UsersOnThisNode_Dictionary.TryAdd(user.userName, user);
        }

        public void RemoveUser(UserProfile user)
        {
            if (UsersOnThisNode_Dictionary == null)
                UsersOnThisNode_Dictionary = new ConcurrentDictionary<string, UserProfile>();
            UsersOnThisNode_Dictionary.TryRemove(user.userName, out var value);
        }

        public override void OnGraphStarted()
        {
            if (isInitialized)
                return;

            base.OnGraphStarted();

            Startevents = stateDelegate.GetComponent<StateNodeDelegate>().Startevents;
            Disableevents = stateDelegate.GetComponent<StateNodeDelegate>().Disableevents;

            isInitialized = true;
        }

        /// <summary>
        /// Will be called on creating the node
        /// </summary>
        /// <param name="assignedGraph"></param>
        public override void OnCreate(Graph assignedGraph)
        {
            base.OnCreate(assignedGraph);
            CreateStateDelegate();
        }

        /// <summary>
        /// Creating delegate state object in scene
        /// </summary>
        private void CreateStateDelegate()
        {
            stateDelegate = new GameObject();
            stateDelegate.transform.parent = GameObject.FindObjectOfType<FlowScriptController>().transform;
            stateDelegate.AddComponent<StateNodeDelegate>();
            var lastNodeId = graph.allNodes.Count;
            stateDelegate.name = $"State {lastNodeId} Delegate";
        }

        /// <summary>
        /// Will be called on destroying the node
        /// </summary>
        public override void OnDestroy()
        {
            Object.DestroyImmediate(stateDelegate);
            base.OnDestroy();
        }

        /// <summary>
        /// Set node as current
        /// </summary>
        [ShowInGraphNodeInspector("Set as current")]
        public void SetAsCurrent()
        {
            if (ScenarioManager.current != null)
                ScenarioManager.current.SetDefaults();

            ScenarioManager.current = this;
            ExecuteEvents(Startevents);
        }

        /// <summary>
        /// Drawing ports
        /// </summary>
        protected override void RegisterPorts()
        {
            input = AddFlowInput("Input", delegate(Flow flow) { });
            output = AddFlowOutput("Output");
        }

        /// <summary>
        /// Move to the next node
        /// </summary>
        [ShowInGraphNodeInspector("Move next")]
        public virtual void MoveNext()
        {
            if (!output.GetPortConnections().Any())
                return;

            var node = GetNextNode(output.GetPortConnections());
            if (node == null) return;

            ExecuteEvents(Disableevents);

            node.Enter(this);
        }


        /// <summary>
        /// Determines the next passage
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        protected virtual StateNode GetNextNode(BinderConnection[] connections)
        {
            var firstConnectedNode = connections.First();
            var node = (StateNode) firstConnectedNode?.targetNode;
            return node;
        }

        /// <summary>
        /// Called when someone enteren on the node
        /// </summary>
        /// <param name="node"></param>
        public virtual void Enter(StateNode node)
        {
            SetAsCurrent();
        }

        /// <summary>
        ///  Executing custom events
        /// </summary>
        private void ExecuteEvents(UnityEvent[] eventList)
        {
            if (eventList == null) return;
            if (!eventList.Any()) return;
            foreach (var @event in eventList)
            {
                @event.Invoke();
            }
        }

        [ShowInGraphNodeInspector("Set defaults")]
        private void SetDefaults()
        {
            if (ScenarioManager.current == this)
            {
                ScenarioManager.current = null;
                DeactivateEvenwts();
                ExecuteEvents(Disableevents);
            }
        }

        /// <summary>
        /// Отправляет информацию о ноде
        /// </summary>
        protected void SendNodeInformation()
        {
            /*
             * Отправка данных о прохождении по вебсокетам
             */
        }


        /// <summary>
        /// Called when other client entered the node 
        /// </summary>
        /// <param name="user"></param>
        [ShowInGraphNodeInspector("On enter other")]
        public virtual void OnEnterOther(NodeInformationModel model)
        {
            ScenarioManager.other = this;
            AddUser(model.profile);


            MainThreadDispatcher.Instance.Enqueue(delegate { InitializeEvents(); });
        }

        /// <summary>
        /// Initializing object that connected with current node
        /// </summary>
        protected void InitializeEvents()
        {
        }

        /// <summary>
        /// Deactivating object that connected with current node
        /// </summary>
        protected void DeactivateEvenwts()
        {
        }

        public bool Equals(object x, object y)
        {
            return ((StateNode) x).position == ((StateNode) y).position;
        }

        public int GetHashCode(object obj)
        {
            return ((StateNode) obj).position.GetHashCode();
        }
    }

    public class NodeInformationModel
    {
        public int nodeId;
        public UserProfile profile;

        public NodeInformationModel(int nodeId, UserProfile profile)
        {
            this.nodeId = nodeId;
            this.profile = profile;
        }
    }
}