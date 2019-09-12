using System;
using UnityEngine;

namespace ThirdPersonController
{
    /// <summary>
    /// Allows for basic character movement physics and actions.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterControllerMovementSettings m_MovementSettings = null;
        [SerializeField] private CharacterControllerPhysicsSettings m_PhysicsSettings = null;

        private CharacterController m_CharacterController;
        private Vector3 m_Motion;
        private bool m_ResetGroundStickForce;
        private float m_LockTimer;
        private bool m_WalkMode;

        /// <summary>
        /// Gets a value indicating whether the character is on the ground.
        /// </summary>
        public bool isGrounded { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the character is sprinting.
        /// </summary>
        public bool isSprinting { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the character is crouching.
        /// </summary>
        public bool isCrouching { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the character is jumping.
        /// </summary>
        public bool isJumping { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the character is movement locked.
        /// </summary>
        /// <value></value>
        public bool isMovementLocked => Time.fixedTime < m_LockTimer;

        /// <summary>
        /// Gets the velocity of the <see cref="CharacterController" />
        /// </summary>
        public Vector3 velocity => m_CharacterController.velocity;
        
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Drives the <see cref="CharacterController" /> using the given input values.
        /// </summary>
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
            m_CharacterController.Move((m_Motion + jumpMotion) * Time.fixedDeltaTime);
            
            isGrounded = m_CharacterController.isGrounded;

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
                    m_CharacterController.radius, 
                    Vector3.up, 
                    out var hit, 
                    m_PhysicsSettings.headCheckDistance, 
                    m_PhysicsSettings.headCheckLayers);
        }

        protected virtual void UpdateCrouch()
        {
            if (isCrouching)
            {
                m_CharacterController.height = Mathf.Lerp(
                    m_CharacterController.height, 
                    m_PhysicsSettings.crouchHeight, 
                    Time.fixedDeltaTime * m_PhysicsSettings.crouchHeightLerpSpeed);
            }
            else
            {
                m_CharacterController.height = Mathf.Lerp(
                    m_CharacterController.height, 
                    m_PhysicsSettings.standingHeight, 
                    Time.fixedDeltaTime * m_PhysicsSettings.crouchHeightLerpSpeed);
            }

            m_CharacterController.center = new Vector3(0, m_CharacterController.height / 2f, 0);
        }

        /// <summary>
        /// Gets the motion, that is added onto the standard input motion, while jumping.
        /// </summary>
        protected virtual Vector3 GetJumpMotion()
        {
            return Vector3.up * m_PhysicsSettings.jumpForce * Time.fixedDeltaTime;
        }

        /// <summary>
        /// Gets the desired movement speed used by the character given their movement state.
        /// </summary>
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

        /// <summary>
        /// Stun/movement locks the character for the given period of time.
        /// </summary>
        /// <param name="time">The time period.</param>
        public virtual void MoveLock(float time)
        {
            m_LockTimer = Time.fixedTime + time;
        }

        /// <summary>
        /// Sets the character into walk mode.
        /// </summary>
        /// <param name="walkMode"></param>
        public virtual void SetWalkMode(bool walkMode)
        {
            this.m_WalkMode = walkMode;
        }
        
        /// <summary>
        /// Rotate the character to the given y rotation.
        /// </summary>
        /// <param name="yRotation">The y rotation.</param>
        public virtual void Rotate(float yRotation)
        {
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
