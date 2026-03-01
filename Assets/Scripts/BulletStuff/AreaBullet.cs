using UnityEngine;

public class AreaBullet : Bullet
{
    //Variables used for explosion area of bullet
    [SerializeField] LayerMask enemy;
    private float explosionArea;
    private float areaScaleMult;
    private float fixAreaScale;
    [SerializeField] private GameObject explosionAreaObj;

    //Runs Start() of parent class
    public override void Start()
    {
        base.Start();

        //Additionally makes bullet a fixed size
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //and gets explosionArea variable of turret that spawned it
        explosionArea = GetComponentInParent<AreaTurret>().explosionArea;

        //Equation to make explosion area object same size as OverlapSphere
        areaScaleMult = 1 / transform.localScale.x;
        fixAreaScale = areaScaleMult * explosionArea * 2;
        explosionAreaObj.transform.localScale = new Vector3(fixAreaScale, fixAreaScale, fixAreaScale);
    }

    //Runs FixedUpdate() of parent class
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            //Makes an OverlapSphere to get all enemies in explosion area
            Collider[] explodeArea = Physics.OverlapSphere(transform.position, explosionArea, enemy);

            //Every enemy present in OverlapSphere takes damage
            foreach (Collider enemy in explodeArea)
            {
                enemy.TryGetComponent(out IDamageable damageable);
                enemy.TryGetComponent(out Enemy enemyObj);

                damageable.TakeDamage(bulletDamage);

                if (enemyObj.currentHealth <= 0)
                    damageable.Despawn();
            }

            //then destroys bullet
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionArea);
    }
}
