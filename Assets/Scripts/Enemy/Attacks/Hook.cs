using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GJLJam
{
    public class Hook : MonoBehaviour
    {
        public float HookSize => hookSprite.size.x;
        public Player Hooked => hooked;

        [SerializeField]
        private PlayerDetector hookEnd;
        [SerializeField]
        private AudioClip hookCatchSound;

        private SpriteRenderer hookSprite;
        private AudioSource audioSource;
        private SortingGroup group;
        private Player hooked;

        private void Awake()
        {
            hookSprite = GetComponentInChildren<SpriteRenderer>(true);
            audioSource = GetComponent<AudioSource>();
            group = GetComponent<SortingGroup>();
        }

        public void UpdateSorting(int sorting) => group.sortingOrder = sorting;

        public void SetHook(float size)
        {
            hookSprite.size = new Vector2(size, hookSprite.size.y);
            hookEnd.transform.localPosition = new Vector2(size, 0f);
        }

        public void ResetHook()
        {
            hookSprite.size = new Vector2(0f, hookSprite.size.y);
            hooked = null;
        }

        private void SetHooked(Player player)
        {
            if (hooked != null)
                return;

            audioSource.PlayOneShot(hookCatchSound);
            hooked = player;
        }

        private void OnEnable()
        {
            hookEnd.PlayerInRange += SetHooked;
        }

        private void OnDisable()
        {
            hookEnd.PlayerInRange -= SetHooked;
        }
    }
}
