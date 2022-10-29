using UnityEngine;

namespace Obstacle.DynamicObstacle
{
    public class RotatorStickController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidStick;

        private void Awake()
        {
            rigidStick = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")||other.gameObject.CompareTag("Opponent"))
            {
                var contact =other.GetContact(0).point;
                other.gameObject.GetComponent<Rigidbody>().AddForce(-contact.x * rigidStick.mass, 0f, 0f);
            }
        }
    }
}
