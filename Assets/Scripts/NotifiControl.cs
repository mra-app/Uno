using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotifiControl : MonoBehaviour
{
    public GameObject UI;
    public TMP_Text text;
    public Animator animator;
    public AudioSource Audio;
    public void ShowNotification(string message,int type)
    {
        
        text.text = message;
        if (type == 1)
        {
            animator.SetTrigger("UNOF");
            // StartCoroutine(Display());
        }
        else if (type == 2)
        {
            animator.SetTrigger("DRAW2");
        }
        else if(type == 3) 
        {        
            animator.SetTrigger("REV");
        }
        else if (type == 4)
        {
            animator.SetTrigger("SKIP");
        }
        if (type == 5)
        {
            animator.SetTrigger("DRAW4");
        }
        Audio.Play();

    }
    IEnumerator Display()
    {
        UI.SetActive(true);
        yield return new WaitForSeconds(1);
        UI.SetActive(false);
    }
}
