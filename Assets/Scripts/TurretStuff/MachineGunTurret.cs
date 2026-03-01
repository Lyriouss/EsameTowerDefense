using UnityEngine;
using UnityEngine.EventSystems;

public class MachineGunTurret : Turret, IPointerClickHandler
{
    //Rate changed to bulletRate and min value it can reach
    [Header("Upgrade Values")]
    [SerializeField] private float changeRate;
    [SerializeField] private float minRate;
    [SerializeField] private int damageIncrease;

    private int upgradeCount;

    //Runs Start() of parent class
    public override void Start()
    {
        base.Start();
    }

    //Runs Update() of parent class
    public override void Update()
    {
        base.Update();
    }

    public void Upgrade()
    {
        //Clicking on turret destroys it when Delete button is selected in UI
        if (GameManager.Instance.turretSelected == TurretSelected.DeleteTurret)
        {
            Destroy(gameObject);
        }

        //Runs only when current money is higher or equal to upgradeCost and hasn't reached max upgrade
        if (GameManager.Instance.currentMoney < upgradeCost || upgradeCount >= 5)
            return;

        //If bulletRate after subtraction doesn't go lower than minRate
        if (bulletRate - changeRate >= minRate)
            //Lowers bulletRate equal to changeRate
            bulletRate -= changeRate;
        //Else when going lower, makes bulletRate equal to minRate
        else  
            bulletRate = minRate;

        //Increased damage
        damage += damageIncrease;

        //Deducts money equal to upgradeCost from GameManager
        onUpgradeTurret?.Invoke(-upgradeCost);

        //Doubles the upgradeCost of turret
        upgradeCost *= 2;
        //Adds 1 to upgradeCount
        upgradeCount++;

        //If the upgradeCount hasn't reached 5
        if (upgradeCount < 5)
            //Updates the next upgrade cost text
            upgradeCostUI.text = upgradeCost.ToString();
        //Else when reaching 5, changes upgrade cost text to "MAX"
        else
            upgradeCostUI.text = "MAX";
    }

    //Lowers turret count by 1 when destroyed
    private void OnDestroy()
    {
        onTurretDestroyed?.Invoke(-1);
    }

    //Runs function on game object with this script when clicking it's collider
    public void OnPointerClick(PointerEventData eventData)
    {
        Upgrade();
    }
}
