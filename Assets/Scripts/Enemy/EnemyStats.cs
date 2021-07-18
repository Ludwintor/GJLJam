namespace GJLJam
{
    [System.Serializable]
    public class EnemyStats : Stats
    {
        public int Damage { get; private set; }

        public EnemyStats(EnemyDataObject data) : base(data.Health, data.MovementSpeed)
        {
            Damage = data.Damage;
        }
    }
}