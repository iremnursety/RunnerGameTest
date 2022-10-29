using UnityEngine;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get;  private set; }

        [SerializeField] private Canvas startTouch;
        [SerializeField] private Canvas whiteBoard;
        [SerializeField] private Canvas gameCounts;
        [SerializeField] private Canvas leaderBoard;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            StartTouch  = true;
            GameCounts = true;
            LeaderBoard = false;
            WhiteBoard = false;
        }
        
        public void FinishLine()
        {
            StartTouch = false;
            LeaderBoard = true;
        }

        public bool StartTouch
        {
            get => startTouch;
            set => startTouch.enabled = value;
        }

        public bool WhiteBoard
        {
            get => whiteBoard;
            set => whiteBoard.enabled = value;
        }

        public bool GameCounts
        {
            get => gameCounts;
            set => gameCounts.enabled = value;
        }

        public bool LeaderBoard
        {
            get => leaderBoard;
            set => leaderBoard.enabled = value;
        }
    }
}
