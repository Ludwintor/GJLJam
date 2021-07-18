using UnityEngine;

namespace GJLJam
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Game/Data/Player")]
    public class PlayerDataObject : CharacterDataObject
    {
        public float InvulnerableTime => invulnerableTime;
        
        [SerializeField]
        private float invulnerableTime;
    }
}