using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuSystem : MonoBehaviour
{

    [Header("Animations")]
    public Animator fade;

    [Header("First Buttons")]
    public GameObject firstButton;

    private static MainMenuSystem instance;
    public static MainMenuSystem Instance
    {
        get
        {
            if(instance == null) instance = GameObject.FindObjectOfType<MainMenuSystem>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(StartR());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

	IEnumerator StartR()
	{
		fade.Play("FadeIn");
		yield return new WaitForSeconds(2.5f);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
	}

    public void StartGame()
    {
        StartCoroutine(StartGameR());
    }

    IEnumerator StartGameR()
    {
        fade.Play("FadeOut");
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Level 1");
    }

}
