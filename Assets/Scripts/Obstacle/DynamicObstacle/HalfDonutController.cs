using UnityEngine;

namespace Obstacle.DynamicObstacle
{
    public class HalfDonutController : MonoBehaviour
    {
        [SerializeField] private float speed,minX, maxX;
        [SerializeField] private Vector3 newPos;
        [SerializeField] private Rigidbody rigidDonut;


        private void Awake()
        {
        rigidDonut = GetComponent<Rigidbody>();
        newPos = Vector3.right;
        }

        private void Update()
        {
            SetNewPos();
        }

        private void FixedUpdate()
        {
            MoveDonut();
            SetNewPos();
        }

        private void SetNewPos()
        {
            var pos = rigidDonut.position;
            
            if (pos.x >= maxX)
            {
                newPos = -Vector3.right;
            }

            if (pos.x <= minX)
            {
                newPos = Vector3.right;
            }
        }

        private void MoveDonut()
        {
            rigidDonut.MovePosition(rigidDonut.position + newPos * Time.fixedDeltaTime * speed);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")||other.gameObject.CompareTag("Opponent"))
            {
                var contact =other.GetContact(0).point;
                other.gameObject.GetComponent<Rigidbody>().AddForce(-contact.x * rigidDonut.mass, 0f, 0f);
            }
        }
    }
}