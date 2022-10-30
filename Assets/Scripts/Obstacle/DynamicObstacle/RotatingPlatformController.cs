using Opponent;
using UnityEngine;

namespace Obstacle.DynamicObstacle
{
    public class RotatingPlatformController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Opponent"))
                other.GetComponent<OpponentController>().BackToStart();
        }
    }
}
