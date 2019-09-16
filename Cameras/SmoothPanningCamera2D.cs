using HappyUnity.TransformUtils;
using HappyUnity.UI;
using UnityEngine;

namespace HappyUnity.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class SmoothPanningCamera2D : MonoBehaviour
    {
        public Vector2 UpperBorder;
        public Vector2 LowerBorder;

        public float MouseSensitivity = 1f;

        public bool FixedX;
        public bool FixedY;

        private Vector3 lastMousePosition;
        private new Camera camera;
        private SmoothMover smoothMover;

        private Vector3 oldPosition;

        [SerializeField] private bool mouseInputEnabled = true;

        public bool MouseInputEnabled
        {
            get
            {
                return mouseInputEnabled;
            }
            set
            {
                mouseInputEnabled = value;
                if (mouseInputEnabled)
                {
                    lastMousePosition = Input.mousePosition;
                    oldPosition = transform.position;
                }
            }
        }


        private void Awake()
        {
            camera = GetComponent<Camera>();
            smoothMover = GetComponent<SmoothMover>();
        }

        private void OnEnable()
        {
            oldPosition = transform.position;
            lastMousePosition = Input.mousePosition;
            //smoothMover.TargetPosition = oldPosition; todo
            smoothMover.BeginMoving(false);
        }

        public void MoveBy(Vector3 offset)
        {
            MoveTo(oldPosition + offset);
        }


        public void MoveTo(Vector3 newPosition)
        {
            Vector3 screenHalfSizeWorld =
                camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -newPosition.z)) -
                camera.transform.position;

            if (!FixedX)
            {
                if (newPosition.x - screenHalfSizeWorld.x < LowerBorder.x)
                {
                    newPosition.x = LowerBorder.x + screenHalfSizeWorld.x;
                }
                else if (newPosition.x + screenHalfSizeWorld.x > UpperBorder.x)
                {
                    newPosition.x = UpperBorder.x - screenHalfSizeWorld.x;
                }
            }

            if (!FixedY)
            {
                if (newPosition.y - screenHalfSizeWorld.y < LowerBorder.y)
                {
                    newPosition.y = LowerBorder.y + screenHalfSizeWorld.y;
                }
                else if (newPosition.y + screenHalfSizeWorld.y > UpperBorder.y)
                {
                    newPosition.y = UpperBorder.y - screenHalfSizeWorld.y;
                }
            }

            oldPosition = newPosition;
            //smoothMover.TargetPosition = oldPosition; todo
        }

        private void Update()
        {
            if (MouseInputEnabled)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                    if (UIUtils.PointerOverUserInterface)
                    {
                        return;
                    }

                    Vector3 delta = Input.mousePosition - lastMousePosition;
                    Vector3 translate = new Vector3
                    {
                        x = FixedX ? 0f : -delta.x * MouseSensitivity,
                        y = FixedY ? 0f : -delta.y * MouseSensitivity,
                        z = 0f
                    };


                    Vector3 newPosition = oldPosition + translate;


                    MoveTo(newPosition);
                    lastMousePosition = Input.mousePosition;
                }
            }
        }
    }
}