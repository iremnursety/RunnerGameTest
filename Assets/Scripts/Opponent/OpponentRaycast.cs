using Managers;
using UnityEngine;

namespace Opponent
{
    public class OpponentRaycast : MonoBehaviour
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private int rayNum;
        [SerializeField] private LayerMask layer;
        [SerializeField] private OpponentController opponentCont;
        [SerializeField] private bool isGameStart;
        private float _angle = 90f;

        private void Update()
        {
            isGameStart = GameManager.Instance.gameStarted;
            if (isGameStart)
                RayAround();
        }

        private void RayAround()
        {
            for (int i = 0; i < rayNum; i++)
            {
                var rotation = transform.rotation;
                var rotationMode = Quaternion.AngleAxis(((i / ((float) rayNum - 1)) * _angle * 2 - _angle), transform.up);
                var direction = rotation * rotationMode * Vector3.forward;

                var ray = new Ray(transform.position, direction);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, maxDistance, layer))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(direction * maxDistance), Color.red);

                    if (hitInfo.distance <= 6f)
                    {
                        opponentCont.Dodge(ray.direction);
                    }
                }
            }
        }
    }
}