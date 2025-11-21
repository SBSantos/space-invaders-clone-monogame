using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Laser : Projectile
{
    /// <summary>
    /// Create a new Laser using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The sprite of this Laser.
    /// </param>
    public Laser(Sprite sprite) : base(sprite)
    {
        Sprite = sprite;
        Owner = Owner.Enemy;
    }

    public override void Update()
    {
        Position.Y += MOVEMENT_SPEED;
    }
}

