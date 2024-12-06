using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject readyPanel;
    public GameObject setPanel;
    public GameObject goPanel;
    public GameObject manager;
    public GameObject player;
    private bool hasGameStarted;

    private void Update()
    {
        if (!hasGameStarted)
        {

        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.W))
        {
            goPanel.SetActive(false);
            readyPanel.SetActive(false);
            setPanel.SetActive(true);

            if(Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Y))
            {
                readyPanel.SetActive(false);
                setPanel.SetActive(false);
                goPanel.SetActive(true);
                Invoke("FakePlayGame", 0.1f);
            }
        }
        else
        {
            BackToOriginalMenu();
        }
        }
    }

    private void BackToOriginalMenu()
    {
        setPanel.SetActive(false);
        goPanel.SetActive(false);
        readyPanel.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }    
    public void FakePlayGame()
    {
        Destroy(setPanel);
        Destroy(readyPanel);
        Destroy(goPanel);
        manager.SetActive(true);
        player.GetComponent<PlayerControl>().enabled = true;
        hasGameStarted = true;
    }
}
