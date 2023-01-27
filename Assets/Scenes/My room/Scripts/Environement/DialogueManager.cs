using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Ink.Runtime;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class DialogueManager : MonoBehaviour
{
	public static event Action<Story> OnCreateStory;
	public bool dialogueIsPlaying;

	[SerializeField]
	public TextAsset inkJSONAsset;
	public Story story;

	// UI Prefabs
	[SerializeField]
	private TMP_Text dialogueText = null;
	[SerializeField]
	private Button buttonPrefab = null;
	[SerializeField]
	private GameObject dialoguePanel = null;
	[SerializeField]
	private GameObject choicesHolder = null;
	[SerializeField]
	private MyPlayer player = null;

	void Awake()
	{
		// Remove the default message
		RemoveChildren();
		player = MyPlayer.Instance;
	}

	private static DialogueManager instance;
	public static DialogueManager Instance
	{
		get
		{
			if(instance == null) instance = GameObject.FindObjectOfType<DialogueManager>();
			return instance;
		}
	}

    // Creates a new Story object with the compiled story which we can then play!
    public void StartStory()
	{
		if(dialogueIsPlaying)
		{
			player.IsFrozen = true;
			dialoguePanel.SetActive(true);
			story = new Story(inkJSONAsset.text);
			if(OnCreateStory != null) OnCreateStory(story);
		}
		if(dialogueIsPlaying)
			RefreshView();
	}

	public void EndStory()
    {
		dialogueIsPlaying = false;
		dialoguePanel.SetActive(false);
		player.IsFrozen = false;
	}

	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView()
	{
		// Remove all the UI on screen
		RemoveChildren();

		// Read all the content until we can't continue any more
		while(story.canContinue)
		{
			// Continue gets the next line of the story
			string text = story.Continue();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0)
		{
			for(int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate
				{
					OnClickChoiceButton(choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else
		{
			Button choice = CreateChoiceView("*Leave the Conversation*");
			choice.onClick.AddListener(delegate {
				EndStory();
			});
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView(string text)
	{
		dialogueText.text = text;
	}

	// Creates a button showing the choice text
	Button CreateChoiceView(string text)
	{
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(choicesHolder.transform, false);

		// Gets the text from the button prefab
		TMP_Text choiceText = choice.GetComponentInChildren<TMP_Text>();
		choiceText.text = text;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren()
	{
		int childCount = choicesHolder.transform.childCount;
		for(int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy(choicesHolder.transform.GetChild(i).gameObject);
		}

	}

}
