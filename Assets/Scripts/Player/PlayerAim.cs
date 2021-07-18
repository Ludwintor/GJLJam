using Ludwintor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GJLJam
{
    public class PlayerAim : MonoBehaviour
    {
        private const int renderInfront = 2;
        private const int renderBehind = 0;

        public Vector2 PivotPosition => gunPivot.position;

        [SerializeField]
        private Transform gunPivot;
        [SerializeField]
        private Transform lightPivot;

        private Player player;

        private PlayerAnimation Animator => player.Animator;
        private Gun CurrentGun => player.Shooting.CurrentGun;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            HandleAiming();
        }

        private void HandleAiming()
        {
            Vector2 mousePosition = LUtils.GetMouseWorldPosition();
            Vector2 aimDirection = (mousePosition - (Vector2)gunPivot.position).normalized;
            float angle = LMath.VectorToAngle(aimDirection);
            gunPivot.eulerAngles = new Vector3(0, 0, angle);
            lightPivot.eulerAngles = new Vector3(0, 0, angle);

            Vector3 scale = Vector3.one;
            if (Mathf.Abs(angle) > 90)
                scale.y = -1f;
            else
                scale.y = 1f;

            gunPivot.localScale = scale;
            HandleSwap(angle);
            Animate(aimDirection, angle);
        }

        private void Animate(Vector2 direction, float angle)
        {
            Animator.AnimateAim(direction);
            if (angle > 0f)
            {
                UpdateOrder(renderBehind, renderInfront); // Player behind the gun

            }
            else
            {
                UpdateOrder(renderInfront, renderBehind); // Player infront the gun
            }
        }

        private void HandleSwap(float angle)
        {
            if (player.Shooting.HasGun)
            {
                if (Mathf.Abs(angle) > 90)
                {
                    Animator.LeftHand.gameObject.SetActive(true);
                    Animator.RightHand.gameObject.SetActive(false);
                }
                else
                {
                    Animator.LeftHand.gameObject.SetActive(false);
                    Animator.RightHand.gameObject.SetActive(true);
                }
            }
            else
            {
                Animator.ShowHands();
                Animator.LeftHand.gameObject.SetActive(true);
                Animator.RightHand.gameObject.SetActive(true);
            }

        }

        private void UpdateOrder(int gunOrder, int handsOrder)
        {
            if (player.Shooting.HasGun)
                CurrentGun.UpdateSortingOrder(gunOrder);

            Animator.LeftHand.sortingOrder = handsOrder;
            Animator.RightHand.sortingOrder = handsOrder;
        }
    }
}
