using UnityEngine;
using UnityEngine.EventSystems;

public class TurretPlacement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject[] turretTypes;
    [SerializeField] Material occupiedMat;

    private void PlaceTurret()
    {
        switch (GameManager.Instance.turretSelected)
        {
            case TurretSelected.NormalTurret:
                Instantiate(turretTypes[0], transform.position, Quaternion.identity);
                gameObject.GetComponent<MeshRenderer>().material = occupiedMat;
                break;
            case TurretSelected.MachineGunTurret:
                Instantiate(turretTypes[1], transform.position, Quaternion.identity);
                gameObject.GetComponent<MeshRenderer>().material = occupiedMat;
                break;
            case TurretSelected.AreaTurret:
                Instantiate(turretTypes[2], transform.position, Quaternion.identity);
                gameObject.GetComponent<MeshRenderer>().material = occupiedMat;
                break;
            case TurretSelected.NoSelect:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaceTurret();
    }
}
