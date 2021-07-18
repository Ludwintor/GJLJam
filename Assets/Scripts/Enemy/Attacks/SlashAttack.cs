using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class SlashAttack : EnemyAttack
    {
        [SerializeField]
        private Transform attackPivot; // This pivot gonna be animated so we can simulate attack

        private AttackCircle attackTrigger;
        private int damage;

        private static readonly int attackHash = Animator.StringToHash("isAttacking");

        protected override void OnAwake()
        {
            attackTrigger = GetComponentInChildren<AttackCircle>(true);
        }

        public void StartAttack(int damage)
        {
            this.damage = damage;
            Enemy.Animator.SetBool(attackHash, true);
        }

        public void StopAttack()
        {
            Enemy.Animator.SetBool(attackHash, false);
        }

        private void TriggerAttack(IHittable player)
        {
            Debug.Log("Slash attack hitted");
            player.Hit(damage);
        }

        private void OnEnable()
        {
            attackTrigger.OnPlayerHit += TriggerAttack;
        }

        private void OnDisable()
        {
            attackTrigger.OnPlayerHit -= TriggerAttack;
        }
    }
}
