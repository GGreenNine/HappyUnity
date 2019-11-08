using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HappyUnity.UI;
using Stereometry.Scripts.MiniBehaviours;

#if ValveVR
using Valve.VR.InteractionSystem;
#endif


public class InteractiveObject : MonoBehaviour
{
    public LayerMask InteractionLayers;
    protected Interactable interactable;
    protected FollowTarget followTarget;
    [HideInInspector] public bool isSelected;

    protected virtual void Awake()
    {
        interactable = GetComponent<Interactable>();
        if (interactable)
        {
#if ValveVR
            interactable.onAttachedToHand += InteractableOnAttachedToHand;
            interactable.onDetachedFromHand += InteractableOnDetachedFromHand;
#endif
        }

        followTarget = gameObject.GetComponent<FollowTarget>();
    }

    protected virtual void InteractableOnDetachedFromHand(Hand hand)
    {
    }

    protected virtual void InteractableOnAttachedToHand(Hand hand)
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Colliding(other.gameObject, true);
    }

    private void OnDestroy()
    {
#if ValveVR
        interactable.onAttachedToHand -= InteractableOnAttachedToHand;
        interactable.onDetachedFromHand -= InteractableOnDetachedFromHand;
#endif
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        Colliding(other.gameObject, false);
    }


    protected virtual void EntryEvent()
    {
    }

    protected virtual void ExitEvent()
    {
    }

    private void Colliding(GameObject collider, bool enter)
    {
        if (!UIUtils.LayerInLayerMask(collider.layer, InteractionLayers))
            return;

        if (enter)
            EntryEvent();
        else
            ExitEvent();
    }
}

public interface ISelectable<out T> where T : InteractiveObject
{
    Material RenderingMaterial { get; set; }
    bool IsSelected { get; set; }
    T GetInteractable();
}