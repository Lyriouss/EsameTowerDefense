using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineGunTurret : Turret, IPointerClickHandler
{
    [SerializeField] private float changeRate;
    [SerializeField] private float minRate;

    private int upgradeCount;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void Upgrade()
    {
        if (GameManager.Instance.currentMoney < upgradeCost || upgradeCount >= 5)
            return;

        if (bulletRate - changeRate >= minRate)
            bulletRate -= changeRate;
        else  
            bulletRate = minRate;

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
