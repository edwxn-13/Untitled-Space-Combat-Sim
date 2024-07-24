using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompWeaponComputer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AutomaticWeapon[] priamry_weapons;
    [SerializeField] MissileWeapon[] secondary_weapons;
    [SerializeField] Camera cam;

    FactionSys faction_computer;

    Vector2 rotationVector = Vector2.zero;

    Transform target_trans;

    bool locked = false;

    int selected_priamry_weapon = 0;
    int selected_secondary_weapons = 0;


    float secondary_fire_lock = 0f;


    void Awake()
    {
        faction_computer = GetComponent<FactionSys>();
    }

    void AimController()
    {
        rotationVector.x += 0;
        rotationVector.y += 0;

        DampAim();
    }

 
    void DampAim()
    {
        rotationVector -= rotationVector * 0.01f;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AimController();
        Cooldown();

    }


    public Transform TargetLock()
    {
        float distance = 9999999f;
        for (int i = 0; i < FactionSys.combatant_manifest.Count; i++)
        {
            if (FactionSys.combatant_manifest[i].getFactionID() == faction_computer.getFactionID()) { continue;  }
            if (!FactionSys.combatant_manifest[i].isActiveAndEnabled) { continue; }

            Vector3 direction_vector = transform.position - FactionSys.combatant_manifest[i].transform.position;
            if (direction_vector.magnitude < distance) { distance = direction_vector.magnitude; target_trans = FactionSys.combatant_manifest[i].transform; }

        }
        if (target_trans == null) { target_trans = FactionSys.combatant_manifest[0].transform;  }
        return target_trans;
    }


    public void FireMissile()
    {
        if (secondary_fire_lock < 0)
        {
            secondary_weapons[0].DumbFire(target_trans.position);
            secondary_fire_lock = secondary_weapons[0].fire_rate;
        }
    }


    public void Cooldown()
    {
        secondary_fire_lock -= Time.deltaTime;
    }

    public void FireGun()
    {
        for (int i = 0; i < priamry_weapons.Length; i++)
        {
            priamry_weapons[i].Fire(target_trans.position);
        }

    }
}
