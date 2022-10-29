using Managers;
using UnityEngine;

namespace Obstacle.StaticObstacle
{
    public class ObstacleController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                PlayerTurnBack();
            else if (other.CompareTag("Opponent"))
                OpponentTurnBack();
        }

        private void PlayerTurnBack()
        {
            RunManager.Instance.HitObstacle();
            RunManager.Instance.IsRunStart = false;
            CountManager.Instance.HitObstacle();
        }

        private void OpponentTurnBack()
        {
            //Debug.Log("OpponentTurnBack");
        }
    }
}
