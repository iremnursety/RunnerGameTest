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
            Debug.Log(other);
            if (other.gameObject.CompareTag("Player"))
            {
                playerPassed = true;
                RunManager.Instance.SwerveInput = false;
                LeaderboardManager.Instance.Leaderboard(other.gameObject);
            }

            if (other.gameObject.CompareTag("Opponent"))
            {
                other.GetComponent<OpponentController>().SetAnimation = false;
                LeaderboardManager.Instance.Leaderboard(other.gameObject);
            }
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
                    LoadBoard();
                    playerPassed = false;
                }
            }
        }

        private void LoadBoard()
        {
            CanvasManager.Instance.WhiteBoard = true;
            GameManager.Instance.GameOver();
            CameraManager.Instance.FocusBoard();
            
        }
    }
}