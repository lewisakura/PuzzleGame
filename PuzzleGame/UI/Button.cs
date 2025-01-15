using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.UI;

public class Button(Rectangle rect, string text, Action callback) : Component
{
    private static Texture2D _buttonTexture;
    private static SpriteFont _buttonFont;
    private static SoundEffect _buttonHover;
    private static SoundEffect _buttonClick;

    public override void Update(GameTime gameTime)
    {
        if (InputManager.MouseJustHoveredRect(rect))
        {
            _buttonHover ??= PuzzleGame.ContentManager.Load<SoundEffect>("MenuHover");
            _buttonHover.Play();
        }
        
        if (InputManager.MouseClickedRect(rect))
        {
            _buttonClick ??= PuzzleGame.ContentManager.Load<SoundEffect>("MenuSelect");
            
            _buttonClick.Play();
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