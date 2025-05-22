using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Test : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    public float requiredHoldTime;

    public UnityEvent onLongClick;

    [SerializeField] private Image fillImage;
    [SerializeField] private Image fillImage2;
    [SerializeField] private Image fillImage3;
    [SerializeField] private Image fillImage4;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;        
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        Reset();
    }

    private void Update()
    {
        if(pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if(pointerDownTimer >= requiredHoldTime)
            {
                if(onLongClick != null)
                {
                    onLongClick.Invoke();
                }

                //Executa a ação
                Reset();
            }

            fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
            fillImage2.fillAmount = pointerDownTimer / requiredHoldTime;
            fillImage3.fillAmount = pointerDownTimer / requiredHoldTime;
            fillImage4.fillAmount = pointerDownTimer / requiredHoldTime;
        }
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
        fillImage2.fillAmount = pointerDownTimer / requiredHoldTime;
        fillImage3.fillAmount = pointerDownTimer / requiredHoldTime;
        fillImage4.fillAmount = pointerDownTimer / requiredHoldTime;
    }
}
