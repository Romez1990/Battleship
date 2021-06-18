namespace Core.PlayerData {
    public record Player(string FirstName, string LastName) {
        public override string ToString() =>
            $@"Игрок {FirstName} {LastName}";
    }
}
