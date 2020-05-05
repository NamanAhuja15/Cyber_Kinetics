using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

    public enum PlayerStates
    {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public class PlayerMovement : MonoBehaviour
{
        public PlayerStates playerStates;


        public string HorizontalInput = "Horizontal";
        public string VerticalInput = "Vertical";
        public string RunInput = "Run";
        public string JumpInput = "Jump";


        [Range(1f, 15f)]
        public float walkSpeed;
        [Range(1f, 15f)]
        public float runSpeed;
        [Range(1f, 15f)]
        public float JumpForce;

 
        public Animator CharacterAnimator;
        public bool JumpAnimation;
        public bool LandAnimation;
        public bool Aim, Shoot,Crouch;


        public List<AudioClip> FootstepSounds;
        public List<AudioClip> JumpSounds;
        public List<AudioClip> LandSounds;

        private AudioManager audioManager;
        private CharacterController characterController;
        private float time,dummy;
        private GunScript gun;
        private GunInventory guninventory;
        private IKControl ik;
        // Use this for initialization
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            guninventory = gameObject.GetComponent<GunInventory>();
            ik = gameObject.GetComponent<IKControl>();
           audioManager = gameObject.GetComponent<AudioManager>();
            time = 0f;
        }
    void MoveToLayer(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            MoveToLayer(child.gameObject, layer);
        }
    }


    // Update is called once per frame
    void Update()
        {

            SetAnimation();
            HandleGunControls();
            HandlePlayerControls();
            time += Time.deltaTime;
                if(time>1f)
                {
                    ik.Holding_gun = true;
            ik.enabled = true;
                }
            
        }


    void HandleGunControls()
    {
        if (guninventory.gun_new != null)
        {
            if (gun == null)
                gun = guninventory.gun_new.GetComponent<GunScript>();
            if (Input.GetMouseButton(1))
            {
                Aim = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Aim = false;
            }
            if (Input.GetMouseButton(0))
            {
                Shoot = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Shoot = false;
            }
            gun.shooting = Shoot;
        }
    }
    void HandlePlayerControls()
    {
        float hInput = Input.GetAxisRaw(HorizontalInput);
        float vInput = Input.GetAxisRaw(VerticalInput);
        Vector3 fwdMovement = characterController.isGrounded == true ? transform.forward * vInput : Vector3.zero;
        Vector3 rightMovement = characterController.isGrounded == true ? transform.right * hInput : Vector3.zero;
         dummy = 0f;


        float _speed = Input.GetButton(RunInput) ? runSpeed : walkSpeed;
        bool run = _speed == runSpeed ? true : false;
        characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * _speed);
        if (characterController.isGrounded)
        {
            if (new Vector2(hInput, vInput).sqrMagnitude > 0.5f)
        {
            if (run)
            {
                audioManager.Walk(0.5f);
                dummy = 1f;
            }
            else
            {
                audioManager.Walk(1f);
                dummy = 0.5f;
            }
        }
        else
        {
            dummy = 0f;
        }

            if (JumpAnimation)
            {
                audioManager.Land();
                JumpAnimation = false;
            }
        }

        if (Input.GetButtonDown(JumpInput) && characterController.isGrounded)
        {
            StartCoroutine(PerformJumpRoutine());
            JumpAnimation = true;
        }
        if(Input.GetKey(KeyCode.C))
            {
                Crouch = true;

            }
        if(Input.GetKeyUp(KeyCode.C))
            {
                Crouch = false;
            }

        }

    IEnumerator PerformJumpRoutine()
    {
        audioManager.Jump();
        float _jump = JumpForce;
        do
        {
            characterController.Move(Vector3.up * _jump * Time.deltaTime);
            _jump -= Time.deltaTime;
            yield return null;
        }
        while (!characterController.isGrounded);

       
    }


        public void SetAnimation()
        {

            CharacterAnimator.SetBool("Jump", JumpAnimation);
            CharacterAnimator.SetBool("Aiming", Aim);
            CharacterAnimator.SetBool("Shooting", Shoot);
            CharacterAnimator.SetBool("Crouching", Crouch);

            CharacterAnimator.SetFloat("Blend", dummy);
            CharacterAnimator.SetFloat("Aim_Float", dummy);
            CharacterAnimator.SetFloat("Crouch_speed", dummy);

        }


}
