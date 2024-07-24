using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldMeter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Shields shields;
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
        image.fillAmount = shields.ShieldRatio();
        text.text =  shields.ShieldRatio() * 100 + "%";
    }
}
