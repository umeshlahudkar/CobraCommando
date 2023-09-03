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

    private DynamicJoystick rotateJoystick;
    private DynamicJoystick moveJoystick;
    private Slider healthBar;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float weaponDamagePoint;

    private bool isGrounded;
    private Transform camTransform;
    private Camera cam;

    private float movementX;
    private float movementZ;
    private float rotationX;
    private float rotationY;

    private float gravityMultiplier = 2.5f;

    private Vector3 rightMov;
    private Vector3 forwardMove;
    private Vector3 jumpVect;

    private Vector3 offset;

    private int killCount;

    [SerializeField] private float autoShootInterval = 0.5f;
    private float timeElapced = 0;
    private float health = 100.0f;

    private void Start()
    {
        cam = Camera.main;
        camTransform = cam.gameObject.transform.parent;
    }

    public void Initialize(DynamicJoystick _moveJoystick, DynamicJoystick _rotateJoystick, Slider _healthBar)
    {
        moveJoystick = _moveJoystick;
        rotateJoystick = _rotateJoystick;
        healthBar = _healthBar;
        SetHealth(health);
    }

    private void Update()
    {
        if(!GameManager.Instance.isGameRunning) { return; }
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
    }

    private void LateUpdate()
    {
        camTransform.transform.position = viewPoint.position;
        camTransform.transform.rotation = viewPoint.rotation;
    }

    private void CatchInput()
    {
        rotationX += rotateJoystick.Horizontal * mouseSensitivity;
        rotationY += rotateJoystick.Vertical * mouseSensitivity;
       
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
        ray.origin = camTransform.transform.position + camTransform.transform.forward;

        if(Physics.Raycast(ray, out RaycastHit ht))
        {
            EnemyController enemy = ht.collider.gameObject.GetComponent<EnemyController>();
            if (enemy != null && enemy.GetHealth() > 0)
            {
                Instantiate(bloodeffectParticle, ht.point, Quaternion.identity);
                Instantiate(muzzleFlashParticlePrefab, muzzleFlashPosition.position, muzzleFlashPosition.rotation);
                enemy.DealDamage(10, this);
                AudioManager.Instance.PlayFireSound();
            }
        }
    }

    public void DealDamage(float weaponDamage)
    {
        health -= weaponDamage;
        health = Mathf.Max(0, health);
        healthBar.value = health;

        if (health <= 0)
        {
            // GameplayUIController.Instance.EnableGameOverScreen();
        }
    }

    public void SetHealth(float _health)
    {
        health = _health;
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public float GetHealth()
    {
        return health;
    }

    public void IncreamentKillCount()
    {
        killCount++;
        UIController.Instance.UpdateKillText(killCount);
    }

    public float GetWeaponDamagePoint()
    {
        return weaponDamagePoint;
    }
}
