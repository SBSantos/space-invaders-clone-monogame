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
    /// <param name="column">
    /// The column value.
    /// </param>
    public Roach(
        AnimatedSprite sprite,
        Sprite laserSprite,
        int row,
        int column
    ) : base(
        sprite,
        laserSprite,
        row,
        column
    )
    {
        Sprite = sprite;
        LaserSprite = laserSprite;
        Row = row;
        Column = column;
        Score = 20;
    }

    public override void Initialize(float x, float y)
    {
        float enemyOffset = MathF.Sqrt(Sprite.Width / 2) + 0.5f;
        Position = new(x + enemyOffset, y);
    }
}