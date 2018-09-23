using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueCanvasMgr : MonoBehaviour {

    private struct SpeakerInfo
    {
        public readonly string name;
        public readonly Sprite image;

        public SpeakerInfo(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
        }
    }

    [System.Serializable]
    public struct Option
    {
        public Text optionText;
        public Button optionButton;
    }

    public GameObject basicTextbox;
    public GameObject multiTextbox;

    public Sprite defaultSprite;
    public Button nextButton;

    [Header("Speaker Info")]
    public Text speakerName;
    public Image speakerImage;

    // TextBox
    [Header("Simple TextBox")]
    public Text simpleText;

    // Options TextBox
    [Header("Multi-TextBox")]
    public Option option1;
    public Option option2;
    public Option option3;
    [Tooltip("Can be used for special cases such as if you want to have a 'leave' button")]
    public Option specialOption;

    private UnityAction nextAction;
    private Queue<string> sentencesToDisplay = new Queue<string>();
    private bool sentenceComplete = false;
    private string currentSentence;

    public static DialogueCanvasMgr Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextSpeaker(Queue<string> dialogue, string speakerName, Sprite speakerImage, UnityAction next)
    {
        SpeakerInfo speaker = new SpeakerInfo(speakerName, speakerImage);
        DisplaySpeaker(speaker);
        sentencesToDisplay = dialogue;
        nextAction = next;
        Next();
        multiTextbox.SetActive(false);
        basicTextbox.SetActive(true);
    }

    public void NextSpeaker(string[] options, string speakerName, Sprite speakerImage, UnityAction<int>[] onClicks, bool useSpecial = false)
    {
        SpeakerInfo speaker = new SpeakerInfo(speakerName, speakerImage);
        DisplaySpeaker(speaker);

        HideAllButtons();

        // Initialising Buttons
        for(int i = 0; i < options.Length; i++)
        {
            if (i == 2 && useSpecial && onClicks[i] != null) { InitButton(3, options[i], onClicks[i]); return; }
            if (onClicks[i] != null) InitButton(i, options[i], onClicks[i]);
        }

        basicTextbox.SetActive(false);
        multiTextbox.SetActive(true);
    }

    public IEnumerator DisplaySimpleText(string sentence)
    {
        sentenceComplete = false;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        simpleText.text = "";
        char[] letters = sentence.ToCharArray();
        foreach(char letter in letters)
        {
            simpleText.text += letter;
            yield return wait;
        }
        sentenceComplete = true;
    }

    public void DisplayFullSentence(string sentence)
    {
        simpleText.text = sentence;
        sentenceComplete = true;
    }

    private void DisplaySpeaker(SpeakerInfo speaker)
    {
        if (!speakerName.gameObject.activeInHierarchy) speakerName.gameObject.SetActive(true);
        if (!speakerImage.gameObject.activeInHierarchy) speakerImage.gameObject.SetActive(true);
        speakerName.text = speaker.name;
        speakerImage.sprite = speaker.image;
    }

    public void Next()
    {
        // TODO: Skip typing text effect and just display the entire sentence if next is pressed again.
        //if (!sentenceComplete && !string.IsNullOrEmpty(currentSentence))
        //{
        //    DisplayFullSentence(currentSentence);
        //    return;
        //}

        if (sentencesToDisplay.Count > 0)
        {
            currentSentence = sentencesToDisplay.Dequeue();
            StartCoroutine(DisplaySimpleText(currentSentence));
        }else if(nextAction != null)
        {
            nextAction.Invoke();
        }
        else
        {
            // TODO: Simply Hide the Conversation Canvas and send message.
            DisplaySimpleText("...");
        }
    }

    private void InitButton(int index, string text, UnityAction<int> onClick)
    {
        switch (index)
        {
            case 0:
                option1.optionButton.onClick.RemoveAllListeners();
                option1.optionButton.gameObject.SetActive(true);
                option1.optionText.text = text;
                option1.optionButton.onClick.AddListener(delegate { onClick(index); });
                break;
            case 1:
                option2.optionButton.onClick.RemoveAllListeners();
                option2.optionButton.gameObject.SetActive(true);
                option2.optionText.text = text;
                option2.optionButton.onClick.AddListener(delegate { onClick(index); });
                break;
            case 2:
                option3.optionButton.onClick.RemoveAllListeners();
                option3.optionButton.gameObject.SetActive(true);
                option3.optionText.text = text;
                option3.optionButton.onClick.AddListener(delegate { onClick(index); });
                break;
            case 3:
                specialOption.optionButton.onClick.RemoveAllListeners();
                specialOption.optionButton.gameObject.SetActive(true);
                specialOption.optionText.text = text;
                specialOption.optionButton.onClick.AddListener(delegate { onClick(index); });
                break;
        }
    }

    public void Hide()
    {
        basicTextbox.SetActive(false);
        multiTextbox.SetActive(false);
        speakerName.gameObject.SetActive(false);
        speakerImage.gameObject.SetActive(false);
        // TODO: Hide Canvas.
    }

    private void HideAllButtons()
    {
        option1.optionButton.gameObject.SetActive(false);
        option2.optionButton.gameObject.SetActive(false);
        option3.optionButton.gameObject.SetActive(false);
        specialOption.optionButton.gameObject.SetActive(false);
    }
}
