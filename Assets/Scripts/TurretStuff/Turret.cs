using System;
using TMPro;
using UnityEngine;

//Abstract parent class of child turret scripts
public abstract class Turret : MonoBehaviour
{
    //Variables for reading enemy to target (+ UI text)
    public TMP_Text upgradeCostUI;
    [SerializeField] Transform turretBody;
    [SerializeField] LayerMask enemy;
    private Transform playerBase;
    private float lowestDistance;
    private Transform targetEnemy;
    //Material for material change when Delete Select is active or not
    [SerializeField] GameObject objChangeMat;
    [SerializeField] Material normalMat;
    [SerializeField] Material deleteMat;

    //Variables that determine how to turret behaves
    [Header("Turret Characteristic")]
    public float bulletRate;
    private float bulletTimer;
    public int damage;
    public float range;
    public float speed;
    public int upgradeCost;

    //Variables for spawning a bullet
    [Header("Generate Bullet Elements")]
    [SerializeField] private GameObject bulletType;
    [SerializeField] private Transform head;

    public static event Action<int> OnStart;
    public static event Action OnTurretPlacement;

    //Delegate so children classes can call it as opposed to event Action
    public delegate void OnUpgradeTurret(int cost);
    public static OnUpgradeTurret onTurretClicked;

    public delegate void OnTurretDestroyed();
    public static OnTurretDestroyed onTurretDestroyed;

    public void OnEnable()
    {
        GameManager.OnTurretSelected += ChangePositionMaterial;
    }

    public void OnDisable()
    {
        GameManager.OnTurretSelected -= ChangePositionMaterial;
    }

    public virtual void Start()
    {
        //Adds 1 to total turrets placed in GameManager
        OnTurretPlacement?.Invoke();
        //When spawned, deducts money from GameManager relative to it's placement cost (aka first upgradeCost)
        OnStart?.Invoke(-upgradeCost);

        //Sets timer to 0
        bulletTimer = 0f;

        //Finds GameObject in Hierarchy with name then assign it's Transform to a variable
        playerBase = GameObject.Find("PlayerBase").GetComponent<Transform>();

        //Sets upgradeCost as a text on top of turret
        upgradeCostUI.text = upgradeCost.ToString();
    }

    public virtual void Update()
    {
        //Timer that goes up 1f every second
        bulletTimer += Time.deltaTime;

        //Shoots a bullet only when timer reaches or exceeds bulletRate
        if (bulletTimer <= bulletRate)
            return;

        EnemyClosestToBase();

        TurretShot();
    }

    private void EnemyClosestToBase()
    {
        //Before getting enemy target, resets targetEnemy to null
        targetEnemy = null;

        //Creates OverlapSphere around turret the size of it's range and only detects enemies
        Collider[] enemyPos = Physics.OverlapSphere(head.position, range, enemy);

        //If the overlap sphere doesn't detect enemies, return function
        //Allows there to be no delay to turret shot when an enemy enters Overlap after turret being inactive
        if (enemyPos.Length == 0)
            return;

        //Sets timer back to 0
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
                //Then assigns the target of the turret to this enemy
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

        //Spawns bullet at turret head
        Instantiate(bulletType, head.position, head.rotation, transform);
    }

    private void ChangePositionMaterial()
    {
        //Changes material only when Delete Select is active or deactivated
        if (GameManager.Instance.turretSelected == TurretSelected.DeleteTurret)
        {
            objChangeMat.GetComponent<MeshRenderer>().material = deleteMat;
        }
        else
        {
            objChangeMat.GetComponent<MeshRenderer>().material = normalMat;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(head.position, range);
    }
}
