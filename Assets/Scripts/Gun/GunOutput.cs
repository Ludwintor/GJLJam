using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class GunOutput : MonoBehaviour
    {
        private const string defaultFlashName = "pistolFlash";

        [SerializeField]
        private Animator muzzleFlash;

        private AnimatorOverrideController animator;

        private void Start()
        {
            muzzleFlash.gameObject.SetActive(false);

            animator = new AnimatorOverrideController(muzzleFlash.runtimeAnimatorController);
            muzzleFlash.runtimeAnimatorController = animator;
        }

        public void Flash(AnimationClip muzzle)
        {
            if (muzzle == null)
                return;

            animator[defaultFlashName] = muzzle;
            StartCoroutine(HandleFlash());
        }

        private IEnumerator HandleFlash()
        {
            muzzleFlash.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            muzzleFlash.gameObject.SetActive(false);
        }
    }
}