namespace WpfApp.GameSession {
    public record Score(int Player, int Enemy) {
        public Score() : this(0, 0) { }

        public Score AddPointToPlayer() =>
            new(Player + 1, Enemy);

        public Score AddPointToEnemy() =>
            new(Player, Enemy + 1);

        public override string ToString() =>
            $"{Player}:{Enemy}";
    }
}
