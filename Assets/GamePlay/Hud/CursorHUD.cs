using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHUD : MonoBehaviour
{

    [SerializeField] ShipController player;
    [SerializeField] RectTransform rect;
    [SerializeField] int target_size = 140;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rect.localPosition = player.rotationVector * target_size;
    }
}
