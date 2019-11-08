using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ValveVR
using Valve.VR.InteractionSystem;
#endif

namespace Stereometry.Scripts.MiniBehaviours
{
    public class Hand
    {
        /*
         * Replace this on Valve Hand script
         */
        public Interactable hoveringInteractable;
    }

    public class Interactable : MonoBehaviour
    {
        /*
         * Replace this on Valve Interactable script
         */
    }
    public class Selector : MonoBehaviour
    {
        public Material selectedMaterial;

        public static readonly List<KeyValuePair<ISelectable<InteractiveObject>, Material>> SelectedObjects =
            new List<KeyValuePair<ISelectable<InteractiveObject>, Material>>();

        private Hand hand;
        public static InteractiveObject selectedObject = null;

        public Interactable hoveringObject => hand.hoveringInteractable;

        public delegate void ObjectSelectedDelegate(InteractiveObject obj);

        public event ObjectSelectedDelegate On_ObjectSelected;

        private void Awake()
        {
            hand = GetComponent<Hand>();
        }

        private void Start()
        {
            InputManager.OnObjectSelectedOn += OnSelected;
        }

        private void OnDestroy()
        {
            InputManager.OnObjectSelectedOn -= OnSelected;
        }

        /// <summary>
        /// Unselecting all selected items
        /// </summary>
        public static void UnselectAll()
        {
            foreach (var selectedObject in SelectedObjects)
            {
                selectedObject.Key.RenderingMaterial = selectedObject.Value;
                selectedObject.Key.IsSelected = false;
            }

            selectedObject = null;
            SelectedObjects.Clear();
        }

        /// <summary>
        /// Unselecting item
        /// </summary>
        /// <param name="selectable_Object"></param>
        public static void Unselect(InteractiveObject selectable_Object)
        {
            selectable_Object.isSelected = false;
            
            if (selectedObject == selectable_Object)
                selectedObject = null;
            
            if (!SelectedObjects.Any()) return;
            
            var objectToDelete = SelectedObjects.First(x => x.Key == selectable_Object);
            
            if (objectToDelete.Key != null)
                SelectedObjects.Remove(objectToDelete);
        }

        /// <summary>
        /// Selecting item
        /// </summary>
        /// <param name="selectable_Object"></param>
        /// <typeparam name="T"></typeparam>
        private void Select<T>(ISelectable<T> selectable_Object) where T : InteractiveObject
        {
            selectedObject = selectable_Object.GetInteractable();
            
            if (selectedObject == null)
                return;
            if (selectable_Object.IsSelected)
                return;
            
            SelectedObjects.Add(new KeyValuePair<ISelectable<InteractiveObject>, Material>(selectable_Object,
                selectable_Object.RenderingMaterial));
            
            selectable_Object.RenderingMaterial = selectedMaterial;
            selectable_Object.IsSelected = true;
        }

        /// <summary>
        /// Called by event
        /// </summary>
        private void OnSelected()
        {
            if (hoveringObject == null)
            {
                UnselectAll();
                return;
            }

            if (!(hoveringObject.GetComponent<InteractiveObject>() is ISelectable<InteractiveObject>
                ISelectable))
                return;

            Select(ISelectable);

            On_ObjectSelected?.Invoke(selectedObject);
        }
    }
}