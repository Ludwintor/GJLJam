using UnityEngine;

namespace GJLJam
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Game/Gun")]
    public class GunDataObject : ScriptableObject
    {
        public string Id => id;
        public int BulletCount => bulletsPerShoot;
        public int Damage => damagePerBullet;
        public bool ScaleBulletWithDamage => scaleBulletWithDamage;
        public float DelayBetweenShots => delayBetweenShots;
        public float MaxSpread => maxSpread;
        public float Speed => speed;
        public int Bounces => bounces;
        public Sprite GunSprite => gunSprite;
        public GameObject GunPrefab => gunPrefab;
        public Sprite BulletSprite => bulletSprite;
        public AnimationClip MuzzleFlash => muzzleFlash;
        public AudioClip ShotSound => shotSound;
        public bool IsTwoHanded => isTwoHanded;

        [SerializeField]
        private string id;
        [SerializeField]
        private int bulletsPerShoot;
        [SerializeField]
        private int damagePerBullet;
        [SerializeField]
        private bool scaleBulletWithDamage;
        [SerializeField]
        private float delayBetweenShots;
        [SerializeField]
        private float maxSpread;
        [SerializeField]
        private float speed;
        [SerializeField]
        private int bounces;
        [SerializeField]
        private Sprite gunSprite;
        [SerializeField]
        private GameObject gunPrefab;
        [SerializeField]
        private Sprite bulletSprite;
        [SerializeField]
        private AnimationClip muzzleFlash;
        [SerializeField]
        private AudioClip shotSound;

        [SerializeField]
        private bool isTwoHanded;
    }
}