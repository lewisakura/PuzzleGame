using System;

namespace PuzzleGame;

// exists because i need an integer based vector for the grid, and monogame's float-based one is annoying to work with
public struct Vector2Int(int x, int y) : IEquatable<Vector2Int>
{
    public bool Equals(Vector2Int other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        return obj is Vector2Int other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !(a == b);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int { X = a.X - b.X, Y = a.Y - b.Y };
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}