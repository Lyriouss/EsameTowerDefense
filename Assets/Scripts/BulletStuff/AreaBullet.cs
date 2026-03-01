using UnityEngine;

public class AreaBullet : Bullet
{
    [SerializeField] LayerMask enemy;
    private float explosionArea;
    private float areaScaleMult;
    private float fixAreaScale;
    [SerializeField] private GameObject explosionAreaObj;

    public override void Start()
    {
        base.Start();

        explosionArea = GetComponentInParent<AreaTurret>().explosionArea;

        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        areaScaleMult = 1 / transform.localScale.x;
        fixAreaScale = areaScaleMult * explosionArea * 2;

        explosionAreaObj.transform.localScale = new Vector3(fixAreaScale, fixAreaScale, fixAreaScale);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Collider[] explodeArea = Physics.OverlapSphere(transform.position, explosionArea, enemy);

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
