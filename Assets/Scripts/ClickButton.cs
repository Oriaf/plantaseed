using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
  
    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        
    }

    // Update is called once per frame
    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;
        
    }
}
