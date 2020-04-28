using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Player_Scripts
{
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
        public Transform FootLocation;

 
        public Animator CharacterAnimator;
        public float HorzAnimation;
        public float VertAnimation;
        public bool JumpAnimation;
        public bool LandAnimation;
        public bool Aim, Shoot,Crouch;


        public List<AudioClip> FootstepSounds;
        public List<AudioClip> JumpSounds;
        public List<AudioClip> LandSounds;

        private CharacterController characterController;
        private  AudioSource _audioSource;
        private float footstep_et = 0,time, _footstepDelay,dummy;
        private GunScript gun;
        private GunInventory guninventory;
        private IKControl ik;
        // Use this for initialization
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            _audioSource = gameObject.AddComponent<AudioSource>();
            guninventory = gameObject.GetComponent<GunInventory>();
            ik = gameObject.GetComponent<IKControl>();
            time = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            //handle controller
           // if (isLocalPlayer)
            {
                SetAnimation();
                HandlePlayerControls();
               HandleGunControls();
                time += Time.deltaTime;
                if(time>1f)
                {
                    ik.Holding_gun = true;
                }

            }
        }


        void HandleGunControls()
        {
            if (gun == null)
                gun = guninventory.gun_new.GetComponent<GunScript>();
            if (Input.GetMouseButton(1))
            {
                Aim = true;
            }
            else if(Input.GetMouseButtonUp(1))
            {
                Aim = false;
            }
            if (Input.GetMouseButton(0))
            {
                Shoot = true;
            }
            else if(Input.GetMouseButtonUp(0))
                Shoot = false;


            if(Input.GetKeyDown(KeyCode.R)||gun.bulletsInTheGun==0)
            {
                gun.Reload();
                CharacterAnimator.SetTrigger("Reload");
                ik.Holding_gun = false;
                time = 0f;
            }


            gun.shooting = Shoot;


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

            if (new Vector2(hInput, vInput).sqrMagnitude > 0.1f)
            {
                if (run)
                    dummy = 1f;
                else
                    dummy = 0.5f;

            }
            else
                dummy = 0f;
        if (characterController.isGrounded)
            JumpAnimation = false;

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
        //play jump sound
        //     if (_audioSource)
        //   _audioSource.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Count)]);

        float _jump = JumpForce;

        do
        {
            characterController.Move(Vector3.up * _jump * Time.deltaTime);
            _jump -= Time.deltaTime;
            yield return null;
        }
        while (!characterController.isGrounded);

        //play land sound
        if (_audioSource)
            _audioSource.PlayOneShot(LandSounds[Random.Range(0, LandSounds.Count)]);

    }

    bool onGround()
    {
        bool retVal = false;

        if (Physics.Raycast(FootLocation.position, Vector3.down, 0.1f))
            retVal = true;
        else
            retVal = false;

        return retVal;
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
            if (CharacterAnimator.GetFloat("Aim_Float") > 0.1f)
                Aim = true;
            if (CharacterAnimator.GetFloat("Aim_Float") < 0.1f)
            {
                Aim = false;
            }


        }
        void PlayFootstepSounds()
    {
        if (playerStates == PlayerStates.Idle || playerStates == PlayerStates.Jumping)
            return;

        if (footstep_et < _footstepDelay)
            footstep_et += Time.deltaTime;
        else
        {
            footstep_et = 0;
            _audioSource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Count)]);
        }
    }

}
}