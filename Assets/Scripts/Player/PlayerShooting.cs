using Ludwintor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class PlayerShooting : MonoBehaviour
    {
        public bool HasGun => currentGun != null;
        public Gun CurrentGun => currentGun;

        [SerializeField]
        private Transform attachGun;
        [SerializeField]
        private LayerMask bulletLayer;
        private int bulletLayerIndex;

        private Player player;

        private Gun currentGun;
        private bool canShoot;

        private GunManager GunManager => Game.GunManager;

        private void Start()
        {
            bulletLayerIndex = (int)Mathf.Log(bulletLayer, 2);
            player = GetComponent<Player>();
            GunManager.RequestNext();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                GunManager.RequestResetQueue();
                Throw();
            }
        }

        public void SwapWeapon(Gun gun)
        {
            currentGun = gun;
            currentGun.transform.SetParent(attachGun, true);
            Transform gunTransform = gun.transform;
            gunTransform.localPosition = Vector3.zero;
            gunTransform.localRotation = Quaternion.identity;
            gunTransform.localScale = Vector3.one;
            player.SetHands(!currentGun.Data.IsTwoHanded);
            currentGun.Activate();
            canShoot = true;
        }

        public void Shoot()
        {
            if (currentGun == null || !canShoot)
                return;

            canShoot = false;
            GunDataObject data = currentGun.Data;
            if (data.DelayBetweenShots != 0f && data.BulletCount > 1)
                StartCoroutine(ShootHandlerDelay());
            else
                ShootWithoutDelay();
        }

        private void ShootWithoutDelay()
        {
            GunOutput[] gunOutputs = currentGun.GunOutputs;
            GunDataObject data = currentGun.Data;
            if (gunOutputs.Length == 1)
            {
                GunOutput gunOutput = gunOutputs[0];
                for (int i = 0; i < data.BulletCount; i++)
                {
                    currentGun.ShootBullet(gunOutput.transform.position, gunOutput.transform, bulletLayerIndex);
                }
                currentGun.Animate();
                gunOutput.Flash(data.MuzzleFlash);
            }
            else
            {
                int bulletsOnEach = data.BulletCount / gunOutputs.Length;
                int bulletsLeft = data.BulletCount % gunOutputs.Length;

                for (int i = 0; i < gunOutputs.Length; i++)
                {
                    GunOutput gunOutput = gunOutputs[i];
                    int bullets = bulletsOnEach;
                    if (i < bulletsLeft)
                        bullets++;

                    if (bullets == 0)
                        break;

                    for (int j = 0; j < bullets; j++)
                    {
                        currentGun.ShootBullet(gunOutput.transform.position, gunOutput.transform, bulletLayerIndex);
                    }
                    currentGun.Animate();
                    gunOutput.Flash(data.MuzzleFlash);
                }
            }

            OnShootEnded();
        }

        private IEnumerator ShootHandlerDelay()
        {
            GunOutput[] gunOutputs = currentGun.GunOutputs;
            GunDataObject data = currentGun.Data;
            int bulletsLeft = data.BulletCount;
            while (bulletsLeft > 0)
            {
                for (int i = 0; i < gunOutputs.Length; i++)
                {
                    GunOutput gunOutput = gunOutputs[i];

                    currentGun.ShootBullet(gunOutput.transform.position, gunOutput.transform, bulletLayerIndex);
                    currentGun.Animate();
                    gunOutput.Flash(data.MuzzleFlash);

                    bulletsLeft--;
                    if (bulletsLeft > 0)
                        yield return new WaitForSeconds(data.DelayBetweenShots);
                    else
                        break;
                }
            }

            OnShootEnded();
        }

        private IEnumerator DisposeGun()
        {
            yield return new WaitForSeconds(Game.GameSettings.DisposeAfter);
            Throw();
        }

        private void Throw()
        {
            if (currentGun == null)
                return;

            Vector2 direction = LUtils.GetMouseWorldPosition() - player.Aim.PivotPosition;

            GameSettings settings = Game.GameSettings;
            float angleSpread = settings.ThrowSpread * 0.5f;
            float angle = Random.Range(-angleSpread, angleSpread);
            direction = LMath.RotateVector(direction, angle);
            currentGun.Throw(direction.normalized * settings.ThrowStrength);
            currentGun = null;
        }

        private void OnShootEnded()
        {
            StartCoroutine(DisposeGun());
            GunManager.RequestNext();
        }

        private void OnEnable()
        {
            GunManager.GunReady += SwapWeapon;
        }

        private void OnDisable()
        {
            GunManager.GunReady -= SwapWeapon;
        }
    }
}
