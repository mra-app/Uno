using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnoCard : MonoBehaviour
{
    public enum CardType {
        Red,
        Green,
        Blue,
        Yellow,
        WildCard

    }

    
    public int id;
    public CardType type;
    Image img;

    void Awake()
    {
        img = GetComponent<Image>();

    }
    public void setIDandImg(int id,Sprite sprite)
    {
        this.id = id;

        if (sprite !=null && img != null)
            img.sprite = sprite;

    }
}
