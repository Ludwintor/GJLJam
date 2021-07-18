using Ludwintor.Tools;
using System.Collections;
using UnityEngine;

namespace GJLJam
{
    public class Gun : MonoBehaviour
    {
        public GunOutput[] GunOutputs => gunEnds;
        public GunDataObject Data => data;

        [SerializeField]
        private GunOutput[] gunEnds;

        private GunDataObject data;

        private Animator animator;
        private AudioSource audioSource;
        private ThrowSimulation throwSimulation;
        private DissolveEffect dissolve;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            throwSimulation = GetComponent<ThrowSimulation>();
            dissolve = GetComponent<DissolveEffect>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Animate()
        {
            animator.SetTrigger("shoot");
            audioSource.PlayOneShot(data.ShotSound);
        }

        public void Setup(GunDataObject data)
        {
            this.data = data;
        }

        public void ShootBullet(Vector2 startPoint, Transform from, int mask)
        {
            Vector2 direction = LMath.AngleToVector(from.eulerAngles.z);
            Vector2 spreaded = Spread(direction, data.MaxSpread);
            Bullet bullet = Game.Pools.BulletPool.Claim<Bullet>();
            bullet.Setup(startPoint, spreaded * data.Speed, data.Damage, data.Bounces, data.BulletSprite, data.ScaleBulletWithDamage, mask);
        }

        private Vector2 Spread(Vector2 direction, float maxSpread)
        {
            maxSpread *= 0.5f;
            float spread = Random.Range(-maxSpread, maxSpread);

            return LMath.RotateVector(direction, spread).normalized;
        }

        public void UpdateSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }

        public void Activate()
        {
            throwSimulation.ResetSprite();
            animator.enabled = true;
            dissolve.Undissolve(0f);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Throw(Vector2 velocity)
        {
            transform.SetParent(null, true);
            animator.enabled = false;
            spriteRenderer.sprite = data.GunSprite;
            UpdateSortingOrder(0);

            GameSettings settings = Game.GameSettings;

            int randomSign = Random.Range(0f, 1f) >= 0.5f ? 1 : -1;
            float angularVelocity = Random.Range(settings.MinAngularVelocity, settings.MaxAngularVelocity);
            throwSimulation.Simulate(velocity, settings.ThrowVerticalStrength, 0.1f, angularVelocity * randomSign);
        }

        public void Dissolve(float duration)
        {
            dissolve.Dissolve(duration);
        }

        public void Undissolve(float duration)
        {
            dissolve.Undissolve(duration);
        }
    }
}