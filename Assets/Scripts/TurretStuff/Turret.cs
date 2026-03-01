using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [SerializeField] Transform turretBody;
    public TMP_Text upgradeCostUI;
    [SerializeField] LayerMask enemy;
    private Transform playerBase;
    private float lowestDistance;
    private Transform targetEnemy;

    [Header("Turret Characteristic")]
    public float bulletRate;
    private float bulletTimer;
    public int damage;
    public float range;
    public float speed;
    public int upgradeCost;

    [Header("Generate Bullet Elements")]
    [SerializeField] private GameObject bulletType;
    [SerializeField] private Transform head;

    public static event Action<int> OnStart;
    public static event Action OnTurretPlacement;

    public delegate void OnUpgradeTurret(int cost);
    public static OnUpgradeTurret onUpgradeTurret;

    public virtual void Start()
    {
        OnStart?.Invoke(-upgradeCost);
        OnTurretPlacement?.Invoke();

        bulletTimer = 0f;

        //Finds GameObject in Hierarchy with name then assign Transform as a variable
        playerBase = GameObject.Find("PlayerBase").GetComponent<Transform>();

        upgradeCostUI.text = upgradeCost.ToString();
    }

    public virtual void Update()
    {
        //Timer that goes up 1f every second
        bulletTimer += Time.deltaTime;

        //Return if timer hasn't reached turret bulletRate yet
        if (bulletTimer < bulletRate)
            return;

        EnemyClosestToBase();

        TurretShot();
    }

    private void EnemyClosestToBase()
    {
        //Creates OverlapSphere around turret the size of it's range and only detects enemies
        Collider[] enemyPos = Physics.OverlapSphere(head.position, range, enemy);

        if (enemyPos.Length == 0)
            return;

        bulletTimer = 0f;
        //Sets the distance to a number larger than the distance from playerBase to enemySpawn
        lowestDistance = 50f;

        //Runs a loop for each Collider in OverlapSphere array
        foreach (Collider enemy in enemyPos)
        {
            //Calculates the distance from enemy to playerBase
            float distance = Vector3.Distance(enemy.transform.position, playerBase.position);

            //If the distance calculatd is lower than currently assigned lowest
            if (distance < lowestDistance)
            {
                //Sets the distance of this Collider as the lowestDistance
                lowestDistance = distance;
                //Then assigns the target of the turret to this Collider
                targetEnemy = enemy.transform;
            }
        }
    }

    private void TurretShot()
    {
        //Null check for turret target
        if (targetEnemy == null)
            return;

        //Calculates the direction between turret and enemy target
        Vector3 direction = (targetEnemy.position - head.position).normalized;

        //Sets the turret to face toward enemy target
        turretBody.transform.forward = direction;

        //Spawns bullet from barrel and instantiates in an empty game object
        Instantiate(bulletType, head.position, head.rotation, transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(head.position, range);
    }
}
