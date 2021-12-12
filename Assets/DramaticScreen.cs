using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DramaticScreen_OnLoadCommands
{
    FadeIn,FadeOut,stayBlack,disable
}
public class DramaticScreen : MonoBehaviour
{
    
    public static GameObject blackScreenPF;
    public DramaticScreen_OnLoadCommands onLoadCommand;
    public float fadeSpeed = .06f;


    SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {

        spr = GetComponent<SpriteRenderer>();
        Color c = spr.material.color;

        switch (onLoadCommand)
        {
            case DramaticScreen_OnLoadCommands.FadeIn:
                //set alpha 
                
                c = spr.material.color;
                c.a = 0;
                spr.material.color = c;

                StartCoroutine(FadeIn());
                break;
            case DramaticScreen_OnLoadCommands.stayBlack:

                break;
            case DramaticScreen_OnLoadCommands.FadeOut:

                StartCoroutine(FadeOut());
                break;
            case DramaticScreen_OnLoadCommands.disable:
                gameObject.SetActive(false);
                break;
        }

    }
    
    public IEnumerator FadeIn()
    {
        gameObject.SetActive(true);

        for(float f = .05f; f <= 1; f += fadeSpeed)
        {
            Color c = spr.material.color;
            c.a = f;
            spr.material.color = c;

            yield return new WaitForSeconds(fadeSpeed);
        }
    }

    public  IEnumerator FadeOut()
    {

        for (float f = 1; f > 0; f -= fadeSpeed)
        {

            Color c = spr.material.color;
            c.a = f;
            spr.material.color = c;

            yield return new WaitForSeconds(fadeSpeed);
        }

        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        spr = GetComponent<SpriteRenderer>();
        Color c = spr.material.color;
        c.a = 0;
    }
}
