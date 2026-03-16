using GameLibrary.Graphics;

namespace SpaceInvadersClone.Entities;

public class Wavey : Enemy
{
    /// <summary>
    /// Create a new Wavey using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the wavey.
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
    public Wavey(
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
        Score = 30;
    }

    public override void Initialize(float x, float y)
    {
        float enemyOffset = (Sprite.Width / 2) - 1;
        Position = new(x + enemyOffset, y);
    }
}