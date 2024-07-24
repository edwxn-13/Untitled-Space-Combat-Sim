using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<AudioClip> explosion;
    [SerializeField] List<AudioClip> bulletHIt;
    [SerializeField] List<AudioClip> shieldHit;
    [SerializeField] List<AudioClip> collision;
    [SerializeField] AudioClip ship_explode;
    [SerializeField] ObjectiveBehaviour objective;
    [SerializeField] Camera deathCam;
    [SerializeField] ShipController player;
    [SerializeField] int gameMode;

    [SerializeField] float time_amount;
    [SerializeField] int next_scene = 0;

    [SerializeField] Text objectiveText;
    string objTag = "Current Objective\n- ";
    float end_state_timer = 10;
    bool gotime = false;
    bool startClock = false;

    int live_opps = 20;

    // 0 Skirmish
    // 1 defense
    // 2 offsense
    // 3 survival
    
    void Start()
    {

    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FactionSys.combatant_manifest = new List<FactionSys>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDead();
        count_opps();
        DefenseLoseCondition();
        OffenseWinCondition();
        Timer();
        setText();
        if (startClock) { EndTimer(); }
    }


    public float get_time()
    {
        return time_amount;
    }

    void setText()
    {
        if (!startClock)
        {
            if (gameMode == 0) { objectiveText.text = objTag + "They are conduction operations in the area. Destroy all enemies"; }
            if (gameMode == 2) { objectiveText.text = objTag + "The enemy is controlling a HArVeST series global warming beam, eliminate it"; }
            if (gameMode == 1) { objectiveText.text = objTag + "They are trying to take out our eco platform, defend it"; }
            if (gameMode == 3) { objectiveText.text = objTag + "Survive"; }
        }

    }
    void EndTimer()
    {
        end_state_timer -= Time.deltaTime;
        if (end_state_timer <= 0) { gotime = true; }
    }
    void Timer()
    {
        if (gameMode == 1 || gameMode == 3)
        {


            time_amount -= Time.deltaTime;
            if (time_amount <= 0)
            {
                if (gameMode == 1) { DefenseWinCondition(); }
                if (gameMode == 3) { SurvivalWinCondition(); }

            }
        }
    }

    public void count_opps()
    {
        if (gameMode != 0)
        {
           return;
        }
        int count = 0;
        for (int i = 0; i < FactionSys.combatant_manifest.Count; i++)
        {
            if (FactionSys.combatant_manifest[i].player_controll()) { continue; }
            if (FactionSys.combatant_manifest[i].getFactionID() == 0) { continue; }
            if (FactionSys.combatant_manifest[i].isActiveAndEnabled) { count++; }
            live_opps = count;
        }

        print(live_opps);
        if (live_opps == 0)
        {
            SkirmishWinCondition();
        }
    }

    public void Explosion(Vector3 position)
    {      
        AudioSource.PlayClipAtPoint(explosion[0], position);
    }


    public void BulletImpact(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(bulletHIt[Random.Range(0, bulletHIt.Count - 1)], position);
    }

    public void ShipExplosion(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(ship_explode, position);
    }


    public void ShieldImpact(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(shieldHit[Random.Range(0, shieldHit.Count - 1)], position);
    }


    public void CollisionImpact(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(collision[Random.Range(0, collision.Count - 1)], position);
    }
        
    public void playerDead()
    {
        print(player.isActiveAndEnabled);
        if (!player.isActiveAndEnabled)
        {
            startClock = true;
            deathCam.gameObject.SetActive(true);
            objectiveText.text = "SNAKE?? SNAKE?? SNAAAAAAKE!!!!!!!!";
            if (gotime)
                SceneManager.LoadSceneAsync(0);

        }
    }

    public void DefenseLoseCondition()
    {
        if (gameMode != 1) { return; }
        if (!objective.isActiveAndEnabled)
        {
            startClock = true;
            objectiveText.text = "WHAT HAVE YOU DONE, THE PLATFORM IS GONE";

            if (gotime)
                SceneManager.LoadSceneAsync(0);

        }
    }

    public void DefenseWinCondition()
    {
        startClock = true;
        objectiveText.text = "Well donw, another succesful mission";

        if (gotime)
            SceneManager.LoadSceneAsync(next_scene);
    }

    public void OffenseWinCondition()
    {
        if (gameMode != 2) { return; }
        if (!objective.isActiveAndEnabled)
        {
            startClock = true;
            objectiveText.text = "Target Destroyed, another succesful mission";

            if (gotime)
                SceneManager.LoadSceneAsync(next_scene);
        }
    }

    public void SkirmishWinCondition()
    {
        startClock = true;
        objectiveText.text = "All targets eliminated, well done";

        if (gotime)
            SceneManager.LoadSceneAsync(next_scene);
    }

    public void SurvivalWinCondition()
    {
        startClock = true;
        objectiveText.text = "Noble 6 could never do that";

        if (gotime)
            SceneManager.LoadSceneAsync(next_scene);
    }
}
