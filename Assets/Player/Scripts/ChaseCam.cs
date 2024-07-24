using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class ChaseCam : MonoBehaviour
{
     Transform local_transform;

    [SerializeField] Transform player_transform;
    [SerializeField] Transform player;
    [SerializeField] Vector3 camera_position = new Vector3(0f, 2f, -10f);
    [SerializeField] float damp = 10.0f;
    [SerializeField] float r_damp = 10.0f;
    [SerializeField] float look_rate = 5.0f;
    Quaternion stock_rotate = Quaternion.identity;

    Vector2 rotationVector = Vector2.zero;


    public Vector3 velocity = Vector3.one;


    void Awake()
    {
        local_transform = transform;
    }

    void Start()
    {
       //stock_rotate = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        LookTurn();
    }
    void FixedUpdate()
    {
        chase_cam();
        //, r_damp* Time.deltaTime
    }


    void follow_cam()
    {
        Vector3 update_positon = player_transform.position + (player_transform.rotation * camera_position);
        Vector3 current_pos = Vector3.Lerp(local_transform.position, update_positon, damp * Time.deltaTime);
        local_transform.position = current_pos;

        Quaternion towards = Quaternion.LookRotation(player_transform.position - local_transform.position, local_transform.up);
        Quaternion current_rotation = Quaternion.Slerp(local_transform.rotation, towards, r_damp * Time.deltaTime);
        
        local_transform.rotation = current_rotation;
    }


    void chase_cam()
    {
        //Vector3 update_positon = player_transform.position + (player_transform.rotation * camera_position);
        //Vector3 current_pos = Vector3.Lerp(local_transform.position, update_positon,  1);
        //local_transform.position = current_pos;
        if (Input.GetButton("look"))
        {
            TurnController();

            local_transform.Rotate(-(Input.GetAxis("Mouse Y") * Time.deltaTime * look_rate), Input.GetAxis("Mouse X") * Time.deltaTime * look_rate, 0);
            /*float x = Input.GetAxis("Mouse X") * Time.deltaTime * look_rate;
            float y = Input.GetAxis("Mouse Y") * Time.deltaTime * look_rate;
            Vector3 rot = transform.rotation.eulerAngles + new Vector3(-y, x, 0f); //use local if your char is not always oriented Vector3.up
            //rot.x = Mathf.Clamp(rot.x, -60f, 60f);
            rot.x = Mathf.Clamp(rot.y, -80f, 80f);
            local_transform.eulerAngles = rot;*/
        }
        else
        {
            //local_transform.LookAt(player_transform, player_transform.up);
            //Quaternion towards = Quaternion.LookRotation(player_transform.position - local_transform.position, local_transform.up);
            //Quaternion current_rotation = Quaternion.Slerp(local_transform.rotation, towards, r_damp * Time.deltaTime);
            local_transform.rotation = Quaternion.Lerp(local_transform.rotation, player.rotation, r_damp);
            
            //local_transform.rotation = player.rotation;
        }


    }

    void TurnController()
    {
        rotationVector.x += Input.GetAxis("Mouse X") * Time.deltaTime * look_rate;
        rotationVector.y += Input.GetAxis("Mouse Y") * Time.deltaTime * look_rate;

    }

    void LookTurn()
    {
        
    }
}
