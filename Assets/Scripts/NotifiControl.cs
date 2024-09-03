using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotifiControl : MonoBehaviour
{
    public GameObject UI;
    public TMP_Text text;
    public void ShowNotification(string message)
    {
        text.text = message;
        StartCoroutine(Display());
      
    }
    IEnumerator Display()
    {
        UI.SetActive(true);
        yield return new WaitForSeconds(1);
        UI.SetActive(false);
    }
}
