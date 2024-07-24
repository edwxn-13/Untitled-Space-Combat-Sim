using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEngine : MonoBehaviour
{
    [SerializeField] float m_armour = 1000f;
    SceneController soundSource;

    float max_armour;
    // Start is called before the first frame update
    void Start()
    {
        soundSource = FindFirstObjectByType<SceneController>();

        max_armour = m_armour;
    }

    // Update is called once per frame
    void Update()
    {


    }


    public float ArmourRatio()
    {
        return m_armour / max_armour;
    }

    public void Damage(float damage)
    {
        m_armour -= damage;

        if (m_armour < 0)
        {
            m_armour = 0;
            Death();
            return;
        }
    }

    public void Death()
    {
        //Destroy(this);
        soundSource.ShipExplosion(transform.position);

   

        gameObject.SetActive(false);

    }
}
