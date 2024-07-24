using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverheatMeter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AutomaticWeapon weapon;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = weapon.OverHeatRatio(); 
    }
}
