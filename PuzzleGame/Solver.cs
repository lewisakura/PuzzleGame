using System;
using System.Collections.Generic;

namespace PuzzleGame;

// this implements layered restoration, based on the algorithm found at https://www.jaapsch.net/puzzles/javascript/fifteenj.htm
// essentially just a port to C# and then some rewrites to make it suit our game code better
public class Solver
{
    private const int Size = PuzzleGame.Size;
    private static int[,] state;
    private static int blankX, blankY;

    // Main entry point: returns a stack of puzzle states at each step
    public static Stack<int[,]> Solve(int[,] input)
    {
        Stack<int[,]> stateStack = new Stack<int[,]>();
        state = (int[,])input.Clone();
        (blankX, blankY) = FindBlankSpace(state);

        // Save the initial state
        SaveState(stateStack);

        // Step 1: Solve the top row
        for (int target = 1; target <= Size; target++)
        {
            PlaceTile(target, stateStack);
        }

        // Step 2: Solve the second row
        for (int target = Size + 1; target <= Size * 2; target++)
        {
            PlaceTile(target, stateStack);
        }

        // Step 3: Solve the third row except the last two tiles
        for (int target = Size * 2 + 1; target <= Size * 3 - 1; target++)
        {
            PlaceTile(target, stateStack);
        }

        // Step 4: Solve the bottom left tile
        PlaceTile(Size * 3, stateStack);

        // Step 5: Solve the last two tiles (final positioning)
        SolveLastTwoTiles(stateStack);

        return stateStack;
    }

    private static void PlaceTile(int target, Stack<int[,]> stateStack)
    {
        (int targetX, int targetY) = FindTargetPosition(target);

        while (state[targetX, targetY] != target)
        {
            MoveTileToTarget(target, targetX, targetY, stateStack);
        }
    }

    private static void MoveTileToTarget(int target, int targetX, int targetY, Stack<int[,]> stateStack)
    {
        (int currentX, int currentY) = FindTilePosition(target);

        while (currentX != targetX || currentY != targetY)
        {
            Direction dir = GetNextMoveDirection(currentX, currentY, targetX, targetY);
            (blankX, blankY) = UpdateBlankSpace(dir);
            Swap(state, currentX, currentY, blankX, blankY);
            SaveState(stateStack); // Save state after each move
            (currentX, currentY) = FindTilePosition(target);
        }
    }

    private static Direction GetNextMoveDirection(int currentX, int currentY, int targetX, int targetY)
    {
        if (currentY < targetY) return Direction.Right;
        if (currentY > targetY) return Direction.Left;
        if (currentX < targetX) return Direction.Down;
        return Direction.Up;
    }

    private static void SolveLastTwoTiles(Stack<int[,]> stateStack)
    {
        if (state[2, 2] != 13 || state[2, 3] != 14 || state[3, 2] != 15 || state[3, 3] != 0)
        {
            // Example final swaps for positioning
            Swap(state, 3, 2, 3, 3); // Example movement (3,2) <-> (3,3)
            SaveState(stateStack);
        }
    }

    private static (int, int) FindBlankSpace(int[,] state)
    {
        for (int i = 0; i < state.GetLength(0); i++)
        for (int j = 0; j < state.GetLength(1); j++)
            if (state[i, j] == 0)
                return (i, j);
        throw new Exception("Blank space not found");
    }

    private static (int, int) FindTilePosition(int tile)
    {
        for (int i = 0; i < state.GetLength(0); i++)
        for (int j = 0; j < state.GetLength(1); j++)
            if (state[i, j] == tile)
                return (i, j);
        throw new Exception("Tile not found");
    }

    private static (int, int) FindTargetPosition(int tile)
    {
        int targetX = (tile - 1) / Size;
        int targetY = (tile - 1) % Size;

        if (tile == 0)
            return (Size - 1, Size - 1);

        return (targetX, targetY);
    }

    private static (int, int) UpdateBlankSpace(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right: return (blankX, blankY + 1);
            case Direction.Left: return (blankX, blankY - 1);
            case Direction.Up: return (blankX - 1, blankY);
            case Direction.Down: return (blankX + 1, blankY);
            default: throw new ArgumentException("Invalid direction");
        }
    }

    private static void Swap(int[,] array, int x1, int y1, int x2, int y2)
    {
        int temp = array[x1, y1];
        array[x1, y1] = array[x2, y2];
        array[x2, y2] = temp;
    }

    private static void SaveState(Stack<int[,]> stateStack)
    {
        int[,] snapshot = (int[,])state.Clone();
        stateStack.Push(snapshot);
    }
}