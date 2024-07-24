using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerScript : MonoBehaviour
{

    [SerializeField] Camera cam;
    int current_size = 0;
    int active_size = 0;
    List<ObjectiveHUD> markers;
    List<FactionSys> active_enemies;
    List<int> indecies;


    [SerializeField] ObjectiveHUD hud_prefab;
    [SerializeField] Sprite target_sprite;
    [SerializeField] Sprite off_screen_sprite;


    // Start is called before the first frame update
    void Start()
    {
        //RefreshEntityList();
    }


    private void Awake()
    {


    }

    void createMarker()
    {
        ObjectiveHUD temp = Instantiate(hud_prefab, transform);
        markers.Add(temp); 
        temp.img.enabled = true;
       
        print("added");


    }
    void RefreshEntityList()
    {

        print("we updated");

        if (active_size > 0)
        {
            for (int i = 0; i < active_size; i++)
            {
                markers[i].img.enabled = false;
                Destroy(markers[i].GetComponentInChildren<Text>());
                Destroy(markers[i].img);
                Destroy(markers[i]);

            }

            //markers.RemoveRange(0, markers.Count);

        }

        active_size = 0;


        markers = new List<ObjectiveHUD>();
        active_enemies = new List<FactionSys>();
        indecies = new List<int>();

        current_size = FactionSys.combatant_manifest.Count;

        for (int i = 0; i < FactionSys.combatant_manifest.Count; i++)
        {
            if (FactionSys.combatant_manifest[i].player_controll()) { continue; }

            if (FactionSys.combatant_manifest[i].getFactionID() == 0) { continue; }

            if (FactionSys.combatant_manifest[i].isActiveAndEnabled)
            {
                active_size++;
                indecies.Add(i);
                createMarker();
                active_enemies.Add(FactionSys.combatant_manifest[i]);
            }
            //eqeewwweqwemarkers.Add(ObjectiveHUD.marker_manifest[i]);
        }

    }

    void checkActiveEntities()
    {
        int tempActive = 0;
        for (int i = 0; i < FactionSys.combatant_manifest.Count; i++)
        {
            if (FactionSys.combatant_manifest[i].player_controll()) { continue; }

            if (FactionSys.combatant_manifest[i].getFactionID() == 0) { continue; }

            if (FactionSys.combatant_manifest[i].isActiveAndEnabled)
            {
                tempActive++;
            }
        }

        if (active_size != tempActive) { RefreshEntityList(); }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        checkActiveEntities();
       
        for (int i = 0; i < active_size; i++)
        {
            FactionSys current = FactionSys.combatant_manifest[indecies[i]]; 
            Vector3 towards = FactionSys.combatant_manifest[indecies[i]].transform.position;
            Vector3 from = Camera.main.transform.position;
            Vector3 dir = (towards - from).normalized;
            Vector3 dirt = (towards - from);
            Vector3 targetPositionScreen = Camera.main.WorldToScreenPoint(FactionSys.combatant_manifest[indecies[i]].transform.position);

            Text text = markers[i].GetComponentInChildren<Text>();

            text.text = (active_enemies[i].unit_name + "\nShields:" + active_enemies[i].local_shields.get_shield());
            bool offscreen = targetPositionScreen.x <= 0 || targetPositionScreen.x >= Screen.width || targetPositionScreen.y <= 0 || targetPositionScreen.y >= Screen.height;
            float borer = 100;
            //bool offscreen = false;
            if (offscreen)
            {
                Vector3 clampedTargetScreenPos = targetPositionScreen;
                if (clampedTargetScreenPos.x <= borer) clampedTargetScreenPos.x = borer;
                if (clampedTargetScreenPos.x >= Screen.width - borer) clampedTargetScreenPos.x = Screen.width - borer;
                if (clampedTargetScreenPos.y <= borer) clampedTargetScreenPos.y = borer;
                if (clampedTargetScreenPos.y >= Screen.height - borer) clampedTargetScreenPos.y = Screen.height - borer;

                Vector3 pointerPos = cam.ScreenToWorldPoint(clampedTargetScreenPos);
                markers[i].rect.transform.position = pointerPos;
                markers[i].rect.transform.localPosition = new Vector3(markers[i].rect.localPosition.x, markers[i].rect.localPosition.y,0f).normalized * 40;
                markers[i].img.sprite = off_screen_sprite;
                markers[i].img.transform.localScale = new Vector2(0.3f, 0.3f);

            }
            else
            {
                Vector3 pointerPos = cam.ScreenToWorldPoint(targetPositionScreen);
                /*
                markers[i].rect.transform.position = pointerPos;
                markers[i].rect.transform.localPosition = new Vector3(
                    markers[i].rect.localPosition.x, markers[i].rect.localPosition.y,
                    markers[i].rect.localPosition.z);
                markers[i].rect.localScale = new Vector3(1 * dirt.magnitude / 250, 1 * dirt.magnitude / 250, 1);
                markers[i].img.transform.localScale = new Vector3(1 * dirt.magnitude/250, 1 * dirt.magnitude/250, 1);*/

                
                markers[i].img.sprite = target_sprite;
                markers[i].transform.position = pointerPos;
                markers[i].transform.localPosition = new Vector3(markers[i].rect.localPosition.x, markers[i].rect.localPosition.y,
                    markers[i].rect.localPosition.z);
                markers[i].transform.localScale = new Vector3(1 * dirt.magnitude / 250, 1 * dirt.magnitude / 250, 1);

            }
        }

    }
}
