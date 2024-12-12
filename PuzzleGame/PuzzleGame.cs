using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleGame.Scenes;

namespace PuzzleGame;

public class PuzzleGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    public const int Size = 4;

    public const int TileSize = 96;
    public const int TilePadding = 6;

    public static ContentManager ContentManager { get; private set; }

    public PuzzleGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = (TileSize * Size) + (TilePadding * (Size + 1));
        _graphics.PreferredBackBufferHeight = (TileSize * Size) + (TilePadding * (Size + 1));
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

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
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

        SceneManager.Draw(gameTime, GraphicsDevice, _spriteBatch);

        base.Draw(gameTime);
    } 
}