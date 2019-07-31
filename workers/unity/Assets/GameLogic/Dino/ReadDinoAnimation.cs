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
        private Animator animator; // 恐龙的动画，直接修改动画的播放
        private DinoFSMState.StateEnum _lastStatus = DinoFSMState.StateEnum.IDLE;
        
        [Require] private DinoBrachioReader dinoReader; // 恐龙的状态

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
            animator.SetBool(animationBool[(int)inStatus], true);
            _lastStatus = inStatus;
        }
    }
}