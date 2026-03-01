using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    //Bullets move with RigidBody
    Rigidbody rb;

    Vector3 startPos;

    private float bulletSpeed;
    private float bulletRange;
    [HideInInspector] public int bulletDamage;

    public virtual void Start()
    {
        //Initialize RigidBody
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;

        bulletSpeed = GetComponentInParent<Turret>().speed;
        bulletRange = GetComponentInParent<Turret>().range;
        bulletDamage = GetComponentInParent<Turret>().damage;
    }

    public virtual void FixedUpdate()
    {
        //Moves the bullet forward relative to the rotation of the game object
        rb.linearVelocity = transform.forward * bulletSpeed * Time.fixedDeltaTime;

        float distance = Vector3.Distance(transform.position, startPos);

        if (distance > bulletRange)
            Destroy(gameObject);
    }
}
