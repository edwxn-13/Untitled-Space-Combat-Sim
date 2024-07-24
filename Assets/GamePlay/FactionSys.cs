using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSys : MonoBehaviour
{

    [SerializeField] int FactionnID;
    [SerializeField] bool player_controlled;

    [SerializeField] public string unit_name = "SV00-1";

    public bool targeted;

    public DamageEngine local_damage;
    public Shields local_shields;
    public static List<FactionSys> combatant_manifest = new List<FactionSys>() { };

    // Start is called before the first frame update
    void Awake()
    {
        local_damage = GetComponent<DamageEngine>();
        local_shields = GetComponent<Shields>();
    }

    void Start()
    {
        combatant_manifest.Add(this);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public int getFactionID()
    {
        return FactionnID;
    }

    public bool player_controll()
    {
        return player_controlled;
    }
}
