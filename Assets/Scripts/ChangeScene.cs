using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
public void btn_scene_change(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitButton() { 
        Application.Quit();
        Debug.Log("Game closed");
    }
}
