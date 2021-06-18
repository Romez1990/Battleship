using System;

namespace Core.Geometry {
    public record Vector(int X, int Y) {
        public Vector(int n) : this(n, n) { }

        public static readonly Vector Zero = new(0);
        public static readonly Vector Unit = new(1);

        public Vector Map(Func<int, int> fn) =>
            new(fn(X), fn(Y));

        public bool Every(Func<int, bool> fn) =>
            fn(X) && fn(Y);

        public static Vector operator -(Vector a) =>
            new(-a.X, -a.Y);

        public static Vector operator +(Vector a, int n) =>
            new(a.X + n, a.Y + n);

        public static Vector operator -(Vector a, int n) =>
            a + -n;

        public static Vector operator *(Vector a, int n) =>
            new(a.X * n, a.Y * n);

        public static Vector operator +(Vector a, Vector b) =>
            new(a.X + b.X, a.Y + b.Y);

        public static Vector operator -(Vector a, Vector b) =>
            a + -b;

        public static int operator *(Vector a, Vector b) =>
            a.X * b.X + a.Y * b.Y;
    }
}
