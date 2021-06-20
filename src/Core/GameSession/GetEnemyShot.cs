using Core.Field;

namespace Core.GameSession {
    public record GetEnemyShot(Cross Coordinates, bool Hit, bool Won);
}
