using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabSpeakerManager : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    [SerializeField] List<AudioClip> clips;
    Shields shield;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        shield = GetComponentInParent<Shields>();
    }

    // Update is called once per frame
    void Update()
    {
        ui_sound();
        lowShields();
    }

    void ui_sound()
    {
        if(Input.GetButtonDown("lock"))
            source.PlayOneShot(clips[0]);

        if (Input.GetButtonDown("unlock"))
            source.PlayOneShot(clips[1]);
    }

    void lowShields()
    {
        if (shield.ShieldRatio() * 100 < 20f)
        {
            source.clip = clips[2];
            source.Play();
        }
    }


}
