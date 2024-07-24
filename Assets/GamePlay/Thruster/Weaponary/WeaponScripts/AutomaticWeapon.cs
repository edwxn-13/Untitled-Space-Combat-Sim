using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Bullet round_type;

    [SerializeField] float fire_rate = 0.19f;

    FactionSys sys;
    float def_fire_rate;

    float temp_rate = 0;

    [SerializeField] List<AudioClip> laserSounds;
    AudioSource gunSource;

    float overheat_max = 100f;
    float overheat_level = 0;
    float overhheat_rate = 15f;
    bool isHeated = false;

    void Start() 
    {
        def_fire_rate = fire_rate;
        temp_rate = fire_rate;
        gunSource = GetComponent<AudioSource>();
        sys = GetComponentInParent<FactionSys>();
    }

    public float OverHeatRatio()
    {
        return overheat_level / overheat_max;
    }
    // Update is called once per frame
    void Update()
    {
        RadiatorFunction();
    }


    void RadiatorFunction()
    {
        overheat_level -= overhheat_rate * 0.7f * Time.deltaTime;

        if (overheat_level < 0)
        {
            overheat_level = 0;            
        }

        if (overheat_level < overheat_max / 2)
        {
            fire_rate = def_fire_rate;
            isHeated = false;
        }
        
    }

    public void Fire(Vector3 target_pos)
    {
        temp_rate -= Time.deltaTime;
        if (overheat_level > overheat_max)
        {
            isHeated = true;
        }
       
        if (!isHeated)
        {
            if (temp_rate < 0)
            {
                round_type.ShotPoint = target_pos;
                Bullet temp = Instantiate(round_type, transform.position, transform.rotation);
                temp.setOwner(sys.gameObject);
                temp.ShotPoint = target_pos;
                temp_rate = fire_rate;
                gunSource.PlayOneShot(laserSounds[Random.Range(0,laserSounds.Count-1)]);
            }
            overheat_level += overhheat_rate * Time.deltaTime * 1.8f;
        }

        
    }
}
