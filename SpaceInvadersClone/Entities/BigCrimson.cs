using GameLibrary.Graphics;

namespace SpaceInvadersClone.Entities;

public class BigCrimson : Enemy
{
    /// <summary>
    /// Create a new Big Crimson using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the big crimson.
    /// </param>
    /// <param name="laserSprite">
    /// The Sprite to use when drawing the laser.
    /// </param>
    /// <param name="row">
    /// The row value.
    /// </param>
    public BigCrimson(
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