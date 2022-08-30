using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FieldOfView : MonoBehaviour
{
    public static FieldOfView Instance;

    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public Animator _animator;
    [SerializeField] private int damage;
    private float shootDelay = 0f;

    [SerializeField] private float health;
    [SerializeField] private Slider healthbar;
    [SerializeField] private Camera myCam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        _animator.SetFloat("AlienState", 0);
        healthbar.value = health / 100;
    }

    private void FixedUpdate()
    {
        if (canSeePlayer)
        {
            shootDelay -= Time.deltaTime;
            if (shootDelay <= 0)
            {
                Shoot();
            }
        }

        healthbar.transform.LookAt(healthbar.transform.position + myCam.transform.rotation * Vector3.back, myCam.transform.rotation * Vector3.down);
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    Vector3 lookVector = playerRef.transform.position - transform.position;
                    lookVector.y = transform.position.y;
                    Quaternion rot = Quaternion.LookRotation(lookVector);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
                    _animator.SetFloat("AlienState", 1);
                }
                    
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void Shoot()
    {   
        if(shootDelay <= 0)
        {
            PlayerHealth.Instance.TakeDamage(damage);
            shootDelay = 2f;
        }
    }

    public void UpdateHealthbar()
    {
        healthbar.value = health / 100;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthbar();
    }
}
