using System;
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

    public event Action<int,Owner> OnSelected;
    public int id;
    public CardType type;
    Image img;
    public Sprite BackImg;
    Sprite FrontImg;
    public MoveObject moveComponent;
    public Owner owner;
    public int globalCardIdx;
    void Awake()
    {
        img = GetComponent<Image>();
        moveComponent = GetComponent<MoveObject>();
        moveComponent.targetTransform = transform;
        moveComponent.Duration = 0.1f;

    }
    public void ShowBackImg(bool back)
    {
        if(back)
            img.sprite = BackImg;
        else
            img.sprite = FrontImg;
    }
    public void setIDandImg(int id,Sprite sprite,int _globalCardIdx)
    {
        this.id = id;

        if (sprite !=null && img != null)
        {
            img.sprite =sprite;
            FrontImg = sprite;
        }
        ShowBackImg(true);
        globalCardIdx = _globalCardIdx;

    }
    public void OnClick()
    {
        OnSelected?.Invoke(globalCardIdx, owner);
    }
    //Move is called after Onclick is processed through manager
    public void Move(Vector3 EndPosition,Action callback)
    {
        moveComponent.Move(EndPosition, callback);
    }
}
