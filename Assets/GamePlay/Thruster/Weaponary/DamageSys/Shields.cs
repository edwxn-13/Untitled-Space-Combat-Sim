using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Shields : MonoBehaviour
{

    [SerializeField] float m_shield = 1000;
    [SerializeField] float m_recharge_rate = 0.6f;
    [SerializeField] float m_down_time = 5.0f;
    Rigidbody m_rigidbody;
    float shield_max;
    float down_time;

    bool isDown = false;

    DamageEngine damage_engine;

    // Start is called before the first frame update

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        damage_engine = GetComponent<DamageEngine>();
        shield_max = m_shield;
    }

    // Update is called once per frame
    void Update()
    {
        ShieldRecharge();
        ShieldDown();
        /*
        if (m_shield <= 0)
        {
            m_shield = 0;
            if (isDown == false)
            {
                isDown = true;
                down_time = m_down_time;
            }
        }*/
    }

    public float get_shield()
    {
        return m_shield;
    }

    void OnCollisionEnter(Collision col)
    {
        Bullet bullet;
        if (col.gameObject.TryGetComponent<Bullet>(out bullet)) { }
        else { Damage(m_rigidbody.velocity.magnitude / 2); }
    }

    public void Damage(float damage)
    {

        m_shield -= damage;

        if (m_shield <= 0)
        {
            isDown = true;
            down_time = m_down_time;
            ShieldDown(damage);
            m_shield = 0;
            return;
        }
        print("Shield: " + damage);

    }

    void ShieldDown(float dmg)
    {
        if (isDown)
        {
            damage_engine.Damage(dmg);
        }
    }


    public float ShieldRatio()
    {
        return m_shield / shield_max;
    }
    void ShieldRecharge()
    {
        if (!isDown)
        {
            if (m_shield < shield_max)
            {
                m_shield += m_recharge_rate * Time.deltaTime;
            }

            if (m_shield > shield_max) { m_shield = shield_max;  }
        }
    }

    void ShieldDown()
    {
        if (isDown)
        {
            //print("Shield: " + down_time);

            down_time -= Time.deltaTime;
            if (down_time <= 0) { isDown = false; }
        }
    }
}
