using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoColorSelect : MonoBehaviour
{
    public static UnoCard.CardType ColorSelected = UnoCard.CardType.Red;

    private void Start()
    {
        ColorSelected = UnoCard.CardType.Red;
    }

    public void SelectBlueColor(bool isOn)
    {
        if(isOn)
            ColorSelected = UnoCard.CardType.Blue;
    }
    public void SelectRedColor(bool isOn)
    {
        if (isOn)
            ColorSelected = UnoCard.CardType.Red;
    }
    public void SelectGreenColor(bool isOn)
    {
        if (isOn)
            ColorSelected = UnoCard.CardType.Green;
    }
    public void SelectYelloColor(bool isOn)
    {
        if (isOn)
            ColorSelected = UnoCard.CardType.Yellow;
    }
}
