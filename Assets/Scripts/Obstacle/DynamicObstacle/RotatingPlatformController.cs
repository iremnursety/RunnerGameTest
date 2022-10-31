using Opponent;
using UnityEngine;

namespace Obstacle.DynamicObstacle
{
    public class RotatingPlatformController : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Opponent"))
                other.transform.GetComponent<OpponentController>().BackToStart();
        }
    }
}
