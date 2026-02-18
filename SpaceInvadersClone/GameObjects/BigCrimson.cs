using System;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class BigCrimson : Enemy
{
    /// <summary>
    /// Create a new Big Crimson using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the big crimson.
    /// </param>
    public BigCrimson(
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
        Score = 10;
    }

    public override void Initialize(
        float x,
        float y
    )
    {
        float enemyOffset = 1;

        Position = new(
            x + enemyOffset,
            y
        );
    }
}
