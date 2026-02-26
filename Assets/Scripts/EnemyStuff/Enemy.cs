using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float damageToBase;
    [SerializeField] private int killMoney;

    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Sets the movement direction of enemy from right to left
        direction = new Vector3(-1, 0, 0);
    }

    private void FixedUpdate()
    {
        //Constantly moves the enemy in one direction
        rb.linearVelocity = direction * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Destroy(gameObject);
        }
    }
}
