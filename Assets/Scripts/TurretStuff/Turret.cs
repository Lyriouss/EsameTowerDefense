using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public static event Action<Vector3> onBulletSpawn;

    [Header("Generate Bullet Elements")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform barrel;
    [SerializeField] private Transform range;
    [SerializeField] private Transform instantiateBullets;

    [Header("Turret Characteristics")]
    [SerializeField] private float bulletRate;
    [SerializeField] private float damage;
    [SerializeField] private int cost;

    private float bulletTimer;

    private void Start()
    {
        bulletTimer = 0f;
    }

    private void Update()
    {
        bulletTimer += Time.deltaTime;

        if (bulletTimer < bulletRate)
            return;

        bulletTimer = 0f;

        SpawnBullet();
    }

    private void SpawnBullet()
    {
        Instantiate(bullet, barrel.position, Quaternion.identity, instantiateBullets);

        onBulletSpawn?.Invoke(range.position);
    }
}
