using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvadersClone.GameObjects;

public enum Owner
{
    Player,
    Enemy
}

public class Projectile
{
    // The TextureRegion of this projectile.
    // There's no animation, in this case use TextureRegion.
    public Sprite Sprite;

    /// <summary>
    /// Bullet position.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The Width of this projectile.
    /// </summary>
    public float Width => Sprite.Width;

    /// <summary>
    /// The Height of this projectile.
    /// </summary>
    public float Height => Sprite.Height;

    /// <summary>
    /// The Owner of this projectile.
    /// </summary>
    public Owner Owner { get; set; }

    public float MovementSpeed { get; set; }

    /// <summary>
    /// Create a new Projectile.
    /// </summary>
    /// <param name="sprite">
    /// The sprite of this Projectile.
    /// </param>
    public Projectile(Sprite sprite)
    {
        Sprite = sprite;
    }

    /// <summary>
    /// Get the bounds of this Projectile.
    /// </summary>
    public virtual Rectangle GetBounds()
    {
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)Width,
            (int)Height
        );
    }

    /// <summary>
    /// Update the Projectile.
    /// </summary>
    public virtual void Update() {}

    /// <summary>
    /// Draw the Projectile.
    /// </summary>
    public virtual void Draw()
    {
        Sprite.Draw(Core.SpriteBatch, Position);
    }
}
