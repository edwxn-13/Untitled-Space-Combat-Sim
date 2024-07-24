using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHealthHUD : MonoBehaviour
{
    ObjectiveBehaviour objective;
    DamageEngine dmg;
    FactionSys sys;

    Image image;

    Text text;
    void Start()
    {
        objective = FindFirstObjectByType<ObjectiveBehaviour>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        sys = objective.GetComponent<FactionSys>();
        dmg = objective.GetComponent<DamageEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = dmg.ArmourRatio();
        text.text = sys.unit_name;
    }
}
