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

        public void StartGame() //For the Start game or after hit obstacle for start running.
        {
           
            playerController.StartMove = true;
            CanvasManager.Instance.StartTouch = false;
            AnimationManager.Instance.PlayerRun = true;
            IsRunStart = true;
            GameManager.Instance.FirstStart();
        }

        public void HitObstacle() //Reset Player Position to First Position and then Show Tap To Run.
        {
            playerController.BackToStartPlayer();
            CountManager.Instance.HitObstacle();
            if (IsRunStart == false) return;
            IsRunStart = false;
            AnimationManager.Instance.PlayerRun = false;
            CanvasManager.Instance.StartTouch = true;

        }
    
        public bool IsRunStart //For Manage Run and Swerve.
        {
            
            get => isRun;
            set
            {
                isRun = value;
                swerveInput.Running = isRun;
                playerController.StartMove = isRun;
            }
        }

        public bool SwerveInput // Manage Player Swerve.
        {
            get => swerveInput;
            set
            {
                swerveInput.enabled = value;
                swerveInput.lastPosX = 0;
                swerveInput.movePos = 0;
            }
        }
    }
}