using Player;
using UnityEngine;

namespace Managers
{


    public class RunManager : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private SwerveInputController swerveInput;
        public static RunManager Instance { get; private set; }
        public bool isRun;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);


            IsRunStart = false;
        }

        public void StartGame()
        {
            playerController.StartMove = true;
            CanvasManager.Instance.StartTouch = false;
            AnimationManager.Instance.PlayerRun = true;
        }

        public void HitObstacle()
        {
            IsRunStart = false;
            AnimationManager.Instance.PlayerRun = false;
            CanvasManager.Instance.StartTouch = true;
            playerController.TurnBackFirstPos();
            AnimationManager.Instance.PlayerRun = false;
        }

        public void StopPlayer()
        {
            playerController.StartMove = false;
            AnimationManager.Instance.PlayerRun = false;
        }

        public bool IsRunStart
        {
            get => isRun;
            set
            {
                isRun = value;
                swerveInput.Running = isRun;
                playerController.StartMove = isRun;
            }
        }

        public bool SwerveInput
        {
            get => swerveInput;
            set => swerveInput.enabled = value;
        }
    }
}