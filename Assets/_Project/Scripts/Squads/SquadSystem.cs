using System.Collections.Generic;
using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Подає кадри симуляції загонів (Architecture 2.2). Уся логіка в SquadSimulation.
    /// </summary>
    public sealed class SquadSystem : MonoBehaviour
    {
        private readonly SquadSimulation _simulation = new();

        public IReadOnlyList<Squad> Squads => _simulation.Squads;

        public void Add(Squad squad) => _simulation.Add(squad);

        private void Update() => _simulation.Step(Time.deltaTime);
    }
}
