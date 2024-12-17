using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.Scenes;

public static class SceneManager
{
    private static readonly Dictionary<Type, Scene> Scenes = new()
    {
        { typeof(MenuScene), new MenuScene() },
        { typeof(PuzzleScene), new PuzzleScene() },
        { typeof(VoidScene), new VoidScene() }
    };
    
    private static Scene _activeScene = Scenes[typeof(MenuScene)];
    
    public static void SwitchScene<T>()
        where T : Scene
    {
        _activeScene.Cleanup();
        
        _activeScene = !Scenes.ContainsKey(typeof(T)) ? Scenes[typeof(VoidScene)] : Scenes[typeof(T)];
        if (_activeScene is VoidScene)
        {
            Console.WriteLine($"Unable to find {typeof(T).Name} in scenes list, is it not defined?");
        }
        Console.WriteLine($"Switching to scene {_activeScene.Name}");
        
        _activeScene.Init();
    }

    public static void Initialize()
    {
        _activeScene.Init();
    }
    
    public static void LoadContent(ContentManager content)
    {
        foreach (var scene in Scenes.Values)
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