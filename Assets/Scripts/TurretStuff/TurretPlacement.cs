using UnityEngine;
using UnityEngine.EventSystems;

public class TurretPlacement : MonoBehaviour, IPointerClickHandler
{
    //Game object array for possible turrets to spawn
    [SerializeField] GameObject[] turretTypes;
    //Material for material change when turret is placed
    [SerializeField] Material occupiedMat;

    private void PlaceTurret()
    {
        //Passes TurretSelected enum variable as a reference
        switch (GameManager.Instance.turretSelected)
        {
            //Order of turrets in array must be in this order to function properly
            //example: spawns Area Turret when placed at element 0 after selecting Normal Turret from UI
            case TurretSelected.NormalTurret:
                //Spawns selected turret on the platform clicked
                Instantiate(turretTypes[0], transform.position, Quaternion.identity);
                //Changes material of spawn position
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

    //Runs function on game object with this script when clicking it's collider
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaceTurret();
    }
}
