using TMPro;
using UI;
using UnityEngine;

namespace Managers
{
    public class CountManager : MonoBehaviour
    {
        public static CountManager Instance { get; private set; }

        [SerializeField] private int hitCount;
        [SerializeField] private float gameTimer;
        [SerializeField] private bool firstStart;
        [SerializeField] private HitCountController hitTextCont;
        [SerializeField] private TextMeshProUGUI textMeshTimer;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            hitCount = 0;
            gameTimer = 30f;
        }


        public void HitObstacle()
        {
            hitCount += 1;
            hitTextCont.UpdateCount = hitCount.ToString();
        }

        private void Update()
        {
            if (firstStart)
                Timer();
        }

        public bool TimerStart
        {
            get => firstStart;
            set => firstStart = value;
        }

        private void Timer()
        {
            if (!(gameTimer > 0)) return;
            gameTimer -= Time.deltaTime;
            textMeshTimer.text = Mathf.RoundToInt(gameTimer) + " Sec!";
            if(gameTimer<=0)
                GameManager.Instance.GameOver();
        }

    }
}