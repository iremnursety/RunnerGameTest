using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class StartTouchController : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            RunManager.Instance.StartGame();
            RunManager.Instance.IsRunStart = true;
            GameManager.Instance.FirstStart();
            Debug.Log("Start Touch On Pointer Click");
        }
    }
}