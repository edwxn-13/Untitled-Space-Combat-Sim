using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrottleMeter : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] ShipController ship;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
       image.fillAmount = ship.throttleRatio();
    }
}
