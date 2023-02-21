using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MazeLoader : BaseButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.name == "RectangleMazeButton")
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
}
