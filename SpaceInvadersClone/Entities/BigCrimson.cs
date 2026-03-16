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
    /// <param name="column">
    /// The column value.
    /// </param>
    public BigCrimson(
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
        Score = 10;
    }

    public override void Initialize(float x, float y)
    {
        float enemyOffset = 1;
        Position = new(x + enemyOffset, y);
    }
}