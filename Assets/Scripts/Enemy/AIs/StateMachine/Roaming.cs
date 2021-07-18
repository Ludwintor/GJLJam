using Ludwintor.Tools;
using UnityEngine;

namespace GJLJam
{
    public class Roaming : IState
    {
        private readonly Enemy enemy;
        private readonly Rigidbody2D rb;
        private readonly float minDistance;
        private readonly float maxDistance;
        private static readonly int moveXHash = Animator.StringToHash("moveX");
        private static readonly int moveYHash = Animator.StringToHash("moveY");
        private static readonly int speedHash = Animator.StringToHash("speed");

        private Vector2 moveTo;

        public Roaming(Enemy enemy, Rigidbody2D rb, float minDistance, float maxDistance)
        {
            this.enemy = enemy;
            this.rb = rb;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
        }
        
        public void OnEnter()
        {
            moveTo = GetRoamingPosition();
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
            Vector2 difference = moveTo - rb.position;
            Vector2 direction = difference.normalized;
            rb.velocity = direction * enemy.Stats.MovementSpeed;
            enemy.Animator.SetFloat(moveXHash, direction.x);
            enemy.Animator.SetFloat(moveYHash, direction.y);
            enemy.Animator.SetFloat(speedHash, direction.magnitude);

            if (difference.sqrMagnitude <= 1)
                moveTo = GetRoamingPosition();
        }

        private Vector2 GetRoamingPosition() => rb.position + LUtils.GetRandomDirection() * Random.Range(minDistance, maxDistance);
    }
}
