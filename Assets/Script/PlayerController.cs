using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform viewPoint;
        [SerializeField] private Transform thisTransform;
        [SerializeField] public CharacterController characterController;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private GameObject muzzleFlashParticlePrefab;
        [SerializeField] private Transform muzzleFlashPosition;
        [SerializeField] private ParticleSystem bloodeffectParticle;

        [SerializeField] private DynamicJoystick rotateJoystick;
        [SerializeField] private DynamicJoystick moveJoystick;

        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float weaponDamagePoint;

        private bool isGrounded;
        private Camera cam;

        private float movementX;
        private float movementZ;
        private float rotationX;
        private float rotationY;

        private float gravityMultiplier = 2.5f;

        private Vector3 rightMov;
        private Vector3 forwardMove;
        private Vector3 jumpVect;

        private HealthController healthController;
        private Vector3 offset;

        private int killCount;
        private int deathCount;

        [SerializeField] private float autoShootInterval = 0.5f;
        private float timeElapced = 0;

        private void Start()
        {
            cam = Camera.main;
            healthController = GetComponent<HealthController>();
        }

        private void Update()
        {
            if(!GameManager.Instance.isLevelStart) { return; }
            CatchInput();
            Rotate();
            LookUpDown();
            MoveForwardBackWard();
            MoveLeftRight();

            offset.x = thisTransform.position.x;
            offset.z = thisTransform.position.z;
            offset.y = 1.5f;
            thisTransform.position = offset;

            timeElapced += Time.deltaTime;
            if(timeElapced > autoShootInterval)
            {
                timeElapced = 0;
                Shoot();
            }
            //isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayer);
            //if(characterController.isGrounded)
            //{
            //    jumpVect.y = 0;
            //}


            //if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            //{
            //    jumpVect.y += jumpForce;
            //}

            //jumpVect.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            //characterController.Move(jumpVect * Time.deltaTime);
        }

        private void LateUpdate()
        {
            cam.transform.position = viewPoint.position;
            cam.transform.rotation = viewPoint.rotation;
        }

        private void CatchInput()
        {
            //rotationX += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            //rotationY += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            rotationX += rotateJoystick.Horizontal * mouseSensitivity;
            rotationY += rotateJoystick.Vertical * mouseSensitivity;

            //movementX = Input.GetAxisRaw("Horizontal");
            //movementZ = Input.GetAxisRaw("Vertical");

            movementX = moveJoystick.Horizontal;
            movementZ = moveJoystick.Vertical;
        }

        private void Rotate()
        {
            thisTransform.rotation = Quaternion.Euler(thisTransform.eulerAngles.x, rotationX, thisTransform.eulerAngles.z);
        }

        private void LookUpDown()
        {
            rotationY = Mathf.Clamp(rotationY, -60, 60);
            viewPoint.rotation = Quaternion.Euler(-rotationY, viewPoint.eulerAngles.y, viewPoint.eulerAngles.z);
        }

        private void MoveForwardBackWard()
        {
            if (movementX != 0)
            {
                rightMov = thisTransform.right * movementX * movementSpeed * Time.deltaTime;
                characterController.Move(rightMov);
            }
        }

        private void MoveLeftRight()
        {
            if (movementZ != 0)
            {
                forwardMove = thisTransform.forward * movementZ * movementSpeed * Time.deltaTime;
                characterController.Move(forwardMove);
            }
        }

        public void Shoot()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin = cam.transform.position + cam.transform.forward;

            //RaycastHit[] raycastHits = Physics.RaycastAll(ray.origin, transform.forward);

            if(Physics.Raycast(ray, out RaycastHit ht))

            //foreach (RaycastHit ht in raycastHits)
            {
                EnemyController enemy = ht.collider.gameObject.GetComponent<EnemyController>();
                if (enemy != null && enemy.GetComponent<HealthController>().GetHealth() > 0)
                {
                    Instantiate(bloodeffectParticle, ht.point, Quaternion.identity);
                    Instantiate(muzzleFlashParticlePrefab, muzzleFlashPosition.position, muzzleFlashPosition.rotation);
                    enemy.DealDamage(10);
                    AudioManager.Instance.PlayFireSound();
                    //break;
                }
            }
        }

        public void DealDamage(float weaponDamage)
        {
            healthController.DecreamentHealth(weaponDamage);
            if(healthController.GetHealth() <= 0)
            {
                IncrementDeathCount();
               // GameplayUIController.Instance.EnableGameOverScreen();
            }
        }

        public void ResetHealth()
        {
            healthController.SetHealth(100);
        }

        public float GetHealth()
        {
            return healthController.GetHealth();
        }

        public void IncreamentKillCount()
        {
            killCount++;
            //GameplayUIController.Instance.UpdateKillText(killCount);
        }

        public void IncrementDeathCount()
        {
            deathCount++;
            //GameplayUIController.Instance.UpdateDeathText(deathCount);
        }


        public float GetWeaponDamagePoint()
        {
            return weaponDamagePoint;
        }
    }
