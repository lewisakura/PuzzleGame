using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PuzzleGame.Scenes;

namespace PuzzleGame;

public class PuzzleGame : Game
{
    private SpriteBatch _spriteBatch;

    private Song _song;
    
    public const int Size = 4;

    public const int TileSize = 96;
    public const int TilePadding = 6;

    public const int Resolution = TileSize * Size + TilePadding * (Size + 1);

    public static ContentManager ContentManager { get; private set; }

    private static Queue<Action> _queuedActions;

    public PuzzleGame()
    {
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        _queuedActions = new Queue<Action>();

        graphics.PreferredBackBufferWidth = Resolution;
        graphics.PreferredBackBufferHeight = Resolution + 128;
    }

    /// <summary>
    /// Queue an action to be called before any other updates on the next frame. Primarily used
    /// for when you want to perform an operation that needs to occur, but can't occur this frame.
    /// </summary>
    /// <param name="action">The action to queue.</param>
    public static void QueueAction(Action action)
    {
        _queuedActions.Enqueue(action);
    }
    
    protected override void Initialize()
    {
        SceneManager.Initialize();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ContentManager = Content;

        SceneManager.LoadContent(Content);

        _song = Content.Load<Song>("WinnerWinner");
        
        MediaPlayer.Volume = 0.25f;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(_song);
    }

    protected override void Update(GameTime gameTime)
    {
        while (_queuedActions.TryDequeue(out var action))
        {
            action();
        }
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        InputManager.Update();
        SceneManager.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        
        SceneManager.Draw(gameTime, GraphicsDevice, _spriteBatch);
        SceneManager.DrawUI(gameTime, GraphicsDevice, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    } 
}