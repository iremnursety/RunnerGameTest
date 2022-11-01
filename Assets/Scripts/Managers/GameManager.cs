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

        public void FirstStart() //For Game First Start and active Timer.
        {
            
            if (gameStarted) return;
            gameStarted = true;
            CountManager.Instance.TimerStart = true;

        }

        public void GameOver() //Stop everything except obstacles. Finalize game for see leaderboard.
        {
            
            gameStarted = false;
            
            RunManager.Instance.IsRunStart = false;
            AnimationManager.Instance.PlayerRun = false;
            CountManager.Instance.TimerStart = false;
            CanvasManager.Instance.StartTouch = false;

        }
    }
}