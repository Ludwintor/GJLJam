using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class ForestHorror : Enemy
    {
        private SlashAttack slashAttack;
        private HookThrow hookThrow;

        private ChaseTarget chasing;
        private Attack attack;
        private Hooking hooking;

        [SerializeField]
        private float attackDistance;
        [SerializeField]
        private float delay;

        protected override void Awake()
        {
            base.Awake();

            slashAttack = GetComponent<SlashAttack>();
            hookThrow = GetComponent<HookThrow>();

            chasing = new ChaseTarget(this, Rigidbody, Seeker);
            attack = new Attack(this, slashAttack);
            hooking = new Hooking(this, hookThrow, delay);

            StateMachine.AddTransition(chasing, attack, () => IsTargetNear());
            StateMachine.AddTransition(attack, chasing, () => !IsTargetNear());
            StateMachine.AddTransition(chasing, hooking, () => hookThrow.CanHook && IsInHookDistance());
            StateMachine.AddTransition(hooking, chasing, () => !hookThrow.CanHook);


            StateMachine.SetState(chasing);
        }

        private bool IsTargetNear() => Target != null && ((Vector2)Target.position - Rigidbody.position).sqrMagnitude <= attackDistance * attackDistance;
        private bool IsInHookDistance()
        {
            float sqrDistance = ((Vector2)Target.position - Rigidbody.position).sqrMagnitude;

            return sqrDistance <= hookThrow.HookDistance * hookThrow.HookDistance && sqrDistance > hookThrow.MinHookDistance * hookThrow.MinHookDistance;
        } 
    }
}
