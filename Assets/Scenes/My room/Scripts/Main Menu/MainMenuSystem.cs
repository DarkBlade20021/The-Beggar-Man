using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MainMenuSystem : MonoBehaviour
{

    [Header("Animations")]
    public Animator anim;

    [Header("First Buttons")]
    public GameObject quitFirstButton;
    public GameObject firstButton;
    public GameObject controlsFirstButton;
    public GameObject xboxFirstButton;
    public GameObject keyboardFirstButton;
    public GameObject settingsFirstButton;
    [Header("Menus")]
    public GameObject quit;
    public GameObject home;
    public GameObject controls;
    public GameObject xbox;
    public GameObject keyboard;
    public GameObject settings;
    public string currentSelecting;
    public string quitTxt;
    public string homeTxt;
    public string controlsTxt;
    public string xboxTxt;
    public string keyboardTxt;
    public string settingsTxt;

    //[Header("Used Buttons")]
    //public InputAction navigate;
    public InputAction submit;
    public InputAction cancel;
    private static MainMenuSystem instance;
    public static MainMenuSystem Instance
    {
        get
        {
            if(instance == null) instance = GameObject.FindObjectOfType<MainMenuSystem>();
            return instance;
        }
    }

    void Update()
    {
        #region AnyButtonChecker
        //if (InputSystem.devices.Count > 0)
        //{
        //    foreach (InputDevice device in InputSystem.devices)
        //    {
        //        if (device is Keyboard keyboard && keyboard.anyKey.isPressed)
        //        {
        //            if (navigate.WasPressedThisFrame())
        //                return;
        //            else if (submit.WasPressedThisFrame())
        //                return;
        //            else if (cancel.WasPressedThisFrame())
        //                return;
        //            ReturnButton();
        //        }
        //        else if (device is Gamepad gamepad)
        //        {
        //            bool anyButtonPressed = false;
        //            foreach (InputControl control in gamepad.allControls)
        //            {
        //                if (control is ButtonControl button && button.isPressed)
        //                {
        //                    anyButtonPressed = true;
        //                    break;
        //                }
        //            }
        //            if (anyButtonPressed)
        //            {
        //                if (navigate.WasPressedThisFrame())
        //                    return;
        //                else if (submit.WasPressedThisFrame())
        //                    return;
        //                else if (cancel.WasPressedThisFrame())
        //                    return;
        //                ReturnButton();
        //            }
        //        }
        //    }
        //}
        #endregion

        #region BackButton
        if(cancel.WasPerformedThisFrame())
        {
            if (currentSelecting == quitTxt)
                QuitToHome();
            else if (currentSelecting == homeTxt)
                HomeToQuit();
            else if (currentSelecting == controlsTxt)
                ControlsToHome();
            else if (currentSelecting == settingsTxt)
                SettingsToHome();
            else if (currentSelecting == xboxTxt)
                XboxToControls();
            else if (currentSelecting == keyboardTxt)
                KeyboardToControls();
        }
        #endregion

        #region SubmitCanceller
        if(submit.WasPerformedThisFrame())
        {
            if (currentSelecting == quitTxt)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(quitFirstButton);
            }
            else if (currentSelecting == homeTxt)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstButton);
            }
            else if (currentSelecting == controlsTxt)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(controlsFirstButton);
            }
            else if (currentSelecting == settingsTxt)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(settingsFirstButton);
            }
            else if (currentSelecting == xboxTxt)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(xboxFirstButton);
            }
            else if (currentSelecting == keyboardTxt)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(keyboardFirstButton);
            }
        }
        #endregion
    }

    private void OnEnable()
    {
    //    navigate.Enable();
        submit.Enable();
        cancel.Enable();
    }

    #region ReturnButton
    //void ReturnButton()
    //{
    //    if (navigate.WasPerformedThisFrame())
    //        return;
    //    else if (submit.WasPerformedThisFrame())
    //        return;
    //    else if (cancel.WasPerformedThisFrame())
    //        return;
    //    if(currentSelecting == homeTxt)
    //    {
    //        EventSystem.current.SetSelectedGameObject(null);
    //        EventSystem.current.SetSelectedGameObject(firstButton);
    //    } else if (currentSelecting == controlsTxt)
    //    {
    //        EventSystem.current.SetSelectedGameObject(null);
    //        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    //    } else if (currentSelecting == settingsTxt)
    //    {
    //        EventSystem.current.SetSelectedGameObject(null);
    //        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    //    }
    //}
    #endregion

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
		anim.Play("FadeIn");
		yield return new WaitForSeconds(2.5f);
        currentSelecting = homeTxt;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
	}
    public void Quit()
    {
        StartCoroutine(QuitR());
    }
    IEnumerator QuitR()
    {
        anim.Play("FadeOut");
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameR());
    }

    IEnumerator StartGameR()
    {
        anim.Play("FadeOut");
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Level 1");
        currentSelecting = homeTxt;
    }

    public void HomeToQuit()
    {
        currentSelecting = quitTxt;
        home.SetActive(true);
        quit.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitFirstButton);
    }
    public void QuitToHome()
    {
        currentSelecting = homeTxt;
        quit.SetActive(false);
        home.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void HomeToControls()
    {
        currentSelecting = controlsTxt;
        home.SetActive(false);
        controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void ControlsToXbox()
    {
        currentSelecting = xboxTxt;
        xbox.SetActive(true);
        controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(xboxFirstButton);
    }
    public void ControlsToKeyboard()
    {
        currentSelecting = keyboardTxt;
        keyboard.SetActive(true);
        controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(keyboardFirstButton);
    }
    public void XboxToControls()
    {
        currentSelecting = controlsTxt;
        xbox.SetActive(false);
        controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void KeyboardToControls()
    {
        currentSelecting = controlsTxt;
        keyboard.SetActive(false);
        controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void HomeToSettings()
    {
        currentSelecting = settingsTxt;
        home.SetActive(false);
        settings.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }
    public void ControlsToHome()
    {
        currentSelecting = homeTxt;
        home.SetActive(true);
        controls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    public void SettingsToHome()
    {
        currentSelecting = homeTxt;
        home.SetActive(true);
        settings.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

}
