using System.Collections.Generic;
using Opponent;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class LeaderboardManager : MonoBehaviour
    {
        //TODO:Turn Leaderboard to realtime.
        public static LeaderboardManager Instance { get; private set; }
        [SerializeField] private List<GameObject> rankedList = new List<GameObject>(11);
        [SerializeField] private List<TextMeshProUGUI> textMPs = new List<TextMeshProUGUI>(11);
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            textMPs.Capacity = 20;
        }
        
        public void Leaderboard(GameObject player)
        {
            if (rankedList.Contains(player)) return;
            rankedList.Add(player);
            var tempIndex = rankedList.Count - 1;
            if (player.CompareTag("Player"))
            {
                textMPs[tempIndex].color = Color.yellow;
                textMPs[tempIndex].fontStyle = FontStyles.Bold;
            }
            if(player.CompareTag("Opponent"))
                player.GetComponent<OpponentController>().isRunning = false;
            textMPs[tempIndex].text = (tempIndex+1)+". "+rankedList[tempIndex].name;
        }

        public void FinalizeGame()
        {
            CanvasManager.Instance.FinishLine();
            if (rankedList.Count > 0)
            {
                for (var i = 0; i < rankedList.Count; i++)
                {
                    if (rankedList[i].CompareTag("Player"))
                    {
                        textMPs[i].color = Color.yellow;
                        textMPs[i].fontStyle = FontStyles.Bold;
                        textMPs[i].text = (i + 1)+". "+rankedList[i].name;
                    }
                    else if(rankedList[i].CompareTag("Opponent"))
                        textMPs[i].text = (i + 1) + ". " + rankedList[i].name;
                }
            }
        }

        public void TextRegister(int index,TextMeshProUGUI textMesh)
        {
            textMPs[index] = textMesh;
        }
    }
}
