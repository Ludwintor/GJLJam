using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class PlayerAnimation : MonoBehaviour
    {
        public SpriteRenderer LeftHand => leftHand;
        public SpriteRenderer RightHand => rightHand;

        [SerializeField]
        private SpriteRenderer leftHand;
        [SerializeField]
        private SpriteRenderer rightHand;
        [SerializeField]
        private Animator handsAnimator;

        private Animator animator;

        private static readonly int lookXHash = Animator.StringToHash("lookX");
        private static readonly int lookYHash = Animator.StringToHash("lookY");
        private static readonly int speedHash = Animator.StringToHash("speed");


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void AnimateAim(Vector2 direction)
        {
            animator.SetFloat(lookXHash, direction.x);
            animator.SetFloat(lookYHash, direction.y);
        }

        public void AnimateMovement(Vector2 direction)
        {
            float sqrMagnitude = direction.sqrMagnitude;
            animator.SetFloat(speedHash, sqrMagnitude);
            handsAnimator.SetFloat(speedHash, sqrMagnitude);
        }

        public void ShowHands() => handsAnimator.gameObject.SetActive(true);

        public void HideHands() => handsAnimator.gameObject.SetActive(false);
    }
}
