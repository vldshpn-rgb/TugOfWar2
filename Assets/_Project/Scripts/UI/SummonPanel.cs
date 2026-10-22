using System.Collections.Generic;
using TugOfWar.Squads;
using UnityEngine;

namespace TugOfWar.UI
{
    /// <summary>Створює по кнопці виклику на кожен доступний гравцеві тип загону.</summary>
    public sealed class SummonPanel : MonoBehaviour
    {
        [SerializeField] private SpawnButton buttonPrefab;
        [SerializeField] private Transform container;

        public void Init(SquadSummoner summoner, IReadOnlyList<SquadDefinition> squads)
        {
            foreach (var definition in squads)
                Instantiate(buttonPrefab, container).Init(summoner, definition);
        }
    }
}
