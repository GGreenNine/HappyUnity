using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class FollowTarget : MonoBehaviour
    {
        [Header("Activity")] public bool FollowPosition = true;
        public bool FollowRotation = true;
        
        [Header("Locking")]
        public bool LockRotationByX;
        public bool LockRotationByY;
        public bool LockRotationByZ;
        
        [Header("Target")] public Transform Target;
        public Vector3 Offset;

        ///
        public bool AddInitialDistanceXToXOffset = false;
        public bool AddInitialDistanceYToYOffset = false;
        public bool AddInitialDistanceZToZOffset = false;

        [Header("Interpolation")] public bool InterpolatePosition = true;
        public bool InterpolateRotation = true;
        public float FollowPositionSpeed = 10f;
        public float FollowRotationSpeed = 10f;

        /// the possible update modes
        public enum Modes
        {
            Update,
            FixedUpdate,
            LateUpdate
        }

        [Header("Mode")] public Modes UpdateMode = Modes.Update;

        [Header("Axis")] public bool FollowPositionX = true;
       
        public bool FollowPositionY = true;
        public bool FollowPositionZ = true;

        protected Vector3 _newTargetPosition;
        protected Vector3 _initialPosition;

        protected Quaternion _newTargetRotation;
        protected Quaternion _initialRotation;
        
        
        [HideInInspector]
        public bool Lock = false;
        
        /// <summary>
        /// On start we store our initial position
        /// </summary>
        public virtual void Initialize()
        {
            SetInitialPosition();
            SetOffset();
            SetPostion();
        }

        /// <summary>
        /// Prevents the object from following the target anymore
        /// </summary>
        public virtual void StopFollowing()
        {
            if(this.Lock)
                return;
            
            Target = null;
            FollowPosition = false;
        }

        /// <summary>
        /// Makes the object follow the target
        /// </summary>
        public virtual void StartFollowing(bool Lock = false)
        {
            if(this.Lock)
                return;
            
            this.Lock = Lock;
            FollowPosition = true;
            SetInitialPosition();
        }

        /// <summary>
        /// Setting target
        /// </summary>
        public void SetTarget(Transform target)
        {
            if(this.Lock)
                return;
            
            Target = target;
            Initialize();
        }

        public void Unlock()
        {
            this.Lock = false;
        }

        public void SetPostion()
        {
            transform.position = Target.position;
        }

        /// <summary>
        /// Stores the initial position
        /// </summary>
        protected virtual void SetInitialPosition()
        {
            _initialPosition = this.transform.position;
            _initialRotation = this.transform.rotation;
        }

        protected virtual void SetOffset()
        {
            Vector3 difference = this.transform.position - Target.transform.position;
            Offset.x = AddInitialDistanceXToXOffset ? difference.x : Offset.x;
            Offset.y = AddInitialDistanceYToYOffset ? difference.y : Offset.y;
            Offset.z = AddInitialDistanceZToZOffset ? difference.z : Offset.z;
        }

        /// <summary>
        /// At update we follow our target 
        /// </summary>
        protected virtual void Update()
        {
            if (UpdateMode == Modes.Update)
            {
                FollowTargetPosition();
                FollowTargetRotation();
            }
        }

        /// <summary>
        /// At fixed update we follow our target 
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (UpdateMode == Modes.FixedUpdate)
            {
                FollowTargetPosition();
            }
        }

        /// <summary>
        /// At late update we follow our target 
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (UpdateMode == Modes.LateUpdate)
            {
                FollowTargetPosition();
            }
        }

        /// <summary>
        /// Follows the target, lerping the position or not based on what's been defined in the inspector
        /// </summary>
        protected virtual void FollowTargetPosition()
        {
            if (Target == null)
            {
                return;
            }

            if (!FollowPosition)
            {
                return;
            }

            _newTargetPosition = Target.position + Offset;
            if (!FollowPositionX)
            {
                _newTargetPosition.x = _initialPosition.x;
            }

            if (!FollowPositionY)
            {
                _newTargetPosition.y = _initialPosition.y;
            }

            if (!FollowPositionZ)
            {
                _newTargetPosition.z = _initialPosition.z;
            }

            if (InterpolatePosition)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, _newTargetPosition,
                    Time.deltaTime * FollowPositionSpeed);
            }
            else
            {
                this.transform.position = _newTargetPosition;
            }
        }

        protected virtual void FollowTargetRotation()
        {
            if (Target == null)
            {
                return;
            }

            if (!FollowRotation)
            {
                return;
            }

            _newTargetRotation = Target.rotation;

            var rotationX = LockRotationByX ? transform.rotation.x : _newTargetRotation.x; 
            var rotationY = LockRotationByY ? transform.rotation.y : _newTargetRotation.y; 
            var rotationZ = LockRotationByZ ? transform.rotation.z : _newTargetRotation.z; 

            
            if (InterpolateRotation)
            {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, new Quaternion(rotationX, rotationY, rotationZ, transform.rotation.w), 
                    Time.deltaTime * FollowRotationSpeed);
            }
            else
            {
                this.transform.rotation = new Quaternion(rotationX, rotationY, rotationZ, transform.rotation.w);
            }
        }
    }
