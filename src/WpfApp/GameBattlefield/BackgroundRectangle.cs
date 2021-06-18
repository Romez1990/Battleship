using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record BackgroundRectangle : Rectangle {
        public BackgroundRectangle(Vector fieldSize) : base(
            Vector.Zero,
            fieldSize
        ) { }
    }
}
