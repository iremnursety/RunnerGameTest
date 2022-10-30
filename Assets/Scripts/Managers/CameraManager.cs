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
        [SerializeField] private CinemachineTransposer transposer;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            transposer = cineMachine.GetCinemachineComponent<CinemachineTransposer>();
        }

        public void ResetFocus()
        {
            cineMachine.LookAt = player;
            transposer.m_FollowOffset.z = -9f;
        }
        public void FocusBoard()
        {
            cineMachine.LookAt = whiteBoard;
            cineMachine.Follow = whiteBoard;
            transposer.m_FollowOffset.z = -14f;
        }

        // private void ChangeCameraPos()
        // {
        //     Debug.Log(player.position.x <= 0 ? "Move Camera Right" : "Move Camera Left");
        //     
        // }
        
        
        
    }
}