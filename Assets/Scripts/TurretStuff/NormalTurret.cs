using System;
using UnityEngine.EventSystems;

public class NormalTurret : Turret, IPointerClickHandler
{
    private int upgradeCount;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    private void Upgrade()
    {
        if (GameManager.Instance.currentMoney < upgradeCost || upgradeCount >= 5)
            return;

        damage *= 2;

        onUpgradeTurret?.Invoke(-upgradeCost); 

        upgradeCost *= 2;
        upgradeCount++;
        if (upgradeCount < 5)
            upgradeCostUI.text = upgradeCost.ToString();
        else
            upgradeCostUI.text = "MAX";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Upgrade();
    }
}
