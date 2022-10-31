using Managers;
using UnityEngine;

namespace Opponent
{
    public class OpponentController : MonoBehaviour
    {
        [SerializeField] private Vector3 firstPos;
        [SerializeField] private bool startMove;
        [SerializeField] private Rigidbody rigidOpponent;
        [SerializeField] private float speed, dodgeSpeed, reverse;
        public Animator opponentAnim;
        public bool isRunning, animBool;
        private static readonly int OpponentRunning = Animator.StringToHash("opponentRunning");

        private void Awake()
        {
            firstPos = transform.position;
            rigidOpponent = GetComponent<Rigidbody>();
            isRunning = true;
        }

        private void Update()
        {
            startMove = GameManager.Instance.gameStarted;
            if (startMove) return;
            if (opponentAnim.GetBool(OpponentRunning))
                opponentAnim.SetBool(OpponentRunning, false);
        }

        private void FixedUpdate()
        {
            if (startMove && isRunning)
            {
                Move();
                if (opponentAnim.GetBool(OpponentRunning) != true)
                    opponentAnim.SetBool(OpponentRunning, true);
            }
            ExtraGravity();
        }

        public bool SetAnimation
        {
            get => animBool;
            set => opponentAnim.SetBool(OpponentRunning, value);
        }

        public void BackToStart()
        {
            rigidOpponent.velocity = Vector3.zero;
            rigidOpponent.transform.position = firstPos;
        }

        private void Move()
        {
            var move = Time.deltaTime * speed * transform.forward;
            rigidOpponent.MovePosition(rigidOpponent.position + move);
        }

        public void Dodge(Vector3 direction)
        {
            //var direction = new Vector3(-obstacle.x * dodgeSpeed * Time.fixedDeltaTime, 0, 0);
            //var direction2 = new Vector3(-obstacle.x*dodgeSpeed*Time.unscaledDeltaTime, 0, 0);
            //rigidOpponent.MovePosition(direction2);

            // if(direction.x >0)
            //     rigidOpponent.AddForce(-obstacle.x*dodgeSpeed,0,0);
            // else
            //     rigidOpponent.AddForce(obstacle.x*dodgeSpeed,0,0);

            if (direction.x < 0)
                reverse = 1;
            else
                reverse = -1;

            rigidOpponent.AddForce(transform.right * (reverse * dodgeSpeed));
            rigidOpponent.velocity = Vector3.zero;
        }

        private void ExtraGravity()
        {
            if (rigidOpponent.velocity.y < 0)
                rigidOpponent.AddForce(-transform.up);
            //rigidOpponent.velocity = new Vector3(0,-10,0);
        }
    }
}