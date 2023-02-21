using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LayerMask layerMask;
    [SerializeField]
    GameObject exitArrow;
    [SerializeField]
    HelpWindow endGameWindow;
    [SerializeField]
    AudioSource endgameSound;

    bool isDragged;
    private void Update()
    {
        if (isDragged)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            position.z = 0;
            if (Physics2D.CircleCast(transform.position, transform.localScale.x / 2, (position - transform.position).normalized, (position - transform.position).magnitude, layerMask))
            {
                return;
            }
            transform.position = position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == exitArrow)
        {
            this.gameObject.SetActive(false);
            endGameWindow.Open();
            endgameSound.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragged = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragged = false;
    }
}