using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class StartTouchController : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            //For the Start game or after hit obstacle for start running.
            RunManager.Instance.StartGame();
            RunManager.Instance.IsRunStart = true;
            GameManager.Instance.FirstStart();
            Debug.Log("Start Touch On Pointer Click");
        }
    }
}