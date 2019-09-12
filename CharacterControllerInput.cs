using UnityEngine;

namespace ThirdPersonController
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerInput : MonoBehaviour
    {
        [SerializeField] private CharacterControllerInputSettings m_InputSettings = null;

        private CharacterController m_CharacterController;
        private Vector3 m_MovementInput;
        private bool m_CrouchInput;
        private bool m_SprintInput;
        private bool m_JumpInput;

        /// Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        /// Update is called every frame, if the MonoBehaviour is enabled.
        protected virtual void Update()
        {
            m_MovementInput = new Vector3(
                InputManager.GetAxis(m_InputSettings.horizontalInput), 0,
                InputManager.GetAxis(m_InputSettings.verticalInput));

            m_MovementInput = transform.TransformDirection(m_MovementInput);

            m_CrouchInput = InputManager.GetButton(m_InputSettings.crouchInput);
            m_SprintInput = InputManager.GetButton(m_InputSettings.sprintInput);
            
            if (InputManager.GetButtonDown(m_InputSettings.jumpInput))
            {
                m_JumpInput = true;
            }
        }

        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        protected virtual void FixedUpdate()
        {
            m_CharacterController.Move(m_MovementInput.x, m_MovementInput.z, m_SprintInput, m_CrouchInput, m_JumpInput);
            m_JumpInput = false;
        }
    }
}
