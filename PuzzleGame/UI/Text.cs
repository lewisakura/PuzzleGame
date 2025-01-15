using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.UI;

public class Text(Point centerPosition, string text) : Component
{
    private string _text = text;
    private static SpriteFont _font;
    
    public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _font ??= PuzzleGame.ContentManager.Load<SpriteFont>("TileOverlay");

        var textSize = _font.MeasureString(_text);
        spriteBatch.DrawString(_font, _text, new Vector2(centerPosition.X - textSize.X / 2, centerPosition.Y - textSize.Y / 2), Color.White);
    }

    public void UpdateText(string text)
    {
        _text = text;
    }
}