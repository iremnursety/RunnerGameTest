using Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        [SerializeField] private CinemachineVirtualCamera cineMachine;
        [SerializeField] private Transform whiteBoard;
        [SerializeField] private Transform player;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
        }

        public void ResetFocus()
        {
            cineMachine.LookAt = player;
        }
        public void FocusBoard()
        {
            cineMachine.LookAt = whiteBoard;
        }

        // private void ChangeCameraPos()
        // {
        //     Debug.Log(player.position.x <= 0 ? "Move Camera Right" : "Move Camera Left");
        //     
        // }
        
        
        
    }
}