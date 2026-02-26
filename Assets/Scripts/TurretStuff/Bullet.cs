using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 startPos;
    private Vector3 range;

    private float t;

    private void OnEnable()
    {
        //Turret.onBulletSpawn += SetRange();
    }

    private void OnDisable()
    {
        //Turret.onBulletSpawn -=;
    }

    private void Start()
    {
       startPos = transform.position;
    }

    private void Update()
    {
      //  transform.position = new Vector3(Mathf.Lerp(startPos.x, range.position.x, t), 0, 0);
    }

    private void SetRange(Vector3 setRange)
    {
        range = setRange;
    }
}
