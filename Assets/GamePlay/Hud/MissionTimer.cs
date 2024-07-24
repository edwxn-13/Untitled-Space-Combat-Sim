using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionTimer : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    SceneController c_scene;
    void Start()
    {
        text = GetComponent<Text>();
        c_scene = FindFirstObjectByType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Time Remaining - " + c_scene.get_time()/60;
    }
}
