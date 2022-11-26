using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<string> sentences;

    [SerializeField] private GameObject dialogueBox = null;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(DisplayNextSentence());
    }

    IEnumerator DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            yield break;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));

        yield return new WaitForSeconds(4f);
        StartCoroutine(DisplayNextSentence());
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }
}
