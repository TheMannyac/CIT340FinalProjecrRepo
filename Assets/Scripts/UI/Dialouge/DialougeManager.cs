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

    private void OnEnable()
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

        foreach (char letter in sentence.ToCharArray())
        {
            SoundManager.instance.PlaySound("text");
            dialougeText.text += letter;
            yield return new WaitForSeconds(.04f);

        }
    }

    public void EndDialouge()
    {
        animator.SetBool("IsOpen", false);
        StartCoroutine(EndGameFadeOut());
        Debug.Log("End of conversation");
    }

    private IEnumerator EndGameFadeOut()
    {
        //Create Dramatic black screen at the start that covers everything
        DramaticScreen bg = Instantiate(GameManagment.Instance.dramaScreePF, new Vector2(3, -23), Quaternion.identity);
        bg.onLoadCommand = DramaticScreen_OnLoadCommands.FadeIn;
        bg.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        bg.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;

        yield return new WaitForSeconds(2.5f);
        GameManagment.ExitGame();

    }

}
