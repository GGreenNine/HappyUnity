using HappyUnity.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HappyUnity.GameDesignPatterns.StateMachine
{
    [RequireComponent(typeof(Collider))]
    public class OnTriggerScenarioEvents : MonoBehaviour, ScenarioEvents
    {
        public UnityEvent[] InitEvents;
        public UnityEvent[] DeactEvents;
        public LayerMask layersToTrigger;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!UIUtils.LayerInLayerMask(other.gameObject.layer, layersToTrigger))
                return;
            InitializeEvent();
        }

        public virtual void InitializeEvent()
        {
            foreach (var unityEvent in InitEvents)
            {
                unityEvent.Invoke();
            }
        }

        public virtual void DeactivateEvent()
        {
            foreach (var unityEvent in DeactEvents)
            {
                unityEvent.Invoke();
            }
        }
    }
}