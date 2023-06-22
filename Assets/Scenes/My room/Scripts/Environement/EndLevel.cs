using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndLevel : MonoBehaviour
{
    [SerializeField] int thisLevelNum;
    [SerializeField] Animator Fade;
    private void Start()
    {
        thisLevelNum = SceneManager.GetActiveScene().buildIndex;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            StartCoroutine(EndingLevel());
    }

    IEnumerator EndingLevel()
    {
        Fade.Play("FadeOut");
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisLevelNum + 1);
    }

}
