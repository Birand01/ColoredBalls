using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManagerInstance;
    public bool gameState;
    public GameObject[] menuElement=new GameObject[2];
    private void Awake()
    {
        gameState = false;
        if(menuManagerInstance==null)
        {
            menuManagerInstance = this;
        }
        menuElement[3].GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
    }
    void Update()
    {
        
    }
    public void StartTheGame()
    {
        gameState = true;
        menuElement[0].SetActive(false); 
        GameObject.FindWithTag("airParticle").GetComponent<ParticleSystem>().Play();
    }
    public void Retry_Btn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} 
