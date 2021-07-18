using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class Skelesaw : Enemy
    {
        private ChaseTarget chasing;
        private ContactDamage contact;


        protected override void Awake()
        {
            base.Awake();

            contact = GetComponent<ContactDamage>();
            contact.Damage = Stats.Damage;

            chasing = new ChaseTarget(this, Rigidbody, Seeker);

            StateMachine.SetState(chasing);
        }
    }
}
