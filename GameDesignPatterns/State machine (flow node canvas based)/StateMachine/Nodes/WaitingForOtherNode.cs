//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using DefaultNamespace;
//using ModelsLibrary;
//using UnityEngine;
//
//public class WaitingForOtherNode : StateNode
//{
//    public override void Enter(StateNode node)
//    {
//        base.Enter(node);
//        
//        /*
//        * Waiting for other players get connected
//        */
//        if (userRoles.Count() <= UsersOnThisNode_Dictionary.Count)
//        {
//            MoveNext();
//            if (GameManager.Instance.GameStateProperty == GameManager.GameState.Waiting)
//                GameManager.Instance.GameStateProperty = GameManager.GameState.Playing;
//        }
//    }
//
//    public override void OnEnterOther(NodeInformationModel model)
//    {
//        base.OnEnterOther(model);
//
//        /*
//        * Waiting for other players get connected
//        */
//        if (userRoles.Count() <= UsersOnThisNode_Dictionary.Count)
//        {
//            MoveNext();
//            if (GameManager.Instance.GameStateProperty == GameManager.GameState.Waiting)
//                GameManager.Instance.GameStateProperty = GameManager.GameState.Playing;
//        }
//    }
//}