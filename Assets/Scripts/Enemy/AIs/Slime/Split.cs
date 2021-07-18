using UnityEngine;

namespace GJLJam
{
    public class Split : IState
    {
        public bool IsSplitting { get; private set; }

        private readonly Enemy enemy;
        private readonly GameObject splitTo;
        private readonly int count;
        private readonly float spawnRadius;
        private readonly float timeForSplit;
        private float splitTime;

        public Split(Enemy enemy, GameObject splitTo, int count, float spawnRadius, float timeForSplit)
        {
            this.enemy = enemy;
            this.splitTo = splitTo;
            this.count = count;
            this.spawnRadius = spawnRadius;
            this.timeForSplit = timeForSplit;
        }

        public void OnEnter()
        {
            IsSplitting = true;
        }

        public void OnExit()
        {

        }

        public void Tick()
        {
            splitTime += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, splitTime / timeForSplit);
            enemy.transform.localScale = new Vector3(1f, scale, 1f);

            if (splitTime >= timeForSplit)
            {
                SplitInto();
                splitTime = float.MinValue;
            }
        }

        private void SplitInto()
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 spawnPoint = (Vector2)enemy.transform.position + Random.insideUnitCircle * spawnRadius;
                Object.Instantiate(splitTo, spawnPoint, Quaternion.identity);
            }
        }
    }
}
