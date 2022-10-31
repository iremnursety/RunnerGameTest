using Managers;
using Opponent;
using UnityEngine;

namespace Obstacle.StaticObstacle
{
    public class ObstacleController : MonoBehaviour
    {
       private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                PlayerTurnBack();
            if (other.CompareTag("Opponent"))
                other.GetComponent<OpponentController>().BackToStart();
        }

        private static void PlayerTurnBack()
        {
            RunManager.Instance.HitObstacle();
        }
    }
}
