////using System.Collections;
////using System.Collections.Generic;
////using System.Linq;
////using UnityEditor;
////using UnityEngine;
////using XNode.Examples.StateGraph;
////
////namespace XNodeEditor.Examples {
////	[CustomNodeEditor(typeof(StateNode))]
//
//using XNodeEditor;
//
//public class StateNodeEditor : NodeEditor {
////
////		public override void OnHeaderGUI() {
////			GUI.color = Color.white;
////			StateNode node = target as StateNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (graph.current == node) GUI.color = Color.blue;
////			if (graph.other == node) GUI.color = Color.red;
////			if (graph.other == node && graph.current == node) GUI.color = Color.magenta;
////			if(graph.findResultList.Contains(node))GUI.color = Color.yellow;
////			string title = target.name;
////			GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
////			GUI.color = Color.white;
////		}
////		
////
////		public override void OnBodyGUI() {
////			base.OnBodyGUI();
////			StateNode node = target as StateNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (GUILayout.Button("MoveNext Node")) node.MoveNext();
////			if (GUILayout.Button("Continue Graph")) graph.Continue();
////			if (GUILayout.Button("Set as current")) graph.current = node;
////		}
////	}
////	
////	[CustomNodeEditor(typeof(FindNode))]
////	public class FindNodeEditor : NodeEditor {
////
////		public override void OnHeaderGUI() {
////			GUI.color = Color.white;
////			FindNode node = target as FindNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (graph.current == node) GUI.color = Color.blue;
////			string title = target.name;
////			GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
////			GUI.color = Color.white;
////		}
////		
////
////		public override void OnBodyGUI() {
////			base.OnBodyGUI();
////			FindNode node = target as FindNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (GUILayout.Button("Find node Node")) node.FindObject();
////			if (GUILayout.Button("Clear search")) node.ClearSearch();
////		}
////	}
////	[CustomNodeEditor(typeof(SettingNode))]
////	public class SettingsNodeEditor : NodeEditor {
////
////		public override void OnHeaderGUI() {
////			GUI.color = Color.white;
////			SettingNode node = target as SettingNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (graph.current == node) GUI.color = Color.blue;
////			string title = target.name;
////			GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
////			GUI.color = Color.cyan;
////		}
////		
////
////		public override void OnBodyGUI() {
////			base.OnBodyGUI();
////			SettingNode node = target as SettingNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (GUILayout.Button("Set Ids To nodes")) node.SetIds();
////		}
////	}
////	[CustomNodeEditor(typeof(MediatorNode))]
////	public class MediatorNodeEditor : NodeEditor {
////
////		public override void OnHeaderGUI() {
////			GUI.color = new Color(100,166,0,1);
////			MediatorNode node = target as MediatorNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (graph.current == node) GUI.color = Color.blue;
////			string title = target.name;
////			GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
////		}
////		
////
////		public override void OnBodyGUI() {
////			base.OnBodyGUI();
////			MediatorNode node = target as MediatorNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (GUILayout.Button("MoveNext Node")) node.MoveNext();
////			if (GUILayout.Button("Continue Graph")) graph.Continue();
////			if (GUILayout.Button("Set as current")) graph.current = node;
////		}
////	}
//
////using UnityEngine;
////using XNode.Examples.StateGraph;
////using XNodeEditor;
////
////[CustomNodeEditor(typeof(TestNode))]
////	public class StartNodeEditor : NodeEditor {
////
////		public override void OnHeaderGUI() {
////			GUI.color = Color.white;
////			TestNode node = target as TestNode;
////			StateGraph graph = node.graph as StateGraph;
////			string title = target.name;
////			GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
////			GUI.color = Color.white;
////		}
////
////		public override void OnBodyGUI() {
////			base.OnBodyGUI();
////			TestNode node = target as TestNode;
////			StateGraph graph = node.graph as StateGraph;
////			if (GUILayout.Button("MoveNext Node")) node.MoveNext();
////			if (GUILayout.Button("Continue Graph")) graph.Continue();
////		}
////	}
////	
////}