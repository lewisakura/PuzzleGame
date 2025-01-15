using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleGame.UI;

namespace PuzzleGame.Scenes;

/// <summary>
/// The primary game.
/// </summary>
public class PuzzleScene() : Scene("Puzzle")
{
    private const int Size = PuzzleGame.Size;
    private const int TileSize = PuzzleGame.TileSize;
    private const int TilePadding = PuzzleGame.TilePadding;
    
    private int[,] _gameBoard;
    private Texture2D _tileTexture;

    private Vector2Int _selectedTile = new() { X = 0, Y = 0 };

    private SpriteFont _tileOverlayFont;

    private SoundEffect _tileSoundEffect;

    private int[,] CompletionState { get; } = new int[Size, Size];
    
    private IEnumerator<int[,]> _steppedSolver;

    private bool _showHints;

    private Text _moveText;

    private int _moves;
    private int Moves
    {
        get => _moves;
        set
        {
            _moveText.UpdateText(value.ToString());
            _moves = value;
        }
    }
    
    public override void Init()
    {
        _gameBoard = new int[Size, Size];
        
        var i = 0;
        for (var x = 0; x < Size; x++)
        for (var y = 0; y < Size; y++)
            _gameBoard[x, y] = i++;

        Array.Copy(_gameBoard, CompletionState, _gameBoard.Length);
        
        do
        {
            ShuffleArray(_gameBoard);
        } while (!IsSolvable(_gameBoard));

        AddUIComponent(new Button(new Rectangle(10, PuzzleGame.Resolution + 64, PuzzleGame.Resolution / 2 - 20, 50), "Shuffle [R]",
            () =>
            {
                do
                {
                    ShuffleArray(_gameBoard);
                } while (!IsSolvable(_gameBoard));
            }));
        
        AddUIComponent(new Button(new Rectangle(PuzzleGame.Resolution / 2, PuzzleGame.Resolution + 64, PuzzleGame.Resolution / 2 - 10, 50), "Numbers [LShift]",
            () =>
            {
                _showHints = !_showHints;
            }));

        _moveText = AddUIComponent(new Text(new Point(PuzzleGame.Resolution / 2, PuzzleGame.Resolution + 32), "0"));
        
        base.Init();
    }

    public override void LoadContent(ContentManager content)
    {
        _tileOverlayFont = content.Load<SpriteFont>("TileOverlay");
        _tileTexture = content.Load<Texture2D>("TextureBase");
        _tileSoundEffect = content.Load<SoundEffect>("TileSoundEffect");
        
        base.LoadContent(content);
    }

    public override void Update(GameTime gameTime)
    {
        if (InputManager.IsMouse1JustPressed())
        {
            var location = InputManager.GetMousePosition();
            
            // find the tile that was clicked
            for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
            {
                var collisionRect = new Rectangle(x * (TileSize + TilePadding) + TilePadding,
                    y * (TileSize + TilePadding) + TilePadding, TileSize, TileSize);

                if (!collisionRect.Contains(location) || _gameBoard[x, y] == 0) continue;
                _selectedTile = new Vector2Int { X = x, Y = y };
                break;
            }
        }

        if (InputManager.IsMouse1Held())
        { 
            var blank = new Vector2Int();
            
            for (var x = 0; x < Size; x++)
                for (var y = 0; y < Size; y++)
                    if (_gameBoard[x, y] == 0)
                        blank = new Vector2Int { X = x, Y = y };

            var collisionRect = new Rectangle(blank.X * (TileSize + TilePadding) + TilePadding,
                blank.Y * (TileSize + TilePadding) + TilePadding, TileSize, TileSize);

            if (InputManager.MouseHoveringRect(collisionRect))
            {
                var direction = blank - _selectedTile;

                switch (direction.X)
                {
                    case -1:
                        MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Left);
                        break;
                    case 1:
                        MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Right);
                        break;
                }

                switch (direction.Y)
                {
                    case -1:
                        MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Up);
                        break;
                    case 1:
                        MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Down);
                        break;
                }

                Console.WriteLine(direction);
            }
        }
        
        if (InputManager.IsKeyJustPressed(Keys.W))
        {
            MoveSelection(Direction.Up);
        }

        if (InputManager.IsKeyJustPressed(Keys.S))
        {
            MoveSelection(Direction.Down);
        }

        if (InputManager.IsKeyJustPressed(Keys.A))
        {
            MoveSelection(Direction.Left);
        }

        if (InputManager.IsKeyJustPressed(Keys.D))
        {
            MoveSelection(Direction.Right);
        }

        if (InputManager.IsKeyJustPressed(Keys.Up))
        {
            MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Up);
        }

        if (InputManager.IsKeyJustPressed(Keys.Down))
        {
            MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Down);
        }

        if (InputManager.IsKeyJustPressed(Keys.Left))
        {
            MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Left);
        }

        if (InputManager.IsKeyJustPressed(Keys.Right))
        {
            MoveTile(_selectedTile.X, _selectedTile.Y, Direction.Right);
        }

        if (InputManager.IsKeyJustPressed(Keys.R))
        {
            do
            {
                ShuffleArray(_gameBoard);
            } while (!IsSolvable(_gameBoard));
        }
        
        if (InputManager.IsKeyJustPressed(Keys.LeftShift))
        {
            _showHints = !_showHints;
        }

        if (_steppedSolver is not null)
        {
            if (_steppedSolver.MoveNext())
            {
                var instruction = _steppedSolver.Current;

                _gameBoard = instruction;

                // if (instruction is Solver.Instruction.Select selection)
                // {
                //     _selectedTile = selection.Tile;
                // } else if (instruction is Solver.Instruction.Move movement)
                // {
                //     MoveTile(_selectedTile.X, _selectedTile.Y, movement.Direction);
                // }
            }
            else
            {
                _steppedSolver.Dispose();
                _steppedSolver = null;
            }
        }
        
        base.Update(gameTime);
    }
    
    public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        graphicsDevice.Clear(Color.Black);

        for (var x = 0; x < Size; x++)
        for (var y = 0; y < Size; y++)
            DrawTile(spriteBatch, x, y);
        
        base.Draw(gameTime, graphicsDevice, spriteBatch);
    }
    
    private static void ShuffleArray<T>(T[,] values)
    {
        var rows = values.GetUpperBound(0) + 1;
        var cols = values.GetUpperBound(1) + 1;
        var cells = rows * cols;

        var rand = new Random();
        for (var i = 0; i < cells - 1; i++)
        {
            var j = rand.Next(i, cells);

            var rowA = i / cols;
            var colA = i % cols;
            var rowB = j / cols;
            var colB = j % cols;

            (values[rowA, colA], values[rowB, colB]) = (values[rowB, colB], values[rowA, colA]);
        }
    }

    private void DrawTile(SpriteBatch spriteBatch, int x, int y)
    {
        if (_gameBoard[x, y] == 0) return; // blank tile

        var value = _gameBoard[x, y];
        var text = value.ToString();
        var textSize = _tileOverlayFont.MeasureString(text);

        var texX = value % 4 * TileSize;
        var texY = value / 4 * TileSize;
        
        spriteBatch.Draw(
            _tileTexture,
            new Vector2(x * (TileSize + TilePadding) + TilePadding, y * (TileSize + TilePadding) + TilePadding),
            new Rectangle(texX, texY, TileSize, TileSize),
            _selectedTile == new Vector2Int { X = x, Y = y } ? Color.Aquamarine : Color.White);

        if (_showHints)
        {
            spriteBatch.DrawString(
                _tileOverlayFont,
                text,
                new Vector2(
                    x * (TileSize + TilePadding) + TilePadding + TileSize / 2 - textSize.X / 2,
                    y * (TileSize + TilePadding) + TilePadding + TileSize / 2 - textSize.Y / 2),
                Color.White);
        }
    }

    private void MoveSelection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up when _selectedTile.Y == 0:
            case Direction.Up when _gameBoard[_selectedTile.X, _selectedTile.Y - 1] == 0:
                return;
            case Direction.Up:
                _selectedTile.Y--;
                break;

            case Direction.Down when _selectedTile.Y == Size - 1:
            case Direction.Down when _gameBoard[_selectedTile.X, _selectedTile.Y + 1] == 0: 
                return;
            case Direction.Down:
                _selectedTile.Y++;
                break;

            case Direction.Left when _selectedTile.X == 0:
            case Direction.Left when _gameBoard[_selectedTile.X - 1, _selectedTile.Y] == 0:
                return;
            case Direction.Left:
                _selectedTile.X--;
                break;

            case Direction.Right when _selectedTile.X == Size - 1:
            case Direction.Right when _gameBoard[_selectedTile.X + 1, _selectedTile.Y] == 0:
                return;
            case Direction.Right:
                _selectedTile.X++;
                break;
        }
    }

    private void MoveTile(int x, int y, Direction direction)
    {
        if (_gameBoard[x, y] == 0) return; // can't move the empty tile

        switch (direction)
        {
            case Direction.Up when y == 0:
            case Direction.Up when _gameBoard[x, y - 1] != 0:
                return;
            case Direction.Up:
                _gameBoard[x, y - 1] = _gameBoard[x, y];
                _gameBoard[x, y] = 0;
                _selectedTile.Y--;
                break;

            case Direction.Down when y == Size - 1:
            case Direction.Down when _gameBoard[x, y + 1] != 0:
                return;
            case Direction.Down:
                _gameBoard[x, y + 1] = _gameBoard[x, y];
                _gameBoard[x, y] = 0;
                _selectedTile.Y++;
                break;

            case Direction.Left when x == 0:
            case Direction.Left when _gameBoard[x - 1, y] != 0:
                return;
            case Direction.Left:
                _gameBoard[x - 1, y] = _gameBoard[x, y];
                _gameBoard[x, y] = 0;
                _selectedTile.X--;
                break;

            case Direction.Right when x == Size - 1:
            case Direction.Right when _gameBoard[x + 1, y] != 0:
                return;
            case Direction.Right:
                _gameBoard[x + 1, y] = _gameBoard[x, y];
                _gameBoard[x, y] = 0;
                _selectedTile.X++;
                break;
        }

        _tileSoundEffect.Play();
        Moves++;
    }

    private static bool IsSolvable(int[,] state)
    {
        var flatBoard = state.Cast<int>().ToArray();
        var inversions = CountInversions(flatBoard);

        if (Size % 2 == 1)
        {
            return inversions % 2 != 0;
        }
        else
        {
            var blankRow = FindBlankRowFromBottom(state);
            return blankRow % 2 == 0 == (inversions % 2 == 1);
        }
    }

    private static int CountInversions(int[] flatBoard)
    {
        var inversions = 0;
        for (var i = 0; i < flatBoard.Length; i++)
        {
            for (var j = i + 1; j < flatBoard.Length; j++)
            {
                if (flatBoard[i] > flatBoard[j] && flatBoard[i] != 0 && flatBoard[j] != 0)
                {
                    inversions++;
                }
            }
        }
        return inversions;
    }

    private static int FindBlankRowFromBottom(int[,] state)
    {
        for (var row = Size - 1; row >= 0; row--)
        {
            for (var col = 0; col < Size; col++)
            {
                if (state[row, col] == 0)
                {
                    return Size - row;
                }
            }
        }
        return -1;
    }
}