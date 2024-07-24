using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Transform local_transform;
    [SerializeField] public float dmg = 15;
    GameObject owner;
    public bool tracking;
    public int target_index;
    Vector3 attack_vector = Vector3.zero;
    public Vector3 ShotPoint = Vector3.zero;
    [SerializeField] float bullet_speed = 100f;
    [SerializeField] float duration = 100f;
    [SerializeField] bool isMissile;

    SceneController soundSource;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void setOwner(GameObject own)
    {
        owner = own;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.GetInstanceID() == owner.GetInstanceID())
        {
            return;
        }
        Expired();
    }
    void Awake()
    {
        local_transform = transform;
        if(tracking == false)
            transform.LookAt(ShotPoint);

        soundSource = FindFirstObjectByType<SceneController>();

    }

    // Update is called once per frame

    void FixedUpdate()
    {
        BulletMechanics();
        local_transform.position += local_transform.forward * Time.deltaTime * bullet_speed;
        duration -= Time.deltaTime;
        if (duration < 0)
        {
            Expired();
        }

        if (tracking) { //if (attack_vector.magnitude < 0.02f)
            { ManouverTowards(); } }
    }

    void ManouverTowards()
    {
        attack_vector = FactionSys.combatant_manifest[target_index].transform.position - transform.position;
        Quaternion look_at_target = Quaternion.LookRotation(attack_vector);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, look_at_target, 900 * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, look_at_target, 30 * Time.deltaTime);
        //print(FactionSys.combatant_manifest[target_index].transform.position);


    }

    void Expired()
    {
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision cols)
    {
        if (cols.transform == transform) { return; }
        Shields shield;

        if (cols.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID()) { return; }

        if (cols.transform.gameObject.TryGetComponent<Shields>(out shield))
        {
            shield.Damage(dmg);
            soundSource.ShieldImpact(transform.position);
        }
        else
        {
            soundSource.BulletImpact(transform.position);
        }

        if (isMissile)
        {
            soundSource.Explosion(transform.position);
        }

        Destroy(gameObject);
    }

    void BulletMechanics()
    {
        RaycastHit hit;
        Ray forward = new Ray(transform.position, transform.forward);

        // Cast a ray straight downwards.
        if (Physics.Raycast(forward, out hit, 5))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID()) { return; }
            if (hit.transform == transform) { return; }
            Shields shield;

            if (hit.transform.gameObject.TryGetComponent<Shields>(out shield))
            {
                shield.Damage(dmg);
                soundSource.ShieldImpact(transform.position);
            }

            else
            {
                soundSource.BulletImpact(transform.position);
            }

            if (isMissile)
            {
                soundSource.Explosion(transform.position);
            }

            Destroy(gameObject);

        }
    }
}
