﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasySurvivalScripts
{
    public enum CameraPerspective
    {
        FirstPerson,
        ThirdPerson
    }

    public class PlayerCamera : Mirror.NetworkBehaviour
    {

        [Header("Input Settings")]
        public string MouseXInput;
        public string MouseYInput;
        public string SwitchPerspectiveInput;

        [Header("Common Camera Settings")]
        public float mouseSensitivity;
        public CameraPerspective cameraPerspective;

        [Header("Character Animator")]
        public Animator CharacterAnimator;

        [Header("FPS Camera Settings")]
        public Vector3 FPS_CameraOffset;
        public Vector2 FPS_MinMaxAngles;

        [Header("TPS Camera Settings")]
        public Vector3 TPS_CameraOffset;
        public Vector2 TPS_MinMaxAngles;

        Transform FPSController;
        float xClamp;
        Vector3 camMoveLoc,offset;
        Transform _fpsCameraHelper;
        Transform _tpsCameraHelper;
        public GameObject head,Gun,Chest;
        private void Awake()
        {
           // Cursor.lockState = CursorLockMode.Locked;
            xClamp = 0;
            FPSController = GetComponentInParent<PlayerMovement>().transform;
        }

        // Use this for initialization
        void Start()
        {
            if (CharacterAnimator)
            {
                Add_FPSCamPositionHelper();
                Add_TPSCamPositionHelper();
            }
        }

        void Add_FPSCamPositionHelper()
        {
            _fpsCameraHelper = new GameObject().transform;
            _fpsCameraHelper.name = "_fpsCameraHelper";
            _fpsCameraHelper.SetParent(head.transform);
            _fpsCameraHelper.localPosition = Vector3.zero;
        }


        void Add_TPSCamPositionHelper()
        {
            _tpsCameraHelper = new GameObject().transform;
            _tpsCameraHelper.name = "_tpsCameraHelper";
            _tpsCameraHelper.SetParent(head.transform);
            _tpsCameraHelper.localPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            SwitchCameraPerspectiveInput();

            GetSetPerspective();

            RotateCamera();

        }

        void SwitchCameraPerspectiveInput()
        {
            if(Input.GetButtonDown(SwitchPerspectiveInput))
            {
                if (cameraPerspective == CameraPerspective.FirstPerson)
                {
                    cameraPerspective = CameraPerspective.ThirdPerson;
                }
                else
                {
                    cameraPerspective = CameraPerspective.FirstPerson;
                }
            }

        }

        void GetSetPerspective()
        {
            switch (cameraPerspective)
            {
                case CameraPerspective.FirstPerson:
                    SetCameraHelperPosition_FPS();
                    break;

                case CameraPerspective.ThirdPerson:
                    SetCameraHelperPosition_TPS();
                    break;
            }
        }

        void SetCameraHelperPosition_FPS()
        {
            if (!CharacterAnimator)
                return;

            _fpsCameraHelper.localPosition = FPS_CameraOffset;

            transform.position = _fpsCameraHelper.position;

        }

        void SetCameraHelperPosition_TPS()
        {
            if (!CharacterAnimator)
                return;

            _tpsCameraHelper.localPosition = TPS_CameraOffset;

            transform.position = _tpsCameraHelper.position;

        }

        void RotateCamera()
        {
          //  if (!locked())
           // {
                float mouseX = Input.GetAxis(MouseXInput) * (mouseSensitivity * Time.deltaTime);
                float mouseY = Input.GetAxis(MouseYInput) * (mouseSensitivity * Time.deltaTime);
                Vector3 eulerRotation = transform.eulerAngles;

                xClamp += mouseY;

                if (cameraPerspective == CameraPerspective.FirstPerson)
                    xClamp = Mathf.Clamp(xClamp, FPS_MinMaxAngles.x, FPS_MinMaxAngles.y);
                else
                    xClamp = Mathf.Clamp(xClamp, TPS_MinMaxAngles.x, TPS_MinMaxAngles.y);

                eulerRotation.x = -xClamp;
                transform.eulerAngles = eulerRotation;
              //  Gun.transform.eulerAngles = eulerRotation;
                FPSController.Rotate(Vector3.up * mouseX);
          //  Chest.transform.Rotate(Vector3.right * mouseY*2);
         //   }
           // else
           // Cursor.lockState = CursorLockMode.None;
        }
       // bool locked()
       // {


        
      //  }
        private void OnDrawGizmosSelected()
        {
            if (_fpsCameraHelper)
                Gizmos.DrawWireSphere(_fpsCameraHelper.position, 0.1f);

            Gizmos.color = Color.green;

            if (_tpsCameraHelper)
                Gizmos.DrawWireSphere(_tpsCameraHelper.position, 0.1f);
        }
    }
}