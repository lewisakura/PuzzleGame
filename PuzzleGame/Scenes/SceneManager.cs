using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.Scenes;

public static class SceneManager
{
    private static readonly Dictionary<Type, Scene> _scenes = new()
    {
        { typeof(MenuScene), new MenuScene() },
        { typeof(PuzzleScene), new PuzzleScene() },
        { typeof(VoidScene), new VoidScene() }
    };
    
    private static Scene _activeScene = _scenes[typeof(MenuScene)];
    
    public static void SwitchScene<T>()
        where T : Scene
    {
        _activeScene.Cleanup();

        if (!_scenes.ContainsKey(typeof(T)))
        {
            _activeScene = _scenes[typeof(VoidScene)];
        }
        else
        {
            _activeScene = _scenes[typeof(T)];
        }
        
        _activeScene.Init();
    }

    public static void Initialize()
    {
        _activeScene.Init();
    }
    
    public static void LoadContent(ContentManager content)
    {
        foreach (var scene in _scenes.Values)
        {
            scene.LoadContent(content);
        }
    }
    
    public static void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _activeScene.Draw(gameTime, graphicsDevice, spriteBatch);
    }
    
    public static void DrawUI(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _activeScene.DrawUI(gameTime, graphicsDevice, spriteBatch);
    }

    public static void Update(GameTime gameTime)
    {
        _activeScene.Update(gameTime);
    }
}