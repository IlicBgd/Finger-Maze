using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HelpWindow : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;
    [Header("Fade Duration")]
    [SerializeField]
    float fadeIn = 0.15f;
    [SerializeField]
    float fadeOut = 0.1f;
    [SerializeField]
    CanvasGroup canvasGroup;

    bool isOpen;

    void OnValidate()
    {
        TryGetComponent(out canvasGroup);
    }
    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(Fade(true));
        }
    }
    public void Close()
    {
        if (isOpen)
        {
            StartCoroutine(Fade(false));
        }
    }
    public IEnumerator Fade(bool isOpen)
    {
        float time = 0f;
        float start = canvasGroup.alpha;
        float duration = isOpen ? fadeIn : fadeOut;
        float target = isOpen ? 1 : 0;

        this.isOpen = isOpen;

        canvasGroup.blocksRaycasts = isOpen;
        canvasGroup.interactable = isOpen;

        while (time < 1)
        {
            time += Time.deltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(start, target, curve.Evaluate(time));
            yield return null;
        }
    }
    public void ForceFade(bool isOpen)
    {
        float target = isOpen ? 1 : 0;
        canvasGroup.alpha = target;
    }
}
