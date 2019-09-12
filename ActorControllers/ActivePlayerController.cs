using System;
using UnityEngine;

namespace ActivePlayerController
{
    [RequireComponent(typeof(ActivePlayerController))]
    public class ActiveCharacterController : MonoBehaviour
    {
        [SerializeField] private ActiveCharacterControllerMovementSettings m_MovementSettings = null;
        [SerializeField] private ActiveCharacterControllerPhysicsSettings m_PhysicsSettings = null;

        private ActiveCharacterController m_ActiveCharacterController;
        private Vector3 m_Motion;
        private bool m_ResetGroundStickForce;
        private float m_LockTimer;
        private bool m_WalkMode;

        public bool isGrounded { get; private set; }
        public bool isSprinting { get; private set; }
        public bool isCrouching { get; private set; }
        public bool isJumping { get; private set; }

        ///Value indicating whether the character is movement locked, or not///
        public bool isMovementLocked => Time.fixedTime < m_LockTimer;
        public Vector3 velocity => m_ActiveCharacterController.velocity;
        
        /// Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            m_ActiveCharacterController = GetComponent<ActiveCharacterController>();
        }

        /// <param name="x">The horizontal input value.</param>
        /// <param name="z">The vertical input value.</param>
        /// <param name="sprint">The sprinting input value.</param>
        /// <param name="crouch">The crocuhing input value.</param>
        /// <param name="jump">The jumping input value.</param>
        public virtual void Move(float x, float z, bool sprint, bool crouch, bool jump)
        {
            if (isGrounded)
            {
                if (isMovementLocked)
                {
                    x = z = 0;
                }

                isCrouching = crouch || CheckCrouch();
                isSprinting = sprint && !isCrouching && x != 0 && z != 0 && !m_WalkMode;

                m_Motion = new Vector3(x, 0, z).normalized * GetDesiredMovementSpeed();
                m_Motion += Physics.gravity * m_PhysicsSettings.groundStickForceMultiplier;

                if (jump && !isMovementLocked) 
                {
                    isJumping = true;
                    m_Motion.y = 0;
                }

                m_ResetGroundStickForce = false;
            }
            else
            {
                if (!m_ResetGroundStickForce)
                {
                    m_Motion.y = 0;
                    m_ResetGroundStickForce = true;
                }

                m_Motion += Physics.gravity * Time.fixedDeltaTime;
            }

            var jumpMotion = isJumping ? GetJumpMotion() : Vector3.zero;
            m_ActiveCharacterController.Move((m_Motion + jumpMotion) * Time.fixedDeltaTime);
            
            isGrounded = m_ActiveCharacterController.isGrounded;

            if (isGrounded)
            {
                isJumping = false;
            }
            else
            {
                isCrouching = false;
                isSprinting = false;
            }

            UpdateCrouch();
        }

        protected virtual bool CheckCrouch()
        {
            return 
                Physics.SphereCast(
                    transform.position, 
                    m_ActiveCharacterController.radius, 
                    Vector3.up, 
                    out var hit, 
                    m_PhysicsSettings.headCheckDistance, 
                    m_PhysicsSettings.headCheckLayers);
        }

        protected virtual void UpdateCrouch()
        {
            if (isCrouching)
            {
                m_ActiveCharacterController.height = Mathf.Lerp(
                    m_ActiveCharacterController.height, 
                    m_PhysicsSettings.crouchHeight, 
                    Time.fixedDeltaTime * m_PhysicsSettings.crouchHeightLerpSpeed);
            }
            else
            {
                m_ActiveCharacterController.height = Mathf.Lerp(
                    m_ActiveCharacterController.height, 
                    m_PhysicsSettings.standingHeight, 
                    Time.fixedDeltaTime * m_PhysicsSettings.crouchHeightLerpSpeed);
            }

            m_ActiveCharacterController.center = new Vector3(0, m_ActiveCharacterController.height / 2f, 0);
        }

        /// Gets standard input motion while actively jumping///
        protected virtual Vector3 GetJumpMotion()
        {
            return Vector3.up * m_PhysicsSettings.jumpForce * Time.fixedDeltaTime;
        }

        ///Movement speed while in movement state///
        protected virtual float GetDesiredMovementSpeed()
        {
            if (m_WalkMode)
            {
                return isCrouching ? m_MovementSettings.crouchSpeed : m_MovementSettings.walkSpeed;
            }

            float desiredSpeed = 
                isSprinting ? m_MovementSettings.sprintSpeed :
                isCrouching ? m_MovementSettings.crouchSpeed :
                m_MovementSettings.walkSpeed;

            return desiredSpeed;
        }

        ///Stun state///
        public virtual void MoveLock(float time)
        {
            m_LockTimer = Time.fixedTime + time;
        }

        ///Sets the character into walk mode///
        public virtual void SetWalkMode(bool walkMode)
        {
            this.m_WalkMode = walkMode;
        }
        
        /// Rotate the character to the given y rotation///
        public virtual void Rotate(float yRotation)
        {
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
