﻿using DG.Tweening;
using UnityEngine;
using Barji.Player.Movement;
using MilkShake;
using UnityEngine.Serialization;
using Barji.UI.Settings;
using Valve.VR;

namespace Barji.Player.Cam
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraLogic;
        [SerializeField] private ShakePreset lungePreset;
        [SerializeField] private Camera mainCam;
        public Transform orientation;
        private PlayerMovement movement;
        
        [Range(0.0001f, 10f)] [SerializeField] private float wallrunDamper;
        private float desiredX;
        private float xRotation;
        private float mouseX, mouseY;
        private bool cursorLocked;
        private float targetRot;
        
        [Header("Impact Settings")]
        [Range(0.0001f, 10f)][SerializeField] private float fallSmoothing;
        [Range(0.0001f, 10f)][SerializeField] private float impactSmoothing;
        [Range(-0.0001f, -1f)][SerializeField] private float maximumOffset;
        private float currentOffset;

        [SerializeField] private Vector2 sensitivity;
        private float sensitivityMultiplier;

        public bool VRPlayer;
        private void Awake()
        {
            SettingsManager.onSensitivityChanged += SetSensitivityMultiplier;
            SetSensitivityMultiplier();
            movement = GetComponent<PlayerMovement>();
        }

        public void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cursorLocked = true;
        }

        private void Update()
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity.x * sensitivityMultiplier * Time.fixedDeltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * sensitivityMultiplier * Time.fixedDeltaTime;
        }

        private void LateUpdate()
        {
            if(!VRPlayer)
                Rotation();
            CursorManager();
            CameraFall();
        }

        private void Rotation()
        {
            //Find current look rotation
            var rot = cameraLogic.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -88f, 88f);

            //Perform the rotations
            cameraLogic.localRotation = Quaternion.Euler(xRotation, desiredX, targetRot);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }

        private void CursorManager()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = !cursorLocked;
                if (cursorLocked)
                    Cursor.lockState = CursorLockMode.Locked;
                else
                    Cursor.lockState = CursorLockMode.None;
            }
        }

        private void CameraFall()
        {
            if(PlayerMovement.currentState == PlayerMovement.state.Airborn)
            {
                // > as the offset is negative
                if (mainCam.transform.localPosition.y > maximumOffset)
                {
                    currentOffset = Mathf.Lerp(currentOffset, maximumOffset, Time.deltaTime * (1 / fallSmoothing));
                }
            }
            else
            {
                currentOffset = Mathf.Lerp(currentOffset, 0, Time.deltaTime * (1 / impactSmoothing));
            }

            Vector3 camPos = mainCam.transform.localPosition;
            camPos = new Vector3(camPos.x, currentOffset, camPos.z);
            mainCam.transform.localPosition = camPos;
        }

        public void ShakeLunge()
        {
            Shaker.ShakeAll(lungePreset);
        }

        public void TweenTargetRot(float _endRot)
        {
            DOTween.To(() => targetRot, x => targetRot = x, _endRot, wallrunDamper).SetEase(Ease.OutExpo);
        }

        public void SetSensitivityMultiplier()
        {
            sensitivityMultiplier = SettingsManager.GetSensitivity() * 2;
        }
    }
}