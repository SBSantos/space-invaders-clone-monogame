using System.Collections.Generic;
using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvadersClone.Entities;

public class Enemy : Entity
{
    #region Fields
    // The animated sprite.
    protected AnimatedSprite Sprite;

    // The Laser sprite.
    protected Sprite LaserSprite;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or Set the row of multiples enemies,
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Gets or Set the column of multiples enemies,
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Enemy score value.
    /// </summary>
    public int Score { get; set; }
    #endregion

    /// <summary>
    /// Creates a new Enemy using a specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the enemy.
    /// </param>
    /// <param name="laserSprite">
    /// The Sprite of the enemy laser.
    /// </param>
    /// <param name="row">
    /// The value of the row where the enemy should be.
    /// </param>
    /// <param name="column">
    /// The value of the column where the enemy should be.
    /// </param>
    public Enemy(
        AnimatedSprite sprite,
        Sprite laserSprite,
        int row,
        int column
    )
    {
        Sprite = sprite;
        LaserSprite = laserSprite;
        Row = row;
        Column = column;
        Score = 0;
    }

    #region Methods
    /// <summary>
    /// Initializes the enemy, can be used to reset it back
    /// to an initial state.
    /// </summary>
    public virtual void Initialize(float x, float y)
    {
        Position = new(x, y);
    }

    /// <summary>
    /// Updates the enemy.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    public virtual void Update(GameTime gameTime)
    {
        Sprite.Update(gameTime);
    }

    /// <summary>
    /// Draws the enemy.
    /// </summary>
    public virtual void Draw()
    {
        Sprite.Draw(Core.SpriteBatch, Position);
    }

    /// <summary>
    /// Returns a Rectangle value that represents collision bounds.
    /// </summary>
    /// <returns>A Rectangle value.</returns>
    public virtual Rectangle GetBounds()
    {
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)Sprite.Width,
            (int)Sprite.Height
        );
    }

    /// <summary>
    /// Create a shoot a new Laser sprite.
    /// </summary>
    /// <param name="lasers">A list of enemy lasers.</param>
    public void ShootLaser(List<Laser> lasers)
    {
        Laser newLaser = new(LaserSprite);

        const float HALF = 0.5f;

        float middle = (Sprite.Width * HALF) - (LaserSprite.Width * HALF);

        float correctYPosition = Sprite.Height;

        newLaser.Position.X = Position.X + middle;
        newLaser.Position.Y = Position.Y + correctYPosition;

        lasers.Add(newLaser);
    }
    #endregion
}