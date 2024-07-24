using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


class FormationData
{
    public bool in_formation = false;
    public int formation_id = 0;
    public bool formation_lead = false;
}

class CompTelemetry
{
    public float aggressiveness_index = 0f;
    public float attack_index = 0.4f;
    public float s_attack_index = 0.4f;

    public bool dogfight = true;

    public bool chase = true;
    public int[] direction_choice = { 0, 0, 0};
}


class DogFightTarget
{
    public Transform m_target;    
    public Vector3 attack_vector;

}

public class CompController : MonoBehaviour
{

    ObjectiveBehaviour primary_objective;

    MeshRenderer mesh;

    [SerializeField] float movevment_speed = 100.0f;
    [SerializeField] float turn_speed = 40.0f;
    [SerializeField] float look_rate = 5.0f;

    [SerializeField] float aggro_distance = 40.0f;
    [SerializeField] float reaction_time = 40.0f;

    [SerializeField] Transform ray_origin;

    [SerializeField] AudioClip thrust_audio;
    [SerializeField] AudioSource ThrusterAudio;

    FormationData fomration_data = new FormationData();
    CompTelemetry telemetry = new CompTelemetry();
    DogFightTarget target = new DogFightTarget();

    int current_mode = 0;



    Transform group_leader;

    Vector2 rotationVector = Vector2.zero;
    Vector3 d_rotationVector = Vector3.zero;
    float m_roll = 0;

    Transform local_transform;
    float throttle = 2;

    Vector3 direction_vector = Vector3.zero;
    Vector3 previous_distance = Vector3.zero;
    Rigidbody m_rigidbody;

    bool colliding = false;


    CompWeaponComputer weapon_comp;

    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();

        local_transform = transform;
        weapon_comp = GetComponent<CompWeaponComputer>();
        m_rigidbody = GetComponent<Rigidbody>();
        primary_objective = FindFirstObjectByType<ObjectiveBehaviour>();
    }

    void Start()
    {
        ThrusterAudio.clip = thrust_audio;
        ThrusterAudio.Play();
        ThrusterAudio.loop = true;
    }

    void FixedUpdate()
    {
        StateManger();
        PhysThrust();
        LookTurn();
    }

    void Thrust()
    {
        if (throttle > 3.5)
        {
            throttle -= throttle * 0.1f * Time.deltaTime;
        }

        if (throttle < 0)
        {
            throttle = 0;
            ThrustControl(true);
        }

        if (throttle > 0)
        {
            local_transform.position += local_transform.forward * Time.deltaTime * movevment_speed * throttle;
        }

        ThrusterAudio.volume = Mathf.Clamp(throttle / 3, 0, 1);

      
    }


    void PhysThrust()
    {
        if (throttle > 3.5)
        {
            throttle -= throttle * 0.1f * Time.deltaTime;
        }

        if (throttle < 0)
        {
            throttle = 0;
            ThrustControl(true);
        }

        if (throttle > 0)
        {
            m_rigidbody.AddForce(throttle * movevment_speed * 20 * transform.forward);

        }
        ThrusterAudio.volume = Mathf.Clamp(throttle / 3, 0, 1);

    }


    void TurnController()
    {

        DampTurn();
    }


    void DampTurn()
    {
        rotationVector -= rotationVector * 0.5f * Time.deltaTime;
        m_roll -= m_roll * 0.5f * Time.deltaTime;
    }


    void LookTurn()
    {
        TurnController();

        //local_transform.Rotate(-rotationVector.y, rotationVector.x, m_roll);
        Vector3 d_rotationVector = new Vector3(-rotationVector.y * 0.5f, rotationVector.x * 0.5f, m_roll);
        m_rigidbody.AddRelativeTorque(d_rotationVector * 5);

    }

    void Roll(bool direction)
    {

        if (direction)
        {
            m_roll += Time.deltaTime * look_rate;
        }

        else
        {
            m_roll -= Time.deltaTime * look_rate;
        }
    }

    void Yaw(bool direction)
    {
        if (direction)
        {
            rotationVector.x += look_rate * Time.deltaTime;
        }

        else
        {
            rotationVector.x -= look_rate * Time.deltaTime;
        }
    }

    void Pitch(bool direction)
    {

        if (direction)
        {
            rotationVector.y += look_rate * Time.deltaTime;
        }

        else
        {
            rotationVector.y -= look_rate * Time.deltaTime;
        }
    }


    void ThrustControl(bool direction)
    {
        if (direction) { throttle += 0.1f * Time.deltaTime; }
        else { throttle -= 0.1f * Time.deltaTime; }

    }

    void ThrustControlSemi()
    {

        if (throttle > 2.5) { throttle -= 0.1f * Time.deltaTime; }
        if (throttle < 1.5) { throttle += 0.1f * Time.deltaTime; }


    }

    void DogFight()
    {
        
        if ((primary_objective.transform.position - transform.position).magnitude > primary_objective.distance)
        {
            FocusObjective();
            ThrustControl(true);
            return;
        }

        if (target.attack_vector.magnitude > aggro_distance * 3)
        {
            Chase();
            return;
        }

        if (Vector3.Angle(transform.forward, target.attack_vector) < 39)
        {
            ThrustControlSemi();

            if (target.attack_vector.magnitude > aggro_distance * 2)
            {
                ThrustControl(true);
            }
            weapon_comp.FireGun();
        }

        telemetry.attack_index = Vector3.Angle(transform.forward, target.attack_vector);
        

        if (telemetry.chase)
        {
            if (telemetry.attack_index < 70)
            {
                Chase();
            }
            else
            {
                telemetry.chase = false;
            }
        }
        else
        {
            if (telemetry.attack_index > 120)
            {
                Chased();
            }
            else
            {
                telemetry.chase = true;
            }
        }

        //telemetry.dogfight = false;
    }


    void Chase()
    {
        //("chase!");
        EndManouver();


        if (target.attack_vector.magnitude > aggro_distance)
        {
            ThrustControl(true);
        }
        else 
        {
            ThrustControlSemi();
        }

        ManouverTowards();
        //FocusObjective();
        RollManouverOnly();
    }

    void Chased()
    {
        //("chased!");

        //mesh.material.SetColor("_Color",Color.red);q

        ThrustControl(true);

        if (Random.Range(0, 5) < 2)
        {
        }

        if (direction_vector.magnitude < aggro_distance / 4) { ManouverAway(); Manouver(); }
        else if (direction_vector.magnitude < aggro_distance / 1.5) { LightManouverTowards(); Manouver(); }
        else if (direction_vector.magnitude > aggro_distance * 2) { EndManouver();  ManouverTowards(); }
        //else { EndManouver(); }
        //FocusObjective();
    }

    void EndManouver()
    {
        telemetry.direction_choice[0] = 0;
        telemetry.direction_choice[1] = 0;
        telemetry.direction_choice[2] = 0;

    }

    void calcManouver()
    {
        telemetry.direction_choice[0] = (int)Random.Range(0,5);
        telemetry.direction_choice[1] = (int)Random.Range(0, 3);
        telemetry.direction_choice[2] = (int)Random.Range(0, 4);
    }

    void ManouverTowards()
    {
        Quaternion look_at_target = Quaternion.LookRotation(target.attack_vector);
        Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time  * 100 * Time.deltaTime);




        m_rigidbody.MoveRotation(rotating);

        //RollManouverOnly();
    }

    void LightManouverTowards()
    {
        Quaternion look_at_target = Quaternion.LookRotation(target.attack_vector);
        Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * 50 * Time.deltaTime);

        m_rigidbody.MoveRotation(rotating);

        //RollManouverOnly();
    }


    void ManouverAway()
    {
        Quaternion look_at_target = Quaternion.LookRotation(-target.attack_vector);
        Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * 50 * Time.deltaTime);

        m_rigidbody.MoveRotation(rotating);

        //RollManouverOnly();
    }

    void FocusObjective()
    {

        Vector3 dirVector  = primary_objective.transform.position - transform.position;
        Quaternion look_at_target = Quaternion.LookRotation(dirVector);
        Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * 100 * Time.deltaTime);

        m_rigidbody.MoveRotation(rotating);
    }

    void RollManouverOnly()
    {
        calcManouver();
        if (telemetry.direction_choice[2] == 1)
        {
            Roll(true);
        }
        if (telemetry.direction_choice[2] == 2)
        {
            Roll(false);
        }
    }

    void Manouver()
    {
        calcManouver();
        ThrustControl(true);
        if (telemetry.direction_choice[0] == 1)
        {
            Pitch(true);
        }
        if (telemetry.direction_choice[1] == 1)
        {
            Yaw(true);
        }
        if (telemetry.direction_choice[2] == 1)
        {
            Roll(true);
        }
        if (telemetry.direction_choice[0] == 2)
        {
            Pitch(false);
        }
        if (telemetry.direction_choice[1] == 2)
        {
            Yaw(false);
        }
        if (telemetry.direction_choice[2] == 2)
        {
            Roll(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //m_rigidbody.AddForce(Vector3.Reflect(m_rigidbody.velocity, collision.contacts[0].normal));

        //m_rigidbody.AddTorque(m_rigidbody.velocity);
    }


    void CollisionDetection()
    {

        int reaction_multi = 100;
        RaycastHit hit;
        Vector3 ray_dir = transform.TransformPoint(m_rigidbody.velocity.normalized);
        Ray forward = new Ray(transform.position, ray_dir);

        Ray forward_ray = new Ray(ray_origin.position, ray_origin.forward);

        Ray left_ray = new Ray(transform.position + (-transform.right * 5), -transform.right);

        Ray right_ray = new Ray(transform.position + (transform.right * 5), transform.right);

        Ray up_ray = new Ray(transform.position + (transform.up * 5), transform.up);

        Ray down_ray = new Ray(transform.position + (-transform.up * 5), -transform.up);

        Debug.DrawRay(ray_origin.position, ray_origin.forward);

      
        if (Physics.Raycast(forward, out hit, 100))
        {
            EndManouver();
            Vector3 dirVector = hit.transform.position - transform.position;
            Quaternion look_at_target = Quaternion.LookRotation(-dirVector);
            Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * reaction_multi * Time.deltaTime);

            m_rigidbody.MoveRotation(rotating);
            colliding = true;
        }

        else if(colliding)
        {
            ThrustControl(true);
            colliding = false;
        }


        if (Physics.Raycast(forward_ray, out hit, 80))
        {
            EndManouver();

            ThrustControl(false);
            print("hit hit hit");
            Vector3 dirVector = hit.transform.position - transform.position;
            Quaternion look_at_target = Quaternion.LookRotation(-dirVector);
            Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * reaction_multi * Time.deltaTime);
            m_rigidbody.MoveRotation(rotating);
            colliding = true;
            if ((hit.transform.position - transform.position).magnitude < 20)
            {
                m_rigidbody.AddRelativeTorque(new Vector3(12000, 0, 0));
            }
        }

        else if (colliding)
        {
            colliding = false;
        }

        if (Physics.Raycast(left_ray, out hit, 80))
        {
            EndManouver();
            Vector3 dirVector = hit.transform.position - transform.position;
            Quaternion look_at_target = Quaternion.LookRotation(-dirVector);
            Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * reaction_multi * Time.deltaTime);
            m_rigidbody.MoveRotation(rotating);
            colliding = true;
        }

        else if (colliding)
        {
            colliding = false; 
        }


        if (Physics.Raycast(right_ray, out hit, 80))
        {
            EndManouver();
            Vector3 dirVector = hit.transform.position - transform.position;
            Quaternion look_at_target = Quaternion.LookRotation(-dirVector);
            Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * reaction_multi * Time.deltaTime);
            m_rigidbody.MoveRotation(rotating);
            colliding = true;
        }

        else if (colliding)
        {
            colliding = false;
        }



        if (Physics.Raycast(up_ray, out hit, 80))
        {
            EndManouver();
            Vector3 dirVector = hit.transform.position - transform.position;
            Quaternion look_at_target = Quaternion.LookRotation(-dirVector);
            Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * reaction_multi * Time.deltaTime);
            m_rigidbody.MoveRotation(rotating);
            colliding = true;
        }

        else if (colliding)
        {
            colliding = false;
        }


        if (Physics.Raycast(down_ray, out hit, 80))
        {
            EndManouver();
            Vector3 dirVector = hit.transform.position - transform.position;
            Quaternion look_at_target = Quaternion.LookRotation(-dirVector);
            Quaternion rotating = Quaternion.RotateTowards(transform.rotation, look_at_target, reaction_time * reaction_multi * Time.deltaTime);
            m_rigidbody.MoveRotation(rotating);
            colliding = true;
        }

        else if (colliding)
        {
            colliding = false;
        }



    }

    void Formation()
    {
        if (previous_distance.magnitude > direction_vector.magnitude)
        {
            ThrustControl(true);
        }
        else
        {
            ThrustControl(true);
        }
    }

    bool LocationCheck()
    {
        bool distance_increasing;
        previous_distance = direction_vector;

        direction_vector = target.m_target.position - transform.position;
        if (previous_distance.magnitude < direction_vector.magnitude)
        {
            distance_increasing = false;
        }
        else
        {
            distance_increasing = true;
        }
        target.attack_vector = direction_vector;
        return distance_increasing;
    }

    void StateManger()
    {
        target.m_target = weapon_comp.TargetLock();
        
        LocationCheck();
        CollisionDetection();
        if (!colliding)
        {
            if (fomration_data.in_formation)
            {
                Formation();
            }
            else
            {
                if (telemetry.dogfight) {DogFight();}
            }
        }
    }
}
