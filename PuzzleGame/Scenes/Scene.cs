﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuzzleGame.UI;

namespace PuzzleGame.Scenes;

public abstract class Scene(string name)
{
    public string Name { get; init; } = name;

    private List<Component> _uiComponents = [];

    public virtual void Init() {}
    public virtual void Cleanup() {}
    public virtual void LoadContent(ContentManager content) {}
    
    public virtual void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {}

    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in _uiComponents)
        {
            component.Update(gameTime);
        }
    }

    internal T AddUIComponent<T>(T component)
        where T : Component
    {
        _uiComponents.Add(component);
        return component;
    }

    internal void RemoveUIComponent<T>(T component)
        where T : Component
    {
        _uiComponents.Remove(component);
    }
    
    public void DrawUI(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        foreach (var component in _uiComponents)
        {
            component.Draw(gameTime, graphicsDevice, spriteBatch);
        }
    }
}