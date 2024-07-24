using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterBehaviou : MonoBehaviour
{
    TrailRenderer t_renderer;

    void Awake()
    {
        t_renderer = GetComponent<TrailRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ThrustSwitch(bool state)
    {
        if (state == true)
        {
            t_renderer.enabled = true;
        }

        else
        {
            t_renderer.enabled = false;
        }
    } 
}
