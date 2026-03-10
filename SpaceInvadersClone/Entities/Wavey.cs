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
    public Wavey(
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