using System;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.Entities;

public class Roach : Enemy
{
    /// <summary>
    /// Create a new Roach using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the roach.
    /// </param>
    /// <param name="laserSprite">
    /// The Sprite to use when drawing the laser.
    /// </param>
    /// <param name="row">
    /// The row value.
    /// </param>
    public Roach(
        AnimatedSprite sprite,
        Sprite laserSprite,
        int row
    ) : base(
        sprite,
        laserSprite,
        row
    )
    {
        Sprite = sprite;
        LaserSprite = laserSprite;
        Row = row;
        Score = 20;
    }

    public override void Initialize(
        float x,
        float y
    )
    {
        float enemyOffset = MathF.Sqrt(Sprite.Width / 2) + 0.5f;

        Position = new(
            x + enemyOffset,
            y
        );
    }
}