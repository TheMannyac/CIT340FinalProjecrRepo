using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Animator animator;

    public Text nameText;
    public Text dialougeText;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
   
    }

    public void StartDialouge(Dialouge dialouge)
    {
        animator.SetBool("IsOpen", true);
        Debug.Log("Starting Conversation with " + dialouge.name);
        nameText.text = dialouge.name;
      
        sentences.Clear();

        foreach(string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialouge();
            return;
        }

        string oldSentence = sentences.Dequeue();
        
        Debug.Log(oldSentence);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(oldSentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialougeText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            dialougeText.text += letter;
            yield return null;

        }
    }

    public void EndDialouge()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("End of conversation");
    }

}
