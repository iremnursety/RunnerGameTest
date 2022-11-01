using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class LeaderboardManager : MonoBehaviour
    {
        //TODO:Turn Leaderboard to realtime.
        public static LeaderboardManager Instance { get; private set; }
        [SerializeField] private List<TextMeshProUGUI> textMPs = new List<TextMeshProUGUI>();
        public List<GameObject> rankedList;
        public List<RankController> tempRank = new List<RankController>();
        [SerializeField] private int playerRank;
        public int indexer;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            indexer = 0;
        }

        public void RankList(List<RankController> rankCont)
        {
            tempRank = rankCont;
            ChangeList();
        }

        private void ChangeList()
        {
            for (var i = 0 + rankedList.Count; i < tempRank.Count; i++)
            {
                if (rankedList.Contains(textMPs[i].gameObject)) continue;
                textMPs[i].color = Color.white;
                textMPs[i].fontStyle = FontStyles.Normal;
                if (tempRank[i].transform.CompareTag("Player"))
                {
                    textMPs[i].color = Color.yellow;
                    textMPs[i].fontStyle = FontStyles.Bold;
                }

                textMPs[i].text = (i + 1) + ". " + tempRank[i].name;
            }
        }

        public void FinishLinePassed(GameObject obj)
        {
            if (rankedList.Contains(obj)) return;
            rankedList.Add(obj);
        }

        public void FinalizeGame() //Finalizing Game(Time out / Passing Finish Line)
        {
            for (var i = 0; i < tempRank.Count; i++)
            {
                if (tempRank[i].CompareTag("Player"))
                {
                    playerRank = i + 1;
                }
            }
        }

        public void IsInRange()
        {
            if (playerRank <= 3)
            {
                CanvasManager.Instance.WhiteBoard = true;
                GameManager.Instance.GameOver();
                CameraManager.Instance.FocusBoard();
            }
            else
            {
                CanvasManager.Instance.NoWhiteBoard = true;
                GameManager.Instance.GameOver();
            }
        }

        public void TextRegister(int index, TextMeshProUGUI textMesh)
        {
            textMPs[index] = textMesh;
        }
    }
}