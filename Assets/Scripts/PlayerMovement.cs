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
        public bool Aim, Shoot;


        public List<AudioClip> FootstepSounds;
        public List<AudioClip> JumpSounds;
        public List<AudioClip> LandSounds;

        CharacterController characterController;

        float _footstepDelay;
        AudioSource _audioSource;
        float footstep_et = 0;
        public GunScript gun;
        private GunInventory guninventory;
        // Use this for initialization
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            _audioSource = gameObject.AddComponent<AudioSource>();
            guninventory = gameObject.GetComponent<GunInventory>();
        }

        // Update is called once per frame
        void Update()
        {
            //handle controller
           // if (isLocalPlayer)
            {

                HandlePlayerControls();
               HandleGunControls();
                PlayFootstepSounds();
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
            else
            {
                Aim = false;
            }
            if (Input.GetMouseButton(0))
            {
                Shoot = true;
                gun.ShootMethod();
            }
            else
                Shoot = false;


            if(Input.GetKeyDown(KeyCode.R)||gun.bulletsInTheGun==0)
            {
                gun.Reload();
                CharacterAnimator.SetTrigger("Reload");
            }


            gun.shooting = Shoot;


        }
    void HandlePlayerControls()
    {
        float hInput = Input.GetAxisRaw(HorizontalInput);
        float vInput = Input.GetAxisRaw(VerticalInput);
        Vector3 fwdMovement = characterController.isGrounded == true ? transform.forward * vInput : Vector3.zero;
        Vector3 rightMovement = characterController.isGrounded == true ? transform.right * hInput : Vector3.zero;
        float dummy = 0f;


        float _speed = Input.GetButton(RunInput) ? runSpeed : walkSpeed;
        bool run = _speed == runSpeed ? true : false;
        characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * _speed);

        if (new Vector2(hInput, vInput).sqrMagnitude > 0.5f)
        {
            if (run)
                dummy = 1f;
            else
                dummy = 0.5f;

        }

        if (characterController.isGrounded)
            JumpAnimation = false;

        if (Input.GetButtonDown(JumpInput) && characterController.isGrounded)
        {
            StartCoroutine(PerformJumpRoutine());
            JumpAnimation = true;
        }
        if (!Aim&&!Shoot)
        {
            CharacterAnimator.SetFloat("Blend", dummy);
        }
        else 
        {
            CharacterAnimator.SetFloat("Aim_Float", dummy);
        }

            CharacterAnimator.SetBool("Jump", JumpAnimation);
            CharacterAnimator.SetBool("Aiming", Aim);
            CharacterAnimator.SetBool("Shooting", Shoot);
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