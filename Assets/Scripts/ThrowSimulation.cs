using Ludwintor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ThrowSimulation : MonoBehaviour
    {
        private float velocity;
        private float angularVelocity;
        private float height;

        [SerializeField]
        private Transform sprite;

        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;
            rb.isKinematic = true;
        }

        public void Simulate(Vector2 moveVelocity, float initialVelocity, float initialHeight, float angularVelocity = 0f)
        {
            velocity = initialVelocity;
            height = initialHeight;
            this.angularVelocity = angularVelocity;

            rb.velocity = moveVelocity;
            StartCoroutine(HandleSimulation());
        }

        public void ResetSprite()
        {
            sprite.localPosition = Vector3.zero;
            sprite.localRotation = Quaternion.identity;
        }

        private IEnumerator HandleSimulation()
        {
            circleCollider.enabled = true;
            rb.isKinematic = false;
            float gravity = Physics2D.gravity.y;
            do
            {
                velocity += gravity * Time.deltaTime;
                height += velocity * Time.deltaTime;

                // We need to multiply with parent scale.y in case parent scale.y is -1 (rotated around Y axis)
                // (really need to test what gonna happen if parent's scale is not proportional)
                Vector3 spritePosition = new Vector3(0, height * transform.localScale.normalized.y, 0);
                sprite.localPosition = LMath.RotateVector(spritePosition, transform.localEulerAngles.z); // Also vector need to be (0, 1, 0) when normalized so we grab parent's local degrees and rotate it
                sprite.Rotate(0, 0, angularVelocity * Time.deltaTime);

                yield return null;
            }
            while (height > 0);

            circleCollider.enabled = false;
            rb.isKinematic = true;
            sprite.localPosition = Vector3.zero;
            rb.velocity = Vector2.zero;
        }
    }
}
