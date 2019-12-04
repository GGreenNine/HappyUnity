using System.Linq;
using HappyUnity.Singletons;
using Stereometry.Scripts.MiniBehaviours;
using UnityEngine;
#if valve_VR
using Valve.VR;
using Valve.VR.InteractionSystem;
#endif


namespace Stereometry.Scripts
{
    public class InputManager : Singleton<InputManager>
    {
        public Hand LeftHand;
        public Hand RightHand;

#if valve_VR
        public SteamVR_Action_Boolean objectSelectAction =
 SteamVR_Input.GetAction<SteamVR_Action_Boolean>("SelectObject");
        public SteamVR_Action_Boolean rightHandMenuPush =
 SteamVR_Input.GetAction<SteamVR_Action_Boolean>("RightHandMenu");

        /*
     * Нажатие на верхнуюю кнопку меню правого контроллера
     */
        public delegate void ObjectSelectInputDelegate();

        public static event ObjectSelectInputDelegate OnObjectSelectedOn;
        public static event ObjectSelectInputDelegate OnObjectSelectedOff;

        /*
     * Нажание на край пада правого контроллера
     */
        public delegate void PadMenuCornerInputDelegate();

        public static event PadMenuCornerInputDelegate OnPadMenuCornerOn;
        public static event PadMenuCornerInputDelegate OnPadMenuCornerOff;

        /*
     * Нажатие на курок контроллера
     */
        public delegate void GrapPinchDelegate();

        public static event GrapPinchDelegate OnLeftHandPinchOn;
        public static event GrapPinchDelegate OnLeftHandPinchOff;

        public static event GrapPinchDelegate OnRightHandPinchOn;
        public static event GrapPinchDelegate OnRightHandPinchOff;

        protected void Start()
        {
            if (LeftHand == null || RightHand == null)
            {
                var hands = FindObjectsOfType<Hand>();
            
                LeftHand = hands.First(x => x.handType == SteamVR_Input_Sources.LeftHand);
                RightHand = hands.First(x => x.handType == SteamVR_Input_Sources.RightHand);
            }
        
        
            LeftHand.grabPinchAction.AddOnChangeListener(OnGrapLefthand, LeftHand.handType);
            RightHand.grabPinchAction.AddOnChangeListener(OnGrapRighthand, RightHand.handType);
            
            objectSelectAction.AddOnChangeListener(OnObjectSelected, RightHand.handType);
            rightHandMenuPush.AddOnChangeListener(OnRightHandMenu, RightHand.handType);
        }

        private void OnRightHandMenu(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource, bool newstate)
        {
            if (newstate)
                OnPadMenuCornerOn?.Invoke();
            else
                OnPadMenuCornerOff?.Invoke();
        }

        private void OnObjectSelected(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource, bool newstate)
        {
            if (newstate)
                OnObjectSelectedOn?.Invoke();
            else
                OnObjectSelectedOff?.Invoke();
        }

        private void OnGrapRighthand(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource, bool newstate)
        {
            Debug.Log("selected button");
            if (newstate)
                OnRightHandPinchOn?.Invoke();
            else
                OnRightHandPinchOff?.Invoke();
        }

        private void OnGrapLefthand(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource, bool newstate)
        {
                if (newstate)
                    OnLeftHandPinchOn?.Invoke();
                else
                    OnLeftHandPinchOff?.Invoke();
        }

        private void OnDestroy()
        {
            LeftHand.grabPinchAction.RemoveOnChangeListener(OnGrapLefthand, LeftHand.handType);
            RightHand.grabPinchAction.RemoveOnChangeListener(OnGrapRighthand, RightHand.handType);
            
            objectSelectAction.RemoveOnChangeListener(OnObjectSelected, RightHand.handType);
            rightHandMenuPush.RemoveOnChangeListener(OnRightHandMenu, RightHand.handType);
        }
#endif
    }
}