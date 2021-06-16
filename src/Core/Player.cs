namespace Core {
    public record Player(string FirstName, string LastName) {
        public override string ToString() =>
            $@"Игрок {FirstName} {LastName}";
    }
}
