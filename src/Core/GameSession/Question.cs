using System.Collections.Immutable;

namespace Core.GameSession {
    public record Question(string Text, ImmutableArray<string> Answers);
}
