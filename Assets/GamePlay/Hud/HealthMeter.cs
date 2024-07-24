using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] DamageEngine armour;
    Image image;

    Text text;
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = armour.ArmourRatio();
        text.text = armour.ArmourRatio() * 100 + "%";
    }
}
