using GameLibrary;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.Entities;

public class Bullet : Projectile
{
    /// <summary>
    /// Create a new Bullet using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The Sprite of this Bullet.
    /// </param>
    public Bullet(Sprite sprite) : base(sprite)
    {
        Sprite = sprite;
        MovementSpeed = 550f;
    }

    public override void Update()
    {
        Position.Y -= MovementSpeed * Core.DeltaTime;
    }
}