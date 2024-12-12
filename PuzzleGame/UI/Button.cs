using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.UI;

public class Button(Rectangle rect, string text, Action callback) : Component
{
    private static Texture2D _buttonTexture;
    private static SpriteFont _buttonFont;

    public override void Update(GameTime gameTime)
    {
        if (InputManager.MouseClickedRect(rect))
        {
            callback.Invoke();
        }
        
        base.Update(gameTime);
    }
    
    public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        if (_buttonTexture is null)
        {
            // init a texture that we can use to draw solid color
            _buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            _buttonTexture.SetData([Color.White]);
        }

        _buttonFont ??= PuzzleGame.ContentManager.Load<SpriteFont>("TileOverlay");

        var textSize = _buttonFont.MeasureString(text);
        
        spriteBatch.Draw(_buttonTexture, rect, InputManager.MouseHoveringRect(rect) ? Color.Purple : Color.Fuchsia);
        spriteBatch.DrawString(_buttonFont, text, new Vector2(rect.Center.X - textSize.X / 2, rect.Center.Y - textSize.Y / 2), Color.White);
    }
}