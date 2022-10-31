using UnityEngine;

namespace Managers
{
   public class AnimationManager : MonoBehaviour
   {
      public static AnimationManager Instance { get; private  set; }

      [SerializeField] private Animator playerAnim;
      [SerializeField] private bool player;
      private static readonly int İsPlayerRunning = Animator.StringToHash("isPlayerRunning");
      

      private void Awake()
      {
         if (Instance == null)
            Instance = this;
         else
            Destroy(gameObject);
      }

      public bool PlayerRun
      {
         get => player;
         set
         {
            player = value;
            playerAnim.SetBool(İsPlayerRunning, player);
         }
      }
   }
}
