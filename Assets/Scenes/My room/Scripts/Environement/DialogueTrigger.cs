using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Properties")]
    public bool canceled;
    public List<GameObject> UI;
    public List<GameObject> UIEnabled;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        visualCue.SetActive(playerInRange);
        if(playerInRange && !DialogueManager.Instance.dialogueIsPlaying && canceled)
        {
            if(MyPlayer.Instance.interactAction.WasReleasedThisFrame())
                ToInteract();
        }
        else
        {
            if(!canceled)
                canceled = true;
        }

    }

    void ToInteract()
    {
        foreach(GameObject UIElement in UI)
        {
            if(UIElement.activeSelf)
                UIEnabled.Add(UIElement);
            UIElement.SetActive(false);
        }
        DialogueManager.Instance.inkJSONAsset = inkJSON;
        DialogueManager.Instance.dialogueIsPlaying = true;
        DialogueManager.Instance.StartStory(UIEnabled);
    }    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerInRange = false;
            canceled = false;
        }
    }

}