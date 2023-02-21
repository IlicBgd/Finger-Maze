using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenButton : HelpWindowButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        helpWindow.Open();
    }
}
