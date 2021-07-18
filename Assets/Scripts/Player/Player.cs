using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GJLJam
{
    public class Player : Character
    {
        public static Player Current { get; private set; }

        public PlayerMovement Movement => movement;
        public PlayerShooting Shooting => shooting;
        public PlayerAim Aim => aim;
        public PlayerAnimation Animator => animator;
        public PlayerStats Stats => stats;
        public PlayerState State { get; set; }

        private PlayerStats stats;
        [SerializeField]
        private PlayerDataObject data;
        [SerializeField]
        private MovementUI movementUI;
        [SerializeField]
        private HealthBar healthBar;

        private PlayerMovement movement;
        private PlayerShooting shooting;
        private PlayerAim aim;
        private PlayerAnimation animator;

        protected override void Awake()
        {
            base.Awake();
            movement = GetComponent<PlayerMovement>();
            shooting = GetComponent<PlayerShooting>();
            animator = GetComponent<PlayerAnimation>();
            aim = GetComponent<PlayerAim>();

            stats = new PlayerStats(data, movementUI);
            State = PlayerState.Normal;

            Current = this;
        }
        
        public void Hit(int damage)
        {
            OnHit(damage);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            stats.OnDeath += OnDeath;
            Enemy.OnEnemyKilled += stats.ReceiveMovement;
            stats.OnDamageReceive += UpdateHealthBar;
            UpdateHealthBar();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            stats.OnDeath -= OnDeath;
            Enemy.OnEnemyKilled -= stats.ReceiveMovement;
            stats.OnDamageReceive -= UpdateHealthBar;
        }

        protected override void OnHit(int damage)
        {
            if (State != PlayerState.Invulnerable)
            {
                Debug.Log("Player hitted " + damage);
                Stats.TakeDamage(damage);
                StartCoroutine(HandleInvulnerable());
            }
        }

        public void SetHands(bool show)
        {
            if (show)
                animator.ShowHands();
            else
                animator.HideHands();
        }

        private void OnDeath()
        {
            SceneManager.LoadScene(0);
        }

        private IEnumerator HandleInvulnerable()
        {
            State = PlayerState.Invulnerable;
            yield return new WaitForSeconds(Stats.InvulnerableTime);
            State = PlayerState.Normal;
        }

        private void UpdateHealthBar() => healthBar.SetHealth(stats.Health, stats.MaxHealth);
    }

    public enum PlayerState
    {
        Normal,
        Stunned,
        Invulnerable
    }
}