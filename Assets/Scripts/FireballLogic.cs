using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : MonoBehaviour
{
    Rigidbody m_rigidbody;

    float m_fireballSpeed = 8.0f;

    [SerializeField]
    ParticleSystem m_fireball;

    [SerializeField]
    ParticleSystem m_explosion;

    Collider m_collider;

    float m_lifeTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody)
        {
            m_rigidbody.velocity = transform.forward * m_fireballSpeed;
            Debug.Log("Setting velocity");
        }

        m_collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        m_lifeTime -= Time.deltaTime;
        if(m_lifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }   
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_collider.enabled = false;

            PlayerLogic playerLogic = other.GetComponent<PlayerLogic>();
            if(playerLogic)
            {
                playerLogic.Die();
            }

            m_fireball.Stop(true);
            m_explosion.Play(true);
            m_rigidbody.velocity = Vector3.zero;
        }
    }
}
