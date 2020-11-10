using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int m_p1score = 0;
    int m_p2socre = 0;

    [SerializeField]
    Text m_p1scoreText;

    [SerializeField]
    Text m_p2scoreText;

    private void OnEnable() 
    {
         PlayerLogic.OnPlayerDeath += OnUpdateScore;
    }

    private void OnDisable() 
    {
        PlayerLogic.OnPlayerDeath -= OnUpdateScore;
    }

    void OnUpdateScore(int playernum)
    {
        if( playernum == 2)
        {
            ++m_p1score;
            m_p1scoreText.text = "" + m_p1score;
        }
        else if (playernum == 1)
        {
             ++m_p2socre;
            m_p2scoreText.text = "" + m_p2socre; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
