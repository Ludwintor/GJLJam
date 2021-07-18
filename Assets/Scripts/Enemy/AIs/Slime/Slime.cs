using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class Slime : Enemy
    {
        private ContactDamage contact;

        private ChaseTarget chasing;
        private RushAttack rushing;
        private Split splitting;

        [SerializeField]
        private float movementBoost;
        [SerializeField]
        private float velocityDrag;
        [SerializeField]
        private float rushRange;
        [SerializeField]
        private float rushCooldown;
        [SerializeField]
        private bool canSplit;
        [SerializeField]
        private GameObject splitTo;
        [SerializeField]
        private int splitCount;
        [SerializeField]
        private float spawnRadius;
        [SerializeField]
        private float splitTime;

        protected override void Awake()
        {
            base.Awake();

            contact = GetComponent<ContactDamage>();
            contact.Damage = Stats.Damage;

            chasing = new ChaseTarget(this, Rigidbody, Seeker);
            rushing = new RushAttack(this, Rigidbody, movementBoost, velocityDrag);
            if (canSplit)
                splitting = new Split(this, splitTo, splitCount, spawnRadius, splitTime);

            StateMachine.AddTransition(chasing, rushing, () => rushing.LastRushTime + rushCooldown <= Time.time && IsTargetNear());
            StateMachine.AddTransition(rushing, chasing, () => rushing.DesiredVelocity.sqrMagnitude <= 0.15f);

            StateMachine.SetState(chasing);
        }

        protected override void OnDeath()
        {
            if (canSplit && !splitting.IsSplitting)
            {
                StateMachine.SetState(splitting);
                StartCoroutine(HandleDeath());
            }
            else if (!canSplit)
            {
                base.OnDeath();
            }
        }

        private IEnumerator HandleDeath()
        {
            yield return new WaitForSeconds(splitTime);
            base.OnDeath();
        }

        private bool IsTargetNear() => Target != null && ((Vector2)Target.position - Rigidbody.position).sqrMagnitude <= rushRange * rushRange;
    }
}
