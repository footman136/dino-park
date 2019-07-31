using Assets.Gamelogic.Utils;
using Com.Infalliblecode;
using Dinopark.Npc;
using Improbable;
using Improbable.Gdk.Subscriptions;
using LowPolyAnimalPack;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public class WriteDinoAnimation : MonoBehaviour
    {
        [Require] private DinoBrachioWriter dinoWriter; // 恐龙的状态

        private Animator animator; // 恐龙的动画，直接修改动画的播放
        private DinoFSMState.StateEnum _lastStatus;

        // Update is called once per frame
        void Update()
        {
            var update3 = new DinoBrachio.Update()
            {
                CurrentState = GetStatus()
            };
            dinoWriter.SendUpdate(update3);
        }

        DinoFSMState.StateEnum GetStatus()
        {
            string[] animationBool = { "isEating", "isWalking", "isRunning", "isAttacking", "isDead"};
            int index = 0;
            foreach (var ani in animationBool)
            {
                var isPlaying = animator.GetBool(ani);
                if (isPlaying && index != (int) _lastStatus)
                {
                    return (DinoFSMState.StateEnum)index;
                }

                index++;
            }

            return DinoFSMState.StateEnum.NONE;
        }
    }
}