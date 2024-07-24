using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHUD : MonoBehaviour
{

    public RectTransform rect;
    public Image img;

    public static List<ObjectiveHUD> marker_manifest = new List<ObjectiveHUD>() { };

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        marker_manifest.Add(this);

    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
