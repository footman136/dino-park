using Com.Infalliblecode;
using Dinopark.Npc;
using Improbable.Gdk.Subscriptions; // Require
using UnityEngine;
using Improbable;
using LowPolyAnimalPack;

namespace Assets.Gamelogic.Core
{
    public class ReadDinoAnimation : MonoBehaviour
    {
        [Require] private DinoBrachioReader dinoReader; // 恐龙的状态

        private Animator animator; // 恐龙的动画，直接修改动画的播放
        [SerializeField] private DinoFSMState.StateEnum _lastStatus = DinoFSMState.StateEnum.IDLE;

        void Awake()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = false;
        }

        // Update is called once per frame
        void Update()
        {
            SetStatus(dinoReader.Data.CurrentState);
        }

        void SetStatus(DinoFSMState.StateEnum inStatus)
        {
            if (animator == null)
                return;
            if (_lastStatus == inStatus)
                return;
            string[] animationBool = { "isEating", "isWalking", "isRunning", "isAttacking", "isDead"};
            if (inStatus < 0 || inStatus > DinoFSMState.StateEnum.ON_FIRE)
                return;
            animator.SetBool(animationBool[(int)_lastStatus], false);
            // 如果动画已经是DEAD了，这时候又收到的其他状态的动画，这时候只有rebind动画，否则，动画不能被正确设置，因为一旦被设置到DEAD，就再也回不来了
            // 这种情况会出现在客户端重启，而服务器没有重启的情况，服务器发过来的动画状态会因为上次运行过而发生错乱。
            // 也就是说，上次运行后，本客户端死过，下次运行的时候，服务器仍然会把死亡状态发过来，然后又重置为新的（活的）状态。
            if (_lastStatus == DinoFSMState.StateEnum.DEAD && inStatus != DinoFSMState.StateEnum.DEAD)
            {
                animator.Rebind();
                Debug.Log("Receive old Status<"+_lastStatus+"> new Status<"+inStatus+">");
            }
            animator.SetBool(animationBool[(int)inStatus], true);
            _lastStatus = inStatus;
        }
    }
}