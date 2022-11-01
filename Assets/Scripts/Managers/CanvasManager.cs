using UnityEngine;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }

        [SerializeField] private Canvas startTouch;
        [SerializeField] private Canvas whiteBoard;
        [SerializeField] private Canvas gameCounts;
        [SerializeField] private Canvas leaderBoard;
        [SerializeField] private Canvas noWhiteBoard;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            StartTouch = true;
            GameCounts = true;
            LeaderBoard = true;
            WhiteBoard = false;
            NoWhiteBoard = false;
        }

        public void FinishLine() //After passing Finish Line.
        {
            StartTouch = false;
            //LeaderBoard = true;
        }

        public bool StartTouch //Tap to Run Canvas.
        {
            get => startTouch;
            set => startTouch.enabled = value;
        }

        public bool WhiteBoard //White Board Canvas.
        {
            get => whiteBoard;
            set => whiteBoard.enabled = value;
        }
        public bool NoWhiteBoard //White Board Canvas.
        {
            get => noWhiteBoard;
            set => noWhiteBoard.enabled = value;
        }

        private bool GameCounts //Timer, Hit Obstacle Count.
        {
            get => gameCounts;
            set => gameCounts.enabled = value;
        }

        private bool LeaderBoard //Leaderboard Canvas.
        {
            get => leaderBoard;
            set => leaderBoard.enabled = value;
        }
    }
}