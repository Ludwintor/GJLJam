namespace GJLJam
{
    public interface IHittable
    {
        public HitCreature HitCreature { get; }

        void Hit(int damage);
    }

    public enum HitCreature
    {
        Player,
        Enemy
    }
}