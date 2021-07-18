using UnityEngine;

namespace GJLJam
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Game/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public float DissolveDuration => dissolveDuration;
        public float GunPreparingDelay => gunPreparingTime;
        public float GunQueueReload => gunQueueReload;
        public float DisposeAfter => disposeAfter;
        public float ScalePerDamage => scalePerDamage;
        public float MinAngularVelocity => minAngularVelocity;
        public float MaxAngularVelocity => maxAngularVelocity;
        public float ThrowVerticalStrength => throwVerticalStrength;
        public float ThrowStrength => throwStrength;
        public float ThrowSpread => throwSpread;


        [Header("Guns")]
        [SerializeField]
        private float dissolveDuration;
        [SerializeField]
        private float gunPreparingTime = 1f;
        [SerializeField]
        private float gunQueueReload;
        [SerializeField]
        private float disposeAfter;
        [SerializeField]
        private float scalePerDamage;

        [Header("Throw Simulation")]
        [SerializeField]
        private float minAngularVelocity;
        [SerializeField]
        private float maxAngularVelocity;
        [SerializeField]
        private float throwVerticalStrength;
        [SerializeField]
        private float throwStrength;
        [SerializeField]
        private float throwSpread;

        [Header("Enemy")]
        [SerializeField]
        private LayerMask playerLayer;

    }
}