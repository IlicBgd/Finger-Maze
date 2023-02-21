using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : HelpWindowButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        helpWindow.Close();
    }
}
