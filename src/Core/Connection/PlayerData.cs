using System.Collections.Immutable;
using Core.Field;
using Core.PlayerData;

namespace Core.Connection {
    public record PlayerData(Player Player, ImmutableArray<Ship> Ships, string ConnectionCode);
}
