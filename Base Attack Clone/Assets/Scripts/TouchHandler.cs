using System.Collections;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private bool wasTouching = false;
    public float offset;
    public float expForce, radius;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private GameObject brokenWallPrefab;
    [SerializeField] private WeaponScriptableObject defaultWeapon;

    private Rect noTouchArea;

    private void Start()
    {
        Application.targetFrameRate = 60;
        noTouchArea = new Rect(GetX(-50.2f), GetY(385), GetX(1100), GetY(150));
        WeaponSelect.Instance.SelectWeapon(defaultWeapon);
    }

    private void FixedUpdate()
    {
        bool isTouching = Input.touchCount > 0;
        if (isTouching && !wasTouching && !noTouchArea.Contains(new Vector2(Input.GetTouch(0).position.x, Screen.height - Input.GetTouch(0).position.y)))
        {
            StartCoroutine(Fire());
        }
        wasTouching = isTouching;
    }

    private IEnumerator Fire()
    {
        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.red);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                Destroy(hit.transform.gameObject);
                Instantiate(brokenWallPrefab, hit.transform.position, hit.transform.rotation);
            }
            Knockback();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        Vector3 offsetPoint = hit.point - ray.direction.normalized * offset;
        Instantiate(WeaponManager.Instance.weapon.effectPrefab, offsetPoint, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }

    private void Knockback()
    {
        Collider[] colliders = Physics.OverlapSphere(hit.point, radius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(WeaponManager.Instance.weapon.explosionForce, hit.point, WeaponManager.Instance.weapon.explosionRadius);
            }
            if (nearby.CompareTag("Alien"))
            {
                FieldOfView.Instance.TakeDamage(WeaponManager.Instance.weapon.weaponDamage);
            }
        }
    }

    private float GetX(float valX)
    {
        float x = (valX / 800) * 100;
        return (x / 100) * Screen.width;
    }

    private float GetY(float valY)
    {
        float y = (valY / 480) * 100;
        return (y / 100) * Screen.height;
    }
}