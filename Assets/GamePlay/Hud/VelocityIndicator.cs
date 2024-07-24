using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityIndicator : MonoBehaviour
{
    [SerializeField] ShipController controller;
    [SerializeField] Camera cam;
    RectTransform rect_target;
    Image img;

    [SerializeField] Sprite retrograde;
    [SerializeField] Sprite prograde;

    // Start is called before the first frame update
    void Awake()
    {
        rect_target = GetComponentInChildren<RectTransform>();
        img = GetComponentInChildren<Image>();
        img.preserveAspect = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 towards = controller.transform.position + controller.getVelocity();
        Vector3 from = Camera.main.transform.position;
        Vector3 dirt = (towards - from);

        Vector3 targetPositionScreen = Camera.main.WorldToScreenPoint(towards);
        bool offscreen = targetPositionScreen.x <= 0 || targetPositionScreen.x >= Screen.width || targetPositionScreen.y <= 0 || targetPositionScreen.y >= Screen.height;

        if (offscreen)
        {
            
            Vector3 clampedTargetScreenPos = targetPositionScreen;
            if (clampedTargetScreenPos.x <= 0) clampedTargetScreenPos.x = Screen.width + clampedTargetScreenPos.x ;
            if (clampedTargetScreenPos.x >= Screen.width) clampedTargetScreenPos.x = clampedTargetScreenPos.x - Screen.width;
            if (clampedTargetScreenPos.y <= 0) clampedTargetScreenPos.y = Screen.height + clampedTargetScreenPos.y ;
            if (clampedTargetScreenPos.y >= Screen.height) clampedTargetScreenPos.y = clampedTargetScreenPos.y - Screen.height;
            Vector3 pointerPos = cam.ScreenToWorldPoint(clampedTargetScreenPos);
            pointerPos.z = -pointerPos.z;
            rect_target.transform.position = pointerPos;
            rect_target.transform.localPosition = new Vector3(rect_target.localPosition.x, rect_target.localPosition.y,
            -rect_target.localPosition.z);
            rect_target.transform.localScale = new Vector3(1 * dirt.magnitude / 40, 1 * dirt.magnitude / 40, 1);
            img.sprite = retrograde;

            //print(clampedTargetScreenPos);
            

        }
        else
        {
            Vector3 pointerPos = cam.ScreenToWorldPoint(targetPositionScreen);
            rect_target.transform.position = pointerPos;
            rect_target.transform.localPosition = new Vector3(rect_target.localPosition.x, rect_target.localPosition.y,
            rect_target.localPosition.z);
            rect_target.transform.localScale = new Vector3(1 * dirt.magnitude/10 , 1 * dirt.magnitude/10 , 1);

            img.sprite = prograde;

        }
    }
}
