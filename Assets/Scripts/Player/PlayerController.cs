using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigBdy;
        [SerializeField] private Vector3 firstPos;
        [SerializeField] private float speed,swerveSpeed,horizontalX;
        
        public bool startMove;

        private void Awake()
        {
            rigBdy = gameObject.GetComponent<Rigidbody>();
            firstPos = transform.position;
            swerveSpeed = 0.5f;
            }

        private void Update()
        {
            MoveHorizontal();
            if (startMove)
                Move();
            if (rigBdy.velocity.y >= 0)
                ExtraGravity();
        }

        public bool StartMove
        {
            get => startMove;
            set => startMove = value;
        }

        public void TurnBackFirstPos()
        {
            rigBdy.velocity=Vector3.zero;
            rigBdy.position = firstPos;
        }

        private void Move()
        {
            var move = Time.deltaTime * speed * transform.forward;
            rigBdy.MovePosition(rigBdy.position + move);
        }

        private void MoveHorizontal()
        {
            var swerveX = Time.deltaTime * swerveSpeed * horizontalX;
            var swervePos = new Vector3(swerveX, 0f, 0f);
            rigBdy.MovePosition(rigBdy.position + swervePos);
        }

        public float Horizontal
        {
            get => horizontalX;
            set => horizontalX = value;
        }
        private void ExtraGravity()
        {
            // if (rigBdy.velocity.y <= 0)
            //     rigBdy.AddForce(-transform.up*10);
            
            if (rigBdy.velocity.y < 0)
                rigBdy.velocity += new Vector3(0,-10,0);
        }
    }
}