using UnityEngine;

namespace GJLJam
{
    public class Hooking : IState
    {
        private readonly Enemy enemy;
        private readonly HookThrow hookThrow;
        private readonly float delay;
        private static readonly int hookingHash = Animator.StringToHash("isHooking");

        public Hooking(Enemy enemy, HookThrow hookThrow, float delay)
        {
            this.enemy = enemy;
            this.hookThrow = hookThrow;
            this.delay = delay;
        }

        public void OnEnter()
        {
            enemy.Animator.SetBool(hookingHash, true);
            hookThrow.StartThrow(enemy.Target.position, delay);
        }

        public void OnExit()
        {
            hookThrow.StopHooking();
            enemy.Animator.SetBool(hookingHash, false);
        }

        public void Tick()
        {

        }
    }
}
