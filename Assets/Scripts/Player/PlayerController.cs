using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigBdy;
        [SerializeField] private Vector3 firstPos;
        [SerializeField] private float speed, swerveSpeed, horizontalX;

        public bool startMove;

        private void Awake()
        {
            rigBdy = gameObject.GetComponent<Rigidbody>();
            firstPos = transform.position;
            swerveSpeed = 0.5f;
        }

        private void FixedUpdate()
        {
            if (startMove)
            {
                Move();
                MoveHorizontal();
            }

            if (rigBdy.velocity.y >= 0)
                ExtraGravity();
        }

        public bool StartMove //Is player touch on Tap to Run.
        {
            get => startMove;
            set => startMove = value;
        }

        public void BackToStartPlayer() //After hit obstacles.
        {
            rigBdy.velocity = Vector3.zero;
            rigBdy.position = firstPos;
        }

        private void Move() //Move Player.
        {
            var move = Time.fixedDeltaTime * speed * transform.forward;
            rigBdy.MovePosition(rigBdy.position + move);
        }

        private void MoveHorizontal() //Swerve
        {
            var swerveX = Time.deltaTime * swerveSpeed * horizontalX;
            var swervePos = new Vector3(swerveX, 0f, 0f);
            rigBdy.MovePosition(rigBdy.position + swervePos);
        }

        public float Horizontal //Swerve value on X-axis.
        {
            get => horizontalX;
            set => horizontalX = value;
        }

        private void ExtraGravity() //Extra gravity. TODO:Check it!
        {
            if (rigBdy.velocity.y <= 0)
                rigBdy.AddForce(-transform.up * 10);
        }
    }
}