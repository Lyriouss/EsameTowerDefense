using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    Rigidbody rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] public float currentHealth;
    [SerializeField] private float damageToBase;
    [SerializeField] private int killMoney;

    public static event Action<int> OnEnemyKilled;
    public static event Action OnKillAdded;
    //public delegate void OnEnemyKilled(int money);
    //public static OnEnemyKilled onEnemyKilled;

    public static event Action<float> OnBaseReached;

    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Sets the movement direction of enemy from right to left
        direction = new Vector3(-1, 0, 0);

        currentHealth = maxHealth * EnemySpawner.Instance.healthMult;
    }

    private void FixedUpdate()
    {
        //Constantly moves the enemy in one direction
        rb.linearVelocity = direction * moveSpeed * Time.fixedDeltaTime;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Took Damage");
    }

    public void Despawn()
    {
        OnEnemyKilled?.Invoke(killMoney);
        OnKillAdded?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            OnBaseReached?.Invoke(damageToBase);
            Destroy(gameObject);
        }
    }
}
