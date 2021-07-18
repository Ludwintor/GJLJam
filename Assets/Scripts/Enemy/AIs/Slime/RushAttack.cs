using UnityEngine;

namespace GJLJam
{
    public class RushAttack : IState
    {
        public Vector2 DesiredVelocity { get; private set; }
        public float LastRushTime { get; private set; }

        private readonly Enemy enemy;
        private readonly Rigidbody2D rb;
        private readonly float movementBoost;
        private readonly float velocityDrag;
        private static readonly int speedHash = Animator.StringToHash("speed");

        public RushAttack(Enemy enemy, Rigidbody2D rb, float movementBoost, float velocityDrag)
        {
            this.enemy = enemy;
            this.rb = rb;
            this.movementBoost = movementBoost;
            this.velocityDrag = velocityDrag;
        }

        public void OnEnter()
        {
            enemy.Animator.SetFloat(speedHash, 1f);
            Vector2 direction = ((Vector2)enemy.Target.position - rb.position).normalized;
            rb.velocity = DesiredVelocity = direction * (enemy.Stats.MovementSpeed + movementBoost);
            LastRushTime = Time.time;
        }

        public void OnExit()
        {
            enemy.Animator.SetFloat(speedHash, 0f);
            rb.velocity = Vector2.zero;
        }

        public void Tick()
        {
            float multiplier = 1f - velocityDrag * Time.fixedDeltaTime;
            if (multiplier < 0f) multiplier = 0f;

            DesiredVelocity *= multiplier;
            rb.velocity = DesiredVelocity;
        }
    }
}
