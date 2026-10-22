using System.Collections.Generic;
using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Тримає всі загони бою і раз на кадр просуває їх (Architecture 2.2).
    /// </summary>
    public sealed class SquadSystem : MonoBehaviour
    {
        private readonly List<Squad> _squads = new();

        public IReadOnlyList<Squad> Squads => _squads;

        public void Add(Squad squad) => _squads.Add(squad);

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < _squads.Count; i++)
                _squads[i].Tick(deltaTime);
        }
    }
}
