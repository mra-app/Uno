using System;
using UnityEngine;
using UnityEngine.UI;

public class UnoCard : MonoBehaviour
{
    public enum CardType {
        Red,
        Green,
        Blue,
        Yellow

    }
    public enum SpecialCard
    {
        None,
        Skip = 10,
        Reverse = 11,
        Take2 = 12,
        Wild = 13,
        Take4 = 14
    }

    public event Action<int,Owner> OnSelected;
    public int id;
    //public CardType type;
    Image img;
    public Sprite BackImg;
    Sprite FrontImg;
    public MoveObject moveComponent;
    public Owner owner;
    public int globalCardIdx;
    CardType Color;
    int Number;
    SpecialCard Type = SpecialCard.None;
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
    public void InitCard(int id,Sprite sprite,int _globalCardIdx)
    {
        this.id = id;

        if (sprite !=null && img != null)
        {
            img.sprite =sprite;
            FrontImg = sprite;
        }
        ShowBackImg(true);
        globalCardIdx = _globalCardIdx;
        SetNumberAndColor();

    }
    public void OnClick()
    {
        SetNumberAndColor();
        OnSelected?.Invoke(globalCardIdx, owner);
    }
    //Move is called after Onclick is processed through manager
    public void Move(Vector3 EndPosition,Action callback)
    {
        moveComponent.Move(EndPosition, callback);
    }
    void SetNumberAndColor()
    {
        int a = (id / 14)%4;
        Color =  a == 0?CardType.Red:a==1?CardType.Yellow:a==2?CardType.Green:CardType.Blue;
        Number = id % 14;
        
        if (Number > 9)
            Type = (SpecialCard)Number;
        if (id > 55)
        {
            int id2 = id - 56;
            Number = (id2 % 13) + 1;
            if (Number > 9)
                Type = Number == 13?SpecialCard.Take4: (SpecialCard)Number;
        }

        DebugControl.Log(id+":"+ Number + " "+Color.ToString()+" "+Type,3);
    }
}
