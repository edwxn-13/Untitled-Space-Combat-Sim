using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Bullet round_type;

    FactionSys sys;
    public float fire_rate = 5f;

    void Start()
    {
        sys = GetComponentInParent<FactionSys>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DumbFire(Vector2 target_pos)
    {
        round_type.tracking = false;
        Bullet temp = Instantiate(round_type, transform.position, transform.rotation);
        temp.setOwner(sys.gameObject);
        temp.tracking = false;
    }


    public void LockFire(int targetID)
    {
        round_type.target_index = targetID;
        round_type.tracking = true;
        Bullet temp = Instantiate(round_type, transform.position, transform.rotation);
        temp.setOwner(sys.gameObject);
        temp.target_index = targetID;
        temp.tracking = true;
    }
}
