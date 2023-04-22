using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKeyAnim : MonoBehaviour
{
    public void KeyPopUp (Animator visualCueAnim) {
        visualCueAnim.SetBool("Pop Up", true);
        visualCueAnim.SetBool("Idle", false);
        visualCueAnim.SetBool("Pressed", false);
    }   
    public void KeyIdle (Animator visualCueAnim) {
        visualCueAnim.SetBool("Pop Up", false);
        visualCueAnim.SetBool("Idle", true);
        visualCueAnim.SetBool("Pressed", false);
    }
    public void KeyPressed (Animator visualCueAnim) {
        visualCueAnim.SetBool("Pop Up", false);
        visualCueAnim.SetBool("Idle", false);
        visualCueAnim.SetBool("Pressed", true);
    }
}
