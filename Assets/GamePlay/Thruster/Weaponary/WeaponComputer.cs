using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComputer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AutomaticWeapon [] priamry_weapons;
    [SerializeField] MissileWeapon [] secondary_weapons;
    [SerializeField] Camera cam;

    Vector2 rotationVector = Vector2.zero;
    Vector3 aim_angle = Vector3.zero;


    FactionSys faction_sys;
    Transform target_trans;


    int selected_priamry_weapon = 0;
    int selected_secondary_weapons = 0;

    bool locked;

    int targetID = -1;

    float secondary_fire_lock = 0f;


    void TargetingComputer()
    {
        TargetUnlcok();
        if (Input.GetButtonDown("lock"))
            target_trans = targetAq();
    }


    void LockCalc()
    {
        if (targetID > -1)
        {
            Vector3 attackVector = FactionSys.combatant_manifest[targetID].transform.position - transform.position;
            float attack_angle = Vector3.Angle(transform.forward, attackVector);
            if (attack_angle < 10)
            {
                locked = true;
            }

            else { locked = false; }
        }

        else
        {
            locked = false;
        }


    }



    Transform targetAq()
    {
        targetID++;

        if (targetID > FactionSys.combatant_manifest.Count - 1) { targetID = 0; }
        Transform target;


        while (FactionSys.combatant_manifest[targetID].player_controll())
        {
            targetID++;
            if (targetID > FactionSys.combatant_manifest.Count - 1) { targetID = 0; }

        }

        while (FactionSys.combatant_manifest[targetID].isActiveAndEnabled == false)
        {
            targetID++;
            if (targetID > FactionSys.combatant_manifest.Count - 1) { targetID = -1; return this.transform; }

        }


        while (FactionSys.combatant_manifest[targetID].getFactionID() == faction_sys.getFactionID())
        {
            targetID++;
            if (targetID > FactionSys.combatant_manifest.Count - 1) { targetID = 0; }

        }

        if (targetID > FactionSys.combatant_manifest.Count - 1) { targetID = 0; }

        target = FactionSys.combatant_manifest[targetID].transform;
        FactionSys.combatant_manifest[targetID].targeted = true;

        return target;
    }

    void TargetUnlcok()
    {
        if (targetID > -1)
        {
            FactionSys.combatant_manifest[targetID].targeted = false;
        }
        if(Input.GetButtonDown("unlock"))
            targetID = -1;
    }

    void AimController()
    {
        rotationVector.x += Input.GetAxis("Mouse X") * Time.deltaTime;
        rotationVector.y += Input.GetAxis("Mouse Y") * Time.deltaTime;

        DampAim();

        RaycastHit hit;

        Ray ray = cam.ScreenPointToRay(new Vector3((rotationVector.x * 0.2f * Screen.width) + Screen.width/2, (rotationVector.y * 0.5f * Screen.height) + Screen.height / 2, 0));
        aim_angle = ray.GetPoint(500);

        if (Physics.Raycast(ray, out hit))
        {
            target_trans = hit.transform;
        }
    }

    void DampAim()
    {
        rotationVector -= rotationVector * 0.1f;
    }

    void Start()
    {

    }


    void Awake()
    {
        faction_sys = GetComponent<FactionSys>();
    }

    // Update is called once per frame
    void Update()
    {
        AimController();
        Cooldown();
        TargetingComputer();
        LockCalc();
    }

    public void TargetLock()
    {

    }

    public int get_target_id()
    {
        return targetID;
    }

    public void FireMissile()
    {
        
        if (secondary_fire_lock < 0)
        {
            if (!locked)
            {
            
            }

            else
            {
                secondary_weapons[0].LockFire(targetID);
                secondary_fire_lock = secondary_weapons[0].fire_rate;
            }
        }
    }


    public void Cooldown()
    {
        secondary_fire_lock -= Time.deltaTime;
    }

    public void FireGun()
    {
       
        {
            for (int i = 0; i < priamry_weapons.Length; i++)
            {
                priamry_weapons[i].Fire(aim_angle);
            }
        }

    }
}
