using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record FilledRectangle(Vector Coordinates, Vector Size, string Color) : Rectangle(Coordinates, Size);
}
