using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace GJLJam
{
    public class GunManager : MonoBehaviour
    {
        private const string saveKey = "gunData";

        public event Action<Gun> GunReady;

        private Dictionary<string, GunDataObject> gunsDatabase = new Dictionary<string, GunDataObject>();

        private Coroutine gunPreparingRoutine;
        private Coroutine queueResetRoutine;

        private List<Gun> gunsQueue = new List<Gun>();
        private List<Gun> usedGuns = new List<Gun>();
        private GunDisposingUI gunUI;

        private GameSettings Settings => Game.GameSettings;


        private void Awake()
        {
            InitDatabase();
        }

        private void Start()
        {
            foreach (GunDataObject gunData in gunsDatabase.Values)
            {
                Gun gun = Instantiate(gunData.GunPrefab).GetComponent<Gun>();
                gun.Setup(gunData);
                gunsQueue.Add(gun);
                gun.Deactivate();
            }

            Shuffle();
        }

        public void RequestNext()
        {
            if (gunsQueue.Count > 0)
            {
                gunPreparingRoutine = StartCoroutine(GrabNext());
            }
            else
            {
                // No guns in the queue. Alert UI
            }
        }

        public void RequestResetQueue()
        {
            if (queueResetRoutine != null)
                return;

            if (gunPreparingRoutine != null)
            {
                StopCoroutine(gunPreparingRoutine);
                gunPreparingRoutine = null;
            }

            queueResetRoutine = StartCoroutine(ResetGunQueue());
        }

        private IEnumerator GrabNext()
        {
            yield return new WaitForSeconds(Settings.GunPreparingDelay);

            Gun result = gunsQueue[0];
            gunsQueue.RemoveAt(0);
            usedGuns.Add(result);

            GunReady?.Invoke(result);
            gunPreparingRoutine = null;
        }

        private IEnumerator ResetGunQueue()
        {
            foreach (Gun usedGun in usedGuns)
            {
                usedGun.Dissolve(Settings.DissolveDuration);
            }

            yield return new WaitForSeconds(Settings.GunQueueReload);

            gunsQueue.AddRange(usedGuns);
            usedGuns.Clear();
            RequestNext();
            queueResetRoutine = null;
        }

        private void Shuffle()
        {
            gunsQueue.Shuffle();
        }

        private void InitDatabase()
        {
            GunDataObject[] loadedObjects = Resources.LoadAll<GunDataObject>("Guns");
            foreach (GunDataObject loadedObject in loadedObjects)
            {
                gunsDatabase.Add(loadedObject.Id, loadedObject);
            }
        }
    }
}
