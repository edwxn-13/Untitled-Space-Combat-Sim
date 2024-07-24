using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float movevment_speed = 100.0f;
    [SerializeField] float turn_speed = 40.0f;
    [SerializeField] float look_rate = 5.0f;

    [SerializeField] AudioClip thrust_audio;
    [SerializeField] AudioSource ThrusterAudio;

    [SerializeField] SceneController scene;
    Vector3 previous_mouse_position = Vector3.zero;
    public Vector2 rotationVector = Vector2.zero;
    public Vector3 d_rotationVector = Vector3.zero;

    Vector2 screen_offset = Vector2.zero;
    Transform local_transform;

    float throttle = 0.0f;
    bool isAlive = true; 
    WeaponComputer weapon_comp;

    Rigidbody rigidbody;

    void Awake()
    {
        local_transform = transform;
        weapon_comp = GetComponent<WeaponComputer>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        screen_offset.x = Screen.width / 2;
        screen_offset.y = Screen.height / 2;
        ThrusterAudio.clip = thrust_audio;
        ThrusterAudio.Play();
        ThrusterAudio.loop = true;
    }

    void FixedUpdate()
    {
        //Thrust();

        PhysThrust();
        LookTurn();
        CombatControls();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Thrust()
    {
        throttle += Input.GetAxis("Vertical") * Time.deltaTime;

        if (throttle > 3)
        {
            throttle -= throttle * 0.2f * Time.deltaTime;
        }

        if (throttle < -0.5f)
        {
            throttle = -0.5f;
        }

        if (throttle > 0)
        {
            local_transform.position += local_transform.forward * Time.deltaTime * movevment_speed * throttle;
        }

        local_transform.position += local_transform.right * Time.deltaTime * movevment_speed / 2 * Input.GetAxis("Horizontal");

        Camera.main.fieldOfView = 65.7f + (4.40f * throttle) + (4f * Input.GetAxis("Vertical"));

        ThrusterAudio.volume = Mathf.Clamp(throttle / 3, 0, 1);
    }


    void PhysThrust()
    {
        throttle += Input.GetAxis("Vertical") * Time.deltaTime;

        if (throttle > 3)
        {
            throttle -= throttle * 0.1f * Time.deltaTime;
        }

        if (throttle < -2f)
        {
            throttle = -2f;
        }

        if (throttle > 0)
        {
            //local_transform.position += local_transform.forward * Time.deltaTime * movevment_speed * throttle;
        }

        //local_transform.position += local_transform.right * Time.deltaTime * movevment_speed / 2 * Input.GetAxis("Horizontal");

        rigidbody.AddForce(throttle * movevment_speed * 20 * transform.forward);
        rigidbody.AddForce(local_transform.right * Time.deltaTime * movevment_speed * 900 * Input.GetAxis("Horizontal"));
        rigidbody.AddForce(local_transform.up * Time.deltaTime * movevment_speed * 900 * Input.GetAxis("Jump"));


        Camera.main.fieldOfView = 65.7f + (4.40f * throttle) + (4f * Input.GetAxis("Vertical"));

        ThrusterAudio.volume = Mathf.Clamp(throttle / 3, 0, 1);
    }


    void TurnController()
    {
        Vector3 mouse_vector;

        mouse_vector = Input.mousePosition - previous_mouse_position;

        mouse_vector = mouse_vector.normalized;

        if (!Input.GetButton("look"))
        {
            rotationVector.x += Input.GetAxis("Mouse X") * Time.deltaTime * look_rate;
            rotationVector.y += Input.GetAxis("Mouse Y") * Time.deltaTime * look_rate;
        }
        previous_mouse_position = Input.mousePosition;

        DampTurn();
    }


    void DampTurn()
    {
        rotationVector -= rotationVector * 0.08f;
        //rigidbody.AddTorque(-d_rotationVector * 40f * Time.deltaTime) ;
    }
    void LookTurn()
    {
        TurnController();
        float roll = Input.GetAxis("Roll") * Time.deltaTime * turn_speed;
        //local_transform.Rotate(-rotationVector.y,rotationVector.x,-roll);
        d_rotationVector = new Vector3(-rotationVector.y * 1.4f, rotationVector.x * 1.4f, -roll);
        rigidbody.AddRelativeTorque(d_rotationVector * 10);
    }


    void CombatControls()
    {
        if (Input.GetMouseButton(0))
        {
            weapon_comp.FireGun();
        }

        if (Input.GetKeyDown("f"))
        {
            weapon_comp.FireMissile();
        }
    }

    public Vector3 getVelocity()
    {
        return rigidbody.velocity;
    }

    public void died()
    {
        scene.playerDead();
    }
    public float throttleRatio()
    {
        if (throttle < 0)
        { return throttle/(-2); }
        else
        {
            return throttle / 5;
        }
    }
}
