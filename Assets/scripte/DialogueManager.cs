using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueex;
    public TMP_Text dialogueText;
    public float displayTime = 2.0f; // Time in seconds the dialogue will be displayed

    private Coroutine displayCoroutine;

    

    void Start()
    {
        if(dialogueex != null)
        {
            dialogueex.gameObject.SetActive(false);
        }
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false); // Ensure the dialogue is hidden initially
        }
    }

    public void ShowDialogue(string message, Color textColor)
    {

        if (dialogueex != null)
        {
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            dialogueex.text = "You Obtained a PowerUp:";
            dialogueex.gameObject.SetActive(true);
            displayCoroutine = StartCoroutine(HideDialogueAfterDelay());
        }
        if (dialogueText != null)
        {
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            dialogueText.text = message;
            dialogueText.color = textColor;
            dialogueText.gameObject.SetActive(true);
            displayCoroutine = StartCoroutine(HideDialogueAfterDelay());
        }
    }

    private IEnumerator HideDialogueAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        dialogueex.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        
    }
}
