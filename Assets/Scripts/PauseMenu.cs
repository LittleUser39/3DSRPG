using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
   // 다른 스크립트에서 쉽게 접근이 가능하도록 static
  public bool GameIsPaused = false;
  public GameObject pauseMenuCanvas;
    BattleState battlestate;
    void Update()
  {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
          if (GameIsPaused)
          {
              Resume();
          }
          else
          {
              Pause();
          }
      }
  }

  public void Resume()
  {
      pauseMenuCanvas.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused = false;
  }

  public void Pause()
  {
      pauseMenuCanvas.SetActive(true);
      Time.timeScale = 0f;
      GameIsPaused = true;
  }

  public void ToSettingMenu()
  {
    
  }

  public void ToMain()
  {
      Time.timeScale = 1f;
      SceneManager.LoadScene("SelectScen");
  }

  public void QuitGame()
  {
      Application.Quit();
  }
  
}
