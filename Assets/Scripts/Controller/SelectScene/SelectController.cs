using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectController : MonoBehaviour
{
    //싱글톤
    private static SelectController _instance;
    public static SelectController instance
    {
        get { return _instance; }
    }
   
    private void Awake()
    {
        _instance = this;
        heroPanel.SetActive(false);
    }

    //스테이지 이름
    public string stageName;

    //영웅 스텟창
    public GameObject heroPanel;

    //장면 변경
    public void ChangeScene(string scene)
    {
      
        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        SetStageName(clickButton.name);
        Debug.Log(clickButton.name);
        SceneManager.LoadScene(scene);
        
    }    
    
    //스테이지 이름 설정
    public void SetStageName(string name)
    {
        stageName = name;
    }
  
}
