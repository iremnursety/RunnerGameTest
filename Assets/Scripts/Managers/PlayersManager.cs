using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Managers
{
    public class PlayersManager : MonoBehaviour
    {
        public static PlayersManager Instance { get; private set; }
        public List<RankController> rankControllers;
        public bool finishLine;
        
        
        public RankController GetPlayer(int id)
        {
            return rankControllers.Find(x => x.id == id);
        }
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            finishLine = false;
            Prepare();
        }

        private void Prepare() //Add Players Index and Name.
        {
            var index = 0;
            foreach (var rankController in rankControllers)
            {
                rankController.id = index;
                index++;
                rankController.name = $"Player{index}";
            }
        }

        private void Update()
        {
            if(finishLine!=true)
                UpdateDataScore();
        }

        private void UpdateDataScore()
        {
            rankControllers.Sort((controller, rankController) => controller.Distance.CompareTo(rankController.Distance));
            LeaderboardManager.Instance.RankList(rankControllers);
        }
    }
}
