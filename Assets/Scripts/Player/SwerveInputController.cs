using UnityEngine;

namespace Player
{
    public class SwerveInputController : MonoBehaviour
    {
        public float lastPosX,movePos;
        [SerializeField] private bool isRunStart;
        [SerializeField] private PlayerController playerCont;

        private void Awake()
        {
            playerCont = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (isRunStart != true)
            {
                movePos = 0f;
                playerCont.Horizontal = movePos;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastPosX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButton(0))
                {
                    movePos = Input.mousePosition.x - lastPosX;
                    playerCont.Horizontal = movePos;
                    lastPosX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    movePos = 0f;
                    playerCont.Horizontal = movePos;
                }
            }
        }

        public bool Running
        {
            get => isRunStart;
            set => isRunStart = value;
        }
    }
}