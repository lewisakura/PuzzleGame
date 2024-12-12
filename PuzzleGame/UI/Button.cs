using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.UI;

public class Button(Rectangle rect, string text, Action callback) : Component
{
    private string _text = text;
    private Rectangle _rect = rect;
    private Action _callback = callback;

    public override void Update(GameTime gameTime)
    {
        if (InputManager.MouseClickedRect(_rect))
        {
            _callback.Invoke();
        }
        
        base.Update(gameTime);
    }
    
    public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        
    }
}