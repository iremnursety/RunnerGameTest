using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Opponent
{
    public class OpponentController : MonoBehaviour
    {
        [SerializeField] private Vector3 firstPos, lastPos;
        [SerializeField] private bool startMove;
        [SerializeField] private NavMeshAgent opponentNav;
        [SerializeField] private Rigidbody rigidOpponent;
        [SerializeField] private float speed;
        private void Awake()
        {
            firstPos = transform.position;
            rigidOpponent = GetComponent<Rigidbody>();
            opponentNav = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            startMove = GameManager.Instance.gameStarted;
            if (startMove == true)
                Move();
        }

        public void BackToStart()
        {
            rigidOpponent.transform.position = firstPos;
        }
        public bool StartMove
        {
            get => startMove;
            set => startMove = value;
        }

        private void Move()
        {
            var move = Time.deltaTime * speed * transform.forward;
            rigidOpponent.MovePosition(rigidOpponent.position + move);
        }
    }
}
