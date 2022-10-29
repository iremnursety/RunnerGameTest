using UnityEngine;

namespace Obstacle.DynamicObstacle
{
    public class ShiningObstacleController : MonoBehaviour
    {
        private int rotationSide;
        private Rigidbody rigShining;
        private Vector3 newPos,eulerAngleVelocity;
        [SerializeField] private float speed,rotationSpeed,maxX,minX;
        [SerializeField] private bool isRight;
        [SerializeField] private ParticleSystem particleSys;
        [SerializeField] private Color color;
        private void Awake()
        {
            if (isRight == false)
                rotationSide = -1;
            else
                rotationSide = 1;
            
            rigShining = GetComponent<Rigidbody>();
            eulerAngleVelocity = new Vector3(0f,rotationSide * rotationSpeed,0f);
            color=Color.magenta;

        }

        private void Update()
        {
            SetNewPos();
        }

        private void SwitchColor()
        {
            var main = particleSys.main;
            color  = color == Color.magenta ? Color.cyan : Color.magenta;
            main.startColor = color;
        }
        private void FixedUpdate()
        {
            ShiningRotation();
            ShiningMovement();
        }
        
        private void ShiningRotation()
        {
            var rotation = Quaternion.Euler(eulerAngleVelocity*Time.fixedDeltaTime);
            rigShining.MoveRotation(rigShining.rotation*rotation);
        }

        private void ShiningMovement()
        {
            rigShining.MovePosition(rigShining.position+newPos*Time.fixedDeltaTime);
        }

        private void SetNewPos()
        {
            if (rigShining.position.x >= maxX)
                newPos = new Vector3(minX*speed, 0f, 0f);
            if(rigShining.position.x <=minX)
                newPos = new Vector3(maxX*speed, 0f, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                SwitchColor();
        }

    }
}
