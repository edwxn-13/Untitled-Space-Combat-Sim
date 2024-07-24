using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] int spawnTeam;
    [SerializeField] CompController spawnFab;
    [SerializeField] int threshold;
    [SerializeField] float timer_length;
    bool canSpawn = false;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        checkSpawnStatus();
    }

    void Timer()
    {
        if (timer <= 0)
        {
            if (canSpawn) { SpawnComp(); }
            timer = timer_length;
        }

        timer -= Time.deltaTime;
    }


    void checkSpawnStatus()
    {
        int temp_count = 0;
        for (int i = 0; i < FactionSys.combatant_manifest.Count; i++)
        {
            if (FactionSys.combatant_manifest[i].getFactionID() != spawnTeam) { continue; }
            if (FactionSys.combatant_manifest[i].player_controll()) { continue; }
            if (FactionSys.combatant_manifest[i].isActiveAndEnabled) { temp_count++; }

            if (temp_count < threshold) { canSpawn = true; }
            else { canSpawn = false; }
        }
    }

    void SpawnComp()
    {
        Instantiate(spawnFab, transform.position, transform.rotation);
    }
}
