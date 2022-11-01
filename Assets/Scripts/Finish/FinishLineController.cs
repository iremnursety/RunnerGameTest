using Managers;
using Opponent;
using UnityEngine;

namespace Finish
{
    public class FinishLineController : MonoBehaviour
    {
        [SerializeField] private bool playerPassed;
        [SerializeField] private float finishLineTimer;

        private void Awake()
        {
            finishLineTimer = 2f;
            playerPassed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) // Player Passed Finish Line
            {
                playerPassed = true;
                RunManager.Instance.SwerveInput = false;
                CanvasManager.Instance.FinishLine();
                LeaderboardManager.Instance.FinalizeGame();
                PlayersManager.Instance.finishLine = true;
                LeaderboardManager.Instance.indexer++;
                
            }

            if (other.gameObject.CompareTag("Opponent")) // Opponent Passed Finish Line
            {
                if (other.GetComponent<OpponentController>().finishPassed) return;
                LeaderboardManager.Instance.indexer++;
                var tempOpponent = other.GetComponent<OpponentController>();
                tempOpponent.SetAnimation = false;
                tempOpponent.isRunning = false;
            }
            
            LeaderboardManager.Instance.FinishLinePassed(other.gameObject);
            // var tempDelete = PlayersManager.Instance.GetPlayer(other.GetComponent<RankController>().id);
            // PlayersManager.Instance.rankControllers.Remove(tempDelete);
            // LeaderboardManager.Instance.tempRank.Remove(tempDelete);
        }

        private void Update()
        {
            if (playerPassed)
                Timer();
        }

        private void Timer()
        {
            if (finishLineTimer > 0)
            {
                finishLineTimer -= Time.deltaTime;
                if (finishLineTimer <= 0)
                {
                    LeaderboardManager.Instance.IsInRange();
                    playerPassed = false;
                }
            }
        }

        // private void LoadBoard()
        // {
        //     CanvasManager.Instance.WhiteBoard = true;
        //     GameManager.Instance.GameOver();
        //     CameraManager.Instance.FocusBoard();
        // }
    }
}