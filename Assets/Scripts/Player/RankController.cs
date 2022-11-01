using UnityEngine;

namespace Player
{
    public class RankController : MonoBehaviour
    {
        [SerializeField] private Vector3 finishLine;
        public int id;
        public float Distance=>Vector3.Distance(finishLine,transform.position);
        public new string name = "";
    }
}
