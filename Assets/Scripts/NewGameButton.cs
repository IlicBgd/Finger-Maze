using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NewGameButton : HelpWindowButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        helpWindow.Close();
        if (this.name == "YesRing")
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}

