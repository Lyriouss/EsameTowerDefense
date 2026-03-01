using UnityEngine;
using UnityEngine.EventSystems;

public class AreaTurret : Turret, IPointerClickHandler
{
    //Explosion area and upgrade value of bullet
    public float explosionArea;
    [SerializeField] private float areaUpgrade;

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

    private void Upgrade()
    {
        //Runs only when current money is higher or equal to upgradeCost and hasn't reached max upgrade
        if (GameManager.Instance.currentMoney < upgradeCost || upgradeCount >= 5)
            return;

        //Highers the explosion area of bullet equal to the areaUpgrade
        explosionArea += areaUpgrade;

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
        //else when reaching 5, changes upgrade cost text to "MAX"
        else
            upgradeCostUI.text = "MAX";
    }

    //Runs function on game object with this script when clicking it's collider
    public void OnPointerClick(PointerEventData eventData)
    {
        Upgrade();
    }
}
