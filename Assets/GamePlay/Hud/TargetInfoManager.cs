using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TargetInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    UnityEngine.UI.Text text;
    [SerializeField] WeaponComputer comp;
    void Start()
    {
        text = GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (comp.get_target_id() < 0)
        {
            text.text = "no target";
        }

        else
        {
            DamageEngine eng = FactionSys.combatant_manifest[comp.get_target_id()].GetComponent<DamageEngine>();
            Shields shield = FactionSys.combatant_manifest[comp.get_target_id()].GetComponent<Shields>();

            text.text = FactionSys.combatant_manifest[comp.get_target_id()].unit_name + "\n" + "+" + (int)shield.get_shield() + "\n" + "-" + (int)(eng.ArmourRatio()*100) + "%";

        }
    }
}
