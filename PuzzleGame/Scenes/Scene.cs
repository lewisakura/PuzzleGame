using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGame.Scenes;

public abstract class Scene(string name)
{
    public string Name { get; init; } = name;

    public virtual void Init() {}
    public virtual void Cleanup() {}
    public virtual void LoadContent(ContentManager content) {}
    
    public virtual void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {}
    public virtual void Update(GameTime gameTime) {}
}