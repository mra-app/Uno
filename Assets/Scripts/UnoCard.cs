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
    public int TurnChangeAmount = 1;
    public SpecialCard Type = SpecialCard.None;
    public int AccumulatedCards = 0;
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

        
        TurnChangeAmount =  Type == SpecialCard.Skip? 2: Type == SpecialCard.Reverse ? -1:1;
        AccumulatedCards = Type == SpecialCard.Take2 ? 2 : Type == SpecialCard.Take4 ? 4 : 0;

    }
    public void OnClick()
    {
        //SetNumberAndColor();
        OnSelected?.Invoke(globalCardIdx, owner);
    }
    public void SetWildColor(CardType wildColor)
    {
        Color = wildColor;
    }
    //Move is called after Onclick is processed through manager
    public void Move(Vector3 EndPosition,Action callback)
    {
        moveComponent.Move(EndPosition, callback);
    }
    void SetNumberAndColor()
    {
        if (id < 56)
        {
            int row = (id / 14) % 4;
            Color = row == 0 ? CardType.Red : row == 1 ? CardType.Yellow : row == 2 ? CardType.Green : CardType.Blue;
            Number = id % 14;

            if (Number > 9)
                Type = (SpecialCard)Number;
        }
        else
        {
            int id2 = id - 56;
            int row = (id2 / 13) % 4;
            Color = row == 0 ? CardType.Red : row == 1 ? CardType.Yellow : row == 2 ? CardType.Green : CardType.Blue;

            Number = (id2 % 13) + 1;
            if (Number > 9)
                Type = Number == 13 ? SpecialCard.Take4 : (SpecialCard)Number;
        }

       // DebugControl.Log(id+":"+ Number + " "+Color.ToString()+" "+Type,3);
    }
    public bool AcceptsCard(UnoCard card)
    {
        //DebugControl.Log(Type + " " + card.Type, 3);
        if (card.Type == SpecialCard.Wild|| card.Type == SpecialCard.Take4)
            return true;

        return (card.Number == Number || card.Color == Color);
    }
}
