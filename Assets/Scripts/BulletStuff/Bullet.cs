using UnityEngine;

//Abstract parent class for child bullet scripts
public abstract class Bullet : MonoBehaviour
{
    Rigidbody rb;

    Vector3 startPos;

    private float bulletSpeed;
    private float bulletRange;
    [HideInInspector] public int bulletDamage;

    public virtual void Start()
    {
        //Initialize RigidBody
        rb = GetComponent<Rigidbody>();

        //startPos is equal to it's spawn location
        startPos = transform.position;

        //Gets variables of turret that spawns this bullet
        bulletSpeed = GetComponentInParent<Turret>().speed;
        bulletRange = GetComponentInParent<Turret>().range;
        bulletDamage = GetComponentInParent<Turret>().damage;
    }

    public virtual void FixedUpdate()
    {
        //Moves the bullet forward relative to the rotation of the game object
        rb.linearVelocity = transform.forward * bulletSpeed * Time.fixedDeltaTime;

        //Gets distance from current position and where it spawned
        float distance = Vector3.Distance(transform.position, startPos);

        //Once the bullet exceeds the distance allowed from turret
        if (distance > bulletRange)
            //Despawns bullet
            Destroy(gameObject);
    }
}
