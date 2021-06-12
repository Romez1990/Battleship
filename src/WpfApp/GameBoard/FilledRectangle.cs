using Core.Geometry;

namespace WpfApp.GameBoard {
    public record FilledRectangle(Vector Coordinates, Vector Size, string Color) : Rectangle(Coordinates, Size);
}
