using System;
using Microsoft.Xna.Framework;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Wavey : Enemy
{
    /// <summary>
    /// Create a new Wavey using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the wavey.
    /// </param>
    public Wavey(
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
        Score = 30;
    }

    public override void Initialize(
        float x,
        float y
    )
    {
        float enemyOffset = (Sprite.Width / 2) - 1;

        Position = new(
            x + enemyOffset,
            y
        );
    }
}