using UnityEngine;
using UnityEngine.EventSystems;

namespace kbradu
{
    public class ButtonState : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField, ViewOnly] private bool isPressed = false;

        public bool IsPressed => isPressed;
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }
    }
}


