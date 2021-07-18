using Ludwintor.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class Game : MonoBehaviour
    {
        public static ObjectPools Pools => instance.pools;
        public static GunManager GunManager => instance.gunManager;
        public static GameSettings GameSettings => instance.gameSettings;

        [SerializeField]
        private ObjectPools pools;
        [SerializeField]
        private GunManager gunManager;
        [SerializeField]
        private GameSettings gameSettings;

        private static Game instance;

        private void Awake()
        {
            instance = this;
        }
    }

    [Serializable]
    public class ObjectPools
    {
        public ObjectPool BulletPool => bulletPool;

        [SerializeField]
        private ObjectPool bulletPool;
    }
}
