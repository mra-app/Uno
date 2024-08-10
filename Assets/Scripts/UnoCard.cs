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
    public MoveObject moveComponent;

    void Awake()
    {
        img = GetComponent<Image>();
        moveComponent = GetComponent<MoveObject>();
        moveComponent.targetTransform = transform;
        moveComponent.EndPosition = new Vector3(11, 0, 0); // temp.position;
        moveComponent.Duration = 0.1f;

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
    public void OnClick()
    {


        moveComponent.Move();
    }
}
