using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCard : MonoBehaviour
{
    public enum CardType{
        Red,
        Green,
        Blue,
        Yellow,
        WildCard

    }
    public int id;
    public CardType type;
    public List<Sprite> CardSprites;
   // public Sprite sprite;
    // Start is called before the first frame update
  public void setID(int id)
    {
        this.id = id;
        Sprite sprite = GetComponent<Sprite>();
        if (sprite != null)
            if(CardSprites != null)
        sprite = CardSprites[id];
    }
}
