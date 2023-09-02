using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


namespace FPS.Multiplayer
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform viewPoint;
        [SerializeField] private Transform thisTransform;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private GameObject muzzleFlashParticlePrefab;
        [SerializeField] private Transform muzzleFlashPosition1;
        [SerializeField] private Transform muzzleFlashPosition2;

        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private ParticleSystem bloodeffectParticle;
        [SerializeField] private Animator animator;

        [SerializeField] private GameObject character;
        [SerializeField] GameObject rifle;

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
        private string playingAnimation = "";
        private string currentAnimationState;

        private DynamicJoystick rotateJoystick;
        private DynamicJoystick moveJoystick;
        private Button fireButton;

        private Vector3 offset;

        [SerializeField] private float autoShootInterval = 0.5f;
        private float timeElapced = 0;

        private void Start()
        {
            if (photonView.IsMine)
            {
                character.SetActive(false);
                rifle.SetActive(true);
                //animator.enabled = false;
            }
            else
            {
                character.SetActive(true);
                rifle.SetActive(false);
                //animator.enabled = true;
            }
        }

        public void SetUpPlayer(Slider healthBar, DynamicJoystick _rotateJoystick, DynamicJoystick _moveJoystick, Button _fireButton)
        {
            cam = Camera.main;
            //Cursor.lockState = CursorLockMode.Locked;
            healthController = GetComponent<HealthController>();
            healthController.SetupHealth(healthBar);
            rotateJoystick = _rotateJoystick;
            moveJoystick = _moveJoystick;
            fireButton = _fireButton;

            fireButton.onClick.AddListener(Shoot);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                CatchInput();
                Rotate();
                LookUpDown();
                MoveForwardBackWard();
                MoveLeftRight();
                PlayAnimation(moveJoystick.Horizontal, moveJoystick.Vertical);

                offset.x = thisTransform.position.x;
                offset.z = thisTransform.position.z;
                offset.y = 1;
                thisTransform.position = offset;

                timeElapced += Time.deltaTime;
                if (timeElapced > autoShootInterval)
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
        }

        private void LateUpdate()
        {
            if (photonView.IsMine)
            {
                cam.transform.position = viewPoint.position;
                cam.transform.rotation = viewPoint.rotation;
            }
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

        private void PlayAnimation(float a, float b)
        {
            string targetAnimationState;
            if (Mathf.Abs(a) >= 0.1f || Mathf.Abs(b) >= 0.1f)
            {
                targetAnimationState = "Run";
                //animator.ResetTrigger("Stop");
                photonView.RPC(nameof(PlayAnim), RpcTarget.All, targetAnimationState);
            }
            else if(Mathf.Abs(a) == 0f || Mathf.Abs(b) == 0f)
            {
                targetAnimationState = "Stop";
                //animator.ResetTrigger("Run");
                photonView.RPC(nameof(PlayAnim), RpcTarget.All, targetAnimationState);
            }

            //if (targetAnimationState != currentAnimationState)
            //{
            //    currentAnimationState = targetAnimationState;
            //    photonView.RPC(nameof(PlayAnim), RpcTarget.All, currentAnimationState);
            //}
        }

        [PunRPC]
        private void PlayAnim(string animationName)
        {
            //if (animator.enabled)
            {

                animator.ResetTrigger("Run");
                animator.ResetTrigger("Stop");
                animator.SetTrigger(animationName);
            }
        }

        private void Shoot()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin = cam.transform.position + cam.transform.forward;

            //RaycastHit[] raycastHits = Physics.RaycastAll(ray.origin, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit ht))
            //foreach (RaycastHit ht in raycastHits)
            {
                PhotonView view = ht.collider.gameObject.GetPhotonView();
                if (view != null)//&& view != photonView)
                {
                    PlayerController controller = view.GetComponent<PlayerController>();
                    view.RPC(nameof(DealDamage), RpcTarget.All, photonView.Owner.NickName, controller.GetWeaponDamagePoint(), photonView.Owner.ActorNumber);
                    PhotonNetwork.Instantiate(bloodeffectParticle.name, ht.point, Quaternion.identity);
                    PhotonNetwork.Instantiate(muzzleFlashParticlePrefab.name, muzzleFlashPosition1.position, muzzleFlashPosition1.rotation);
                    AudioManager.Instance.PlayFireSound();
                    //break;
                }
            }

            //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //ray.origin = cam.transform.position + cam.transform.forward;

            ////RaycastHit[] raycastHits = Physics.RaycastAll(ray.origin, transform.forward);

            //if (Physics.Raycast(ray, out RaycastHit ht))

            ////foreach (RaycastHit ht in raycastHits)
            //{
            //    EnemyController enemy = ht.collider.gameObject.GetComponent<EnemyController>();
            //    if (enemy != null && enemy.GetComponent<HealthController>().GetHealth() > 0)
            //    {
            //        Instantiate(bloodeffectParticle, ht.point, Quaternion.identity);
            //        Instantiate(muzzleFlashParticlePrefab, muzzleFlashPosition.position, muzzleFlashPosition.rotation);
            //        enemy.DealDamage(10);
            //        AudioManager.Instance.PlayFireSound();
            //        //break;
            //    }
            //}

        }

        [PunRPC]
        private void DealDamage(string playerName, float weaponDamage, int actor)
        {
            if (photonView.IsMine)
            {
                healthController.DecreamentHealth(weaponDamage, actor);
            }
        }



        public float GetWeaponDamagePoint()
        {
            return weaponDamagePoint;
        }
    }
}
