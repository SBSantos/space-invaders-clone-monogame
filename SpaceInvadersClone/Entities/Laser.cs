using GameLibrary;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.Entities;

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
        MovementSpeed = 250f;
    }

    public override void Update()
    {
        Position.Y += MovementSpeed * Core.DeltaTime;
    }
}