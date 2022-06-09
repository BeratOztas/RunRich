using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;



    public class SimpleAnimancer : MonoBehaviour
    {
        [SerializeField]
        private AnimancerComponent animancer;
        [SerializeField] private float fadeDuration = 0.1f;
        [SerializeField]
        private AnimationClip[] clips;
        [SerializeField]
        private bool playDefault = false;

        private AnimancerState currentState;

        void Start()
        {
            if (playDefault)
            {
                PlayAnimation(clips[0].name);
            }
        }
        public void PlayAnimation(string clipName)
        {

            AnimationClip clip = GetAnimationClipName(clipName);
            if (animancer != null && clip != null)
            {
                currentState = animancer.Play(clip, fadeDuration);
            }

        }

        public void PlayAnimation(AnimationClip clip)
        {
            if (animancer != null && clip != null)
            {
                currentState = animancer.Play(clip, fadeDuration);
            }
        }
        public void PlayMixer(LinearMixerTransition transition, float speed)
        {
            currentState = animancer.Play(transition, fadeDuration);
        }
        public void SetStateSpeed(float speed)
        {
            if (currentState == null)
                return;
            currentState.Speed = speed;
        }


        AnimationClip GetAnimationClipName(string clipName)
        {
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name.Equals(clipName))
                {
                    return clips[i];
                }

            }
            return null;
        }

        public Transform GetAnimatiorTransform()
        {
            return animancer.transform;
        }

    }


