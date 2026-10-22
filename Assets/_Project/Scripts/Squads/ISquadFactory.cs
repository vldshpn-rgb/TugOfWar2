using TugOfWar.Battle;

namespace TugOfWar.Squads
{
    /// <summary>Хто вміє створити загін. Інтерфейс, щоб SquadSummoner можна було перевірити тестом без сцени.</summary>
    public interface ISquadFactory
    {
        Squad Spawn(SquadDefinition definition, Side side);
    }
}
