using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    [RequireComponent(typeof(DissolveEffect))]
    public abstract class Enemy : Character
    {
        public static event Action<Enemy> OnEnemyKilled;

        public EnemyStats Stats { get; private set; }
        public Animator Animator { get; private set; }
        public Transform Target { get; private set; }

        protected StateMachine StateMachine { get; private set; }
        protected Rigidbody2D Rigidbody { get; private set; }
        protected Seeker Seeker { get; private set; }

        [SerializeField]
        private EnemyDataObject data;
        [SerializeField]
        private HealthBar healthBar;

        private DissolveEffect dissolve;

        protected override void Awake()
        {
            base.Awake();
            Stats = new EnemyStats(data);
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Seeker = GetComponent<Seeker>();
            dissolve = GetComponent<DissolveEffect>();

            StateMachine = new StateMachine();
        }

        protected virtual void Start()
        {
            Target = Player.Current.transform;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Stats.OnDeath += OnDeath;
            Stats.OnDamageReceive += UpdateHealthBar;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Stats.OnDeath -= OnDeath;
            Stats.OnDamageReceive -= UpdateHealthBar;
        }

        protected virtual void Update()
        {
            StateMachine.Tick();
        }

        public void Spawn(float dissolveTime)
        {
            dissolve.Dissolve(0f);
            dissolve.Undissolve(dissolveTime);
        }

        protected override void OnHit(int damage)
        {
            Stats.TakeDamage(damage);
        }

        public void BaseDeath() => OnDeath();

        protected virtual void OnDeath()
        {
            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject);
        }

        private void UpdateHealthBar() => healthBar.SetHealth(Stats.Health, Stats.MaxHealth);
    }
}
