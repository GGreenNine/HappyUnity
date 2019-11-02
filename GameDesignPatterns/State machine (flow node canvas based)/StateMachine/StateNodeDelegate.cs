using System;
using UnityEngine;
using UnityEngine.Events;

    
    public class StateNodeDelegate : MonoBehaviour
    {
        //[InspectorName("Add those from dirived gameobject")]
        [SerializeField] public UnityEvent[] Startevents;
        [SerializeField] public UnityEvent[] Disableevents;
    }
