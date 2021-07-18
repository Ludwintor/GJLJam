using Ludwintor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class HookThrow : EnemyAttack
    {
        public bool CanHook => lastHookedTime + hookCooldown <= Time.time;
        public float HookDistance => hookDistance;
        public float MinHookDistance => minHookDistance;

        [SerializeField] 
        private float hookSpeed;
        [SerializeField]
        private float hookDistance;
        [SerializeField]
        private float minHookDistance;
        [SerializeField]
        private float hookCooldown;
        [SerializeField]
        private Hook hook;
        [SerializeField]
        private Transform hookPivot;
        [SerializeField]
        private AudioClip hookThrowSound;

        private AudioSource audioSource;

        private float lastHookedTime;
        private Coroutine throwingRoutine;

        protected override void OnAwake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            hook.gameObject.SetActive(false);
            lastHookedTime = float.MinValue;
        }

        public void StartThrow(Vector2 hookTo, float delay)
        {
            throwingRoutine = StartCoroutine(Throwing(hookTo, delay));
        }

        public void StopHooking()
        {
            if (throwingRoutine == null)
                return;

            if (hook.Hooked != null)
                hook.Hooked.State = PlayerState.Normal;
            hook.ResetHook();
            hook.gameObject.SetActive(false);
            lastHookedTime = Time.time;
            throwingRoutine = null;
        }

        private IEnumerator Throwing(Vector2 hookTo, float delay)
        {
            yield return new WaitForSeconds(delay);
            Vector2 direction = hookTo - (Vector2)Enemy.transform.position;
            float angle = LMath.VectorToAngle(direction);
            int sorting = angle > 0 ? 0 : 2;
            hook.UpdateSorting(sorting);

            audioSource.PlayOneShot(hookThrowSound);

            hook.gameObject.SetActive(true);
            hook.SetHook(0);
            hook.transform.position = hookPivot.transform.position;
            hook.transform.eulerAngles = new Vector3(0f, 0f, angle);

            while (hook.HookSize < hookDistance && hook.Hooked == null)
            {
                hook.SetHook(hook.HookSize + hookSpeed * Time.deltaTime);

                yield return null;
            }

            while (hook.HookSize > 0)
            {
                hook.SetHook(hook.HookSize - hookSpeed * Time.deltaTime);
                if (hook.Hooked != null)
                {
                    Transform hooked = hook.Hooked.transform;
                    Vector2 hookingDirection = (transform.position - hooked.position).normalized;
                    hooked.Translate(hookSpeed * Time.deltaTime * hookingDirection);
                }
                yield return null;
            }

            StopHooking();
        }
    }
}
