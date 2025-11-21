using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

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
        MovementSpeed = 10.0f;
        Owner = Owner.Player;
    }

    public override void Update()
    {
        Position.Y -= MovementSpeed;
    }
}

