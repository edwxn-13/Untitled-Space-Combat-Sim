using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    // Start is called before the first frame

    Transform local_transform;

    [SerializeField] float minScale = 6.2f;
    [SerializeField] float maxScale = 8.5f;

    [SerializeField] float rotation_offset = 50.0f;

    Vector3 random_rotation;


    void Awake()
    {
        local_transform = transform;
    }

    void Start()
    {
        Vector3 random_scale = Vector3.one;

        random_scale.x = Random.Range(minScale, maxScale);
        random_scale.y = Random.Range(minScale, maxScale);
        random_scale.z = Random.Range(minScale, maxScale);

        transform.localScale = random_scale;

        random_rotation.x = Random.Range(-rotation_offset, rotation_offset);
        random_rotation.y = Random.Range(-rotation_offset, rotation_offset);
        random_rotation.z = Random.Range(-rotation_offset, rotation_offset);
    }

    // Update is called once per frame
    void Update()
    {
        local_transform.Rotate(random_rotation * Time.deltaTime / 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rig = collision.gameObject.GetComponent<Rigidbody>();
        rig.AddForce(0.5f * Vector3.Reflect(rig.velocity, (rig.transform.position - transform.position)));

        Vector3 rot = Vector3.RotateTowards(transform.position, -(rig.transform.position - transform.position), 0.2f, 0.2f);
        rig.AddRelativeTorque(rot);
    }
}
