using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>Візуал одного солдата (D-001): лише спрайт. Біг у бій і смерть додамо пізніше.</summary>
    public sealed class SoldierView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        public void SetColor(Color color) => sprite.color = color;
    }
}
