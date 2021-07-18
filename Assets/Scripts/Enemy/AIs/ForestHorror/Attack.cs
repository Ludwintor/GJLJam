namespace GJLJam
{
    public class Attack : IState
    {
        private readonly Enemy enemy;
        private readonly SlashAttack attack;

        public Attack(Enemy enemy, SlashAttack attack)
        {
            this.enemy = enemy;
            this.attack = attack;
        }

        public void OnEnter()
        {
            attack.StartAttack(enemy.Stats.Damage);
        }

        public void OnExit()
        {
            attack.StopAttack();
        }

        public void Tick()
        {
            
        }
    }
}
