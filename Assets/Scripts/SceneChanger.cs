using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void SceneChangePlay()
    {
        SceneManager.LoadScene("Nivel1");
        
    } 

    public void Quit()
    {
        Application.Quit();
    }
}
