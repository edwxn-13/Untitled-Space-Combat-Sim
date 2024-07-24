using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLockHUD : MonoBehaviour
{

    [SerializeField] WeaponComputer comp;
    [SerializeField] Camera cam;
    [SerializeField] RectTransform rect_target;


    int targetID;
    // Start is called before the first frame update
    void Awake()
    {
        rect_target = GetComponentInChildren<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (comp.get_target_id() < 0)
        {
            rect_target.transform.localPosition = new Vector2(0, 0);
            rect_target.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        }

        else
        {
            Vector3 towards = FactionSys.combatant_manifest[comp.get_target_id()].transform.position;
            Vector3 from = Camera.main.transform.position;
            Vector3 dirt = (towards - from);

            Vector3 targetPositionScreen = Camera.main.WorldToScreenPoint(FactionSys.combatant_manifest[comp.get_target_id()].transform.position);
            bool offscreen = targetPositionScreen.x <= 0 || targetPositionScreen.x >= Screen.width || targetPositionScreen.y <= 0 || targetPositionScreen.y >= Screen.height;

            int borer = 100;
            if (offscreen)
            {
                Vector3 clampedTargetScreenPos = targetPositionScreen;
                if (clampedTargetScreenPos.x <= borer) clampedTargetScreenPos.x = borer;
                if (clampedTargetScreenPos.x >= Screen.width - borer) clampedTargetScreenPos.x = Screen.width - borer;
                if (clampedTargetScreenPos.y <= borer) clampedTargetScreenPos.y = borer;
                if (clampedTargetScreenPos.y >= Screen.height - borer) clampedTargetScreenPos.y = Screen.height - borer;

                Vector3 pointerPos = cam.ScreenToWorldPoint(clampedTargetScreenPos);
                rect_target.transform.position = pointerPos;
                rect_target.transform.localPosition = new Vector3(rect_target.localPosition.x, rect_target.localPosition.y, 0f).normalized * 100;
                rect_target.transform.localScale = new Vector2(0.3f, 0.3f);
                

            }
            else
            {
                Vector3 pointerPos = cam.ScreenToWorldPoint(targetPositionScreen);
                rect_target.transform.position = pointerPos;
                rect_target.transform.localPosition = new Vector3(rect_target.localPosition.x, rect_target.localPosition.y,
                rect_target.localPosition.z);
                rect_target.transform.localScale = new Vector3(1 * dirt.magnitude / 250, 1 * dirt.magnitude / 250, 1);
                rect_target.Rotate(0, 0, 50 * Time.deltaTime);

            }
        }
    }
}
