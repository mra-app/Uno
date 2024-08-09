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
    public Sprite BackImg;
    Sprite FrontImg;

    void Awake()
    {
        img = GetComponent<Image>();

    }
    public void ShowBackImg(bool back)
    {
        if(back)
            img.sprite = BackImg;
        else
            img.sprite = FrontImg;
    }
    public void setIDandImg(int id,Sprite sprite)
    {
        this.id = id;

        if (sprite !=null && img != null)
        {
            img.sprite =sprite;
            FrontImg = sprite;
        }
        ShowBackImg(true);

    }
}
