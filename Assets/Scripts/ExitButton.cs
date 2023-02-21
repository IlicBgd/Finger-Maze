using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitButton : BaseButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.name == "QuitGameButton")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
