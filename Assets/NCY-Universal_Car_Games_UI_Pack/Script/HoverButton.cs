using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UCGUP;
namespace UCGUP
{
    public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Vector3 originalScale;
        public Vector3 hoverScale = new Vector3(1.03f, 1.03f, 1.03f);
        AudioSource _hoverSound, _click_Sound;

        void Start()
        {
            originalScale = transform.localScale;

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = hoverScale;
            if (_hoverSound)
                _hoverSound.Play();
            if (GameManager._instance._cursorIcon)
                Cursor.SetCursor(GameManager._instance._cursorIcon, Vector2.zero, CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = originalScale;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        }
        public void OnPointerClick(PointerEventData eventData)
        {

            if (_click_Sound != null)
                _click_Sound.Play();

        }
    }
}
