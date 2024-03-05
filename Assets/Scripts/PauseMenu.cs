using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject suelo;
    public Material colorSuelo;

    public void ResumeGame(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart(){
        suelo.GetComponent<Renderer>().material = colorSuelo;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
