using Dinopark.Npc;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public class WriteDinoAnimation : MonoBehaviour
    {
        [Require] private DinoBrachioWriter dinoWriter; // 恐龙的状态

        private Animator _animator; // 恐龙的动画，直接修改动画的播放
        [SerializeField] private DinoFSMState.StateEnum _lastStatus = DinoFSMState.StateEnum.NONE;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.applyRootMotion = false;
            _lastStatus = DinoFSMState.StateEnum.NONE;
        }

        // Update is called once per frame
        void Update()
        {
            var status = GetStatus();
            if (status == _lastStatus || status == DinoFSMState.StateEnum.NONE)
                return;
            var update = new DinoBrachio.Update()
            {
                CurrentState =  status
            };
            dinoWriter.SendUpdate(update);
            _lastStatus = status;
        }

        DinoFSMState.StateEnum GetStatus()
        {
            string[] animationBool = { "isEating", "isWalking", "isRunning", "isAttacking", "isDead"};
            int count = 0;
            foreach (var ani in animationBool)
            {
                var isPlaying = _animator.GetBool(ani);
                if (isPlaying)
                    count++;
            }

            if (count > 1)
                Debug.LogError("Playing Count is greater than 1");

            int index = 0;
            foreach (var ani in animationBool)
            {
                var isPlaying = _animator.GetBool(ani);
                if (isPlaying)
                {
                    return (DinoFSMState.StateEnum)index;
                }

                index++;
            }

            return DinoFSMState.StateEnum.NONE;
        }
    }
}