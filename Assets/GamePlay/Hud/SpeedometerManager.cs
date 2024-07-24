using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerManager : MonoBehaviour
{
    [SerializeField] Rigidbody user_rigid;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "[" + (int) (user_rigid.velocity.magnitude * 4) + "]m/s";
    }
}
