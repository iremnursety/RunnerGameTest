using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool gameStarted;
        

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            gameStarted = false;
        }

        public void FirstStart()
        {
            if (gameStarted) return;
            gameStarted = true;
            CountManager.Instance.TimerStart = true;

        }

        public void GameOver()
        {
            RunManager.Instance.SwerveInput = false;
            LeaderboardManager.Instance.FinishGame();
            RunManager.Instance.StopPlayer();
            CountManager.Instance.TimerStart = false;
            CanvasManager.Instance.StartTouch = false;
            gameStarted = false;
        }
    }
}