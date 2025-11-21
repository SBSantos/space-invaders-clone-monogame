using System;
using Microsoft.Xna.Framework;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Roach : Enemy
{
    /// <summary>
    /// Create a new Roach using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the roach.
    /// </param>
    public Roach(
        AnimatedSprite sprite,
        Sprite laserSprite,
        Tilemap tilemap,
        int row
    ) : base(
        sprite,
        laserSprite,
        tilemap,
        row
    )
    {
        Sprite = sprite;
        LaserSprite = laserSprite;
        Row = row;
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
