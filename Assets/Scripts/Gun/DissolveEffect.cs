using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class DissolveEffect : MonoBehaviour
    {
        private Material material;

        private void Awake()
        {
            SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
            renderer.material = material = new Material(renderer.material);
        }

        public void Dissolve(float duration)
        {
            if (duration != 0)
                StartCoroutine(HandleDissolving(1, 0, duration));
            else
                material.SetFloat("_Fade", 0);
        }

        public void Undissolve(float duration)
        {
            if (duration != 0)
                StartCoroutine(HandleDissolving(0, 1, duration));
            else
                material.SetFloat("_Fade", 1);
        }

        private IEnumerator HandleDissolving(float from, float to, float duration)
        {
            float currentDuration = 0f;
            while (currentDuration < duration)
            {
                currentDuration += Time.deltaTime;
                float t = currentDuration / duration;
                float dissolve = Mathf.Lerp(from, to, t);
                material.SetFloat("_Fade", dissolve);

                yield return null;
            }
        }
    }
}
