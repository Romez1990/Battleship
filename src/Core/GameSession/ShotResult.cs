using Core.Field;

namespace Core.GameSession {
    public record ShotResult(bool Hit, bool Destroyed, Ship DestroyedShip, bool Won, Question Question);
}
