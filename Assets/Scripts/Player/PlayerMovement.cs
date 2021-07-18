using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class PlayerMovement : MonoBehaviour
    {
        public Vector2 CurrentMove => currentDirection;

        private Player player;
        private Rigidbody2D rb;
        private PlayerAnimation animator;
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip[] footstepSounds;

        private Vector2 currentDirection;

        private void Start()
        {
            player = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<PlayerAnimation>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            Move();
            Animate();

            if (player.State == PlayerState.Stunned)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            bool spacePressed = Input.GetKeyDown(KeyCode.Space);

            SimplifyMovement(ref horizontal, ref vertical);

            Vector2 direction = spacePressed ? Vector2.zero : new Vector2(horizontal, vertical);

            if ((direction != Vector2.zero || spacePressed) && CanMove(direction) && direction.normalized != currentDirection)
            {
                currentDirection = direction;
                player.Stats.RemoveMove(direction, true);
            }
        }

        private void SimplifyMovement(ref float horizontal, ref float vertical)
        {
            if (horizontal != 0f)
                vertical = 0f;
            else if (vertical != 0f)
                horizontal = 0f;
        }

        private void Move()
        {
            rb.velocity = currentDirection.normalized * player.Stats.MovementSpeed;
            if (rb.velocity != Vector2.zero)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = GetRandomSound();
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }

        private void Animate()
        {
            animator.AnimateMovement(rb.velocity);
        }

        private AudioClip GetRandomSound()
        {
            int index = Random.Range(0, footstepSounds.Length);
            return footstepSounds[index];
        }

        private bool CanMove(Vector2 direction) => player.Stats.MovementCount(direction) > 0;
    }
}
