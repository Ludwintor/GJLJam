using Pathfinding;
using UnityEngine;

namespace GJLJam
{
    public class ChaseTarget : IState
    {
        private const float RepathRate = 0.5f;
        private const float NextWaypointDistance = 0.1f;

        private readonly Enemy enemy;
        private readonly Rigidbody2D rb;
        private readonly Seeker seeker;
        private static readonly int moveXHash = Animator.StringToHash("moveX");
        private static readonly int moveYHash = Animator.StringToHash("moveY");
        private static readonly int speedHash = Animator.StringToHash("speed");

        private Path path;
        private float lastRepath;
        private int currentWaypoint;

        public ChaseTarget(Enemy enemy, Rigidbody2D rb, Seeker seeker)
        {
            this.enemy = enemy;
            this.rb = rb;
            this.seeker = seeker;
        }

        public void OnEnter()
        {
            enemy.Animator.SetFloat(speedHash, 1f);
            seeker.pathCallback += OnPathReceived;
            lastRepath = float.MinValue;
            path = null;
        }

        public void OnExit()
        {
            enemy.Animator.SetFloat(speedHash, 0f);
            rb.velocity = Vector2.zero;
            seeker.pathCallback -= OnPathReceived;
        }

        public void Tick()
        {
            if (lastRepath + RepathRate <= Time.time)
            {
                lastRepath = Time.time;
                seeker.StartPath(rb.position, enemy.Target.position);
            }

            if (path == null)
                return;

            ProcessWaypoints();

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

            rb.velocity = direction * enemy.Stats.MovementSpeed;
            enemy.Animator.SetFloat(moveXHash, direction.x);
            enemy.Animator.SetFloat(moveYHash, direction.y);
        }

        private void OnPathReceived(Path path)
        {
            if (!path.error)
            {
                this.path = path;
                currentWaypoint = 0;
            }
        }

        private void ProcessWaypoints()
        {
            while (true)
            {
                float distanceToWaypoint = (enemy.transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude;
                if (distanceToWaypoint < NextWaypointDistance * NextWaypointDistance)
                {
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                        currentWaypoint++;
                    else
                        break;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
