using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuAnimation : MonoBehaviour
{
    public Animator anim;

    public void MainMenu()
    {
        anim.Play("MainPanel");
    }
    public void Settings()
    {
        anim.Play("SettingsPanel");
    }
    public void Controls()
    {
        anim.Play("ControlsPanel");
    }
}
