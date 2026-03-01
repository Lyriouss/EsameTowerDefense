using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    Rigidbody rb;

    //Variables that determine how enemy behaves
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] public float currentHealth;
    [SerializeField] private float damageToBase;
    [SerializeField] private int killMoney;

    public static event Action<int> OnEnemyKilled;
    public static event Action OnKillAdded;
    public static event Action<float> OnBaseReached;
    public static event Action<int> OnMoneyStolen;

    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Sets the movement direction of enemy from right to left
        direction = new Vector3(-1, 0, 0);

        //Gets healthMult from EnemySpawner and multiplies it to enemy health 
        currentHealth = maxHealth * EnemySpawner.Instance.healthMult;
    }

    private void FixedUpdate()
    {
        //Constantly moves the enemy in one direction
        rb.linearVelocity = direction * moveSpeed * Time.fixedDeltaTime;
    }

    //Interface function that damages enemy when bullet hits it
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Despawn()
    {
        //Updates variables in GameManager when health reaches 0 or lower and despawns enemy
        OnEnemyKilled?.Invoke(killMoney);
        OnKillAdded?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //When enemy reaches base
        if (other.CompareTag("Base"))
        {
            //deal damage to player base
            OnBaseReached?.Invoke(damageToBase);
            //steals money from player
            OnMoneyStolen?.Invoke(-killMoney);
            //then despawns enemy
            Destroy(gameObject);
        }
    }
}
