using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.UI;

public abstract class Component
{
    public virtual void Update(GameTime gameTime) {}
    
    public abstract void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch);
}