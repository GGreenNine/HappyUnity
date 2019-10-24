using System;
using System.Collections;
using System.Collections.Generic;
using HappyUnity.Data;
using HappyUnity.UI;
using UnityEngine;
using UnityEngine.Events;

#if ValveVR
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;
#endif

namespace HappyUnity.VR
{
    [RequireComponent(typeof(Collider))]
    public class CapturePoint : MonoBehaviour
    {
        public LayerMask AttachLayer;
        public List<GameObject> attached;
        public UnityEvent OnAttachEvent;
        public UnityEvent OnDetachEvent;
        public AttachmentType attachmentType;

        
        public delegate void OnInteractDelegate();

        public event OnInteractDelegate On_Attached;
        public event OnInteractDelegate On_Detached;

        private void OnTriggerEnter(Collider other)
        {
            if (!UIUtils.LayerInLayerMask(AttachLayer, other.gameObject.layer))
                return;

            AttachToPoint(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!UIUtils.LayerInLayerMask(AttachLayer, other.gameObject.layer))
                return;

            DetachFromPoint(other.gameObject);
        }

#if ValveVR
        private void Valve_VR_DetachFromHand(GameObject g)
        {
            if (g.gameObject.GetComponent<Throwable>() == null) return;

            var throwable = g.gameObject.GetComponent<Throwable>();
            var s = throwable.interactable.attachedToHand;
            if (s != null)
                s.DetachObject(g.gameObject);
        }
#endif

        private void AttachToPoint(Transform g)
        {
            if (attached.Contains(g.gameObject))
                return;
#if ValveVR
            Valve_VR_DetachFromHand(g.gameObject);
#endif

            OnAttachEvent.Invoke();
            On_Attached?.Invoke();

            switch (attachmentType)
            {
                case AttachmentType.Hard:
                    g.position = transform.position;
                    g.rotation = transform.rotation;
                    break;
                case AttachmentType.Lerp:
                    break;
                case AttachmentType.HardLerp:
                    var followComponent = g.gameObject.AddComponent<FollowTarget>();
                    followComponent.SetTarget(transform);
                    followComponent.StartFollowing();
                    followComponent.Lock = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            attached.Add(g.gameObject);
        }

        private void DetachFromPoint(GameObject g)
        {
            if (!attached.Contains(g)) return;

            OnDetachEvent.Invoke();
            On_Detached?.Invoke();

            attached.Remove(g);
        }

        public enum AttachmentType
        {
            Hard,
            Lerp,
            HardLerp
        }
    }
}