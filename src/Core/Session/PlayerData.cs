using System.Collections.Immutable;
using Core.Field;

namespace Core.Session {
    public record PlayerData(Player Player, ImmutableArray<Ship> Ships, string ConnectionCode);
}
