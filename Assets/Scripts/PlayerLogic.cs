using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerID
{
    _P1,
    _P2
}
public class PlayerLogic : MonoBehaviour
{

    float m_horizontalInput;
    float m_verticalInput;

    float m_movementSpeed = 5.0f;

    CharacterController m_charactercontroller;

    float m_jumpHeight = 0.25f;
    float m_gravity = 0.981f;

    bool m_jump;
    Vector3 m_movement;
    Vector3 m_heightMovement;

    [SerializeField]
    PlayerID m_playerID;

    Animator m_animator;

    bool m_isCastingFireball = false;

    [SerializeField]
    Transform m_fireballSpawn;

    [SerializeField]
    GameObject m_fireball;

    bool m_isDead = false;

    const float MAX_RESPAWN_TIME = 2.0f;
    float m_respawnTimer = MAX_RESPAWN_TIME;

    Vector3 m_spawnPos;

    // Events

    public delegate void PlayerDeath(int playerNum);
    public static event PlayerDeath OnPlayerDeath;

    
    // Start is called before the first frame update
    void Start()
    {
        m_charactercontroller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isDead)
        {
            return;
        }

        m_horizontalInput = Input.GetAxis("Horizontal" + m_playerID);
        m_verticalInput = Input.GetAxis("Vertical" + m_playerID);

        if(Input.GetButtonDown("Jump" + m_playerID) && m_charactercontroller && m_charactercontroller.isGrounded)
        {
            m_jump = true;
        }   

        if(Input.GetButtonDown("Fire1" + m_playerID ) && m_animator)
        {
            m_animator.SetTrigger("CastFireball");
            m_isCastingFireball = true;
        }
    }

    private void FixedUpdate() 
    {
        if(m_isDead)
        {
            m_respawnTimer -= Time.deltaTime;
            if(m_respawnTimer <= 0)
            {
                Respawn();
            }

            return;
        }

        if(m_jump)
        {
            m_heightMovement.y = m_jumpHeight;
            m_jump = false;
        }

        m_heightMovement.y -= m_gravity * Time.deltaTime;

        m_movement = new Vector3(m_horizontalInput, 0, m_verticalInput) * m_movementSpeed * Time.deltaTime;

        if(m_animator)
        {
            m_animator.SetFloat("MovementInput", (Mathf.Max(Mathf.Abs(m_horizontalInput),Mathf.Abs(m_verticalInput))));
        }


    // Rotate towards movement direction
        if(m_movement != Vector3.zero)
        {
             transform.forward = m_movement.normalized;
        }

        if(m_isCastingFireball)
        {
            m_movement = Vector3.zero;
        }

        m_charactercontroller.Move(m_heightMovement + m_movement);

        if(m_charactercontroller.isGrounded)
        {
            m_heightMovement.y = 0;
        }
    }

    public void SetCastingFireballState(bool isCasting)
    {
        m_isCastingFireball = isCasting;
    }

    public void ReleaseFireball()
    {
        Instantiate(m_fireball, m_fireballSpawn.transform.position, transform.rotation);
    }

    void Respawn()
    {
        m_charactercontroller.enabled = false;
        transform.position = m_spawnPos;
        m_charactercontroller.enabled = true;
        m_respawnTimer = MAX_RESPAWN_TIME;
        m_isDead = false;

        if(m_animator)
        {
            m_animator.SetTrigger("Respawn");
        }
    }

    public void Die() 
    {
        m_isDead = true;

        if(m_animator)
        {
            m_animator.SetTrigger("Die");
            m_isDead = true;
        }

        if(m_charactercontroller)
        {
            m_charactercontroller.enabled = false;
        }

        if(OnPlayerDeath != null)
        {
            OnPlayerDeath(GetPlayerNum());
        }
    }

    int GetPlayerNum()
    {
        if(m_playerID == PlayerID._P1)
        {
            return 1;
        }
        else if(m_playerID == PlayerID._P2)
        {
            return 2;
        }

        return 0;
    }
}
