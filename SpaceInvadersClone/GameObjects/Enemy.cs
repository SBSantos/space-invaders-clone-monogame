using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvadersClone.GameObjects;

public class Enemy
{
    // The animated sprite.
    protected AnimatedSprite Sprite;

    // The enemy position
    public Vector2 position;

    /// <summary>
    /// Gets or Set the layer of multiples enemies,
    /// for new row of enemies.
    /// </summary>
    public int Layer { get; set; }

    /// <summary>
    /// Time until it hit the threshold.
    /// </summary>
    public TimeSpan Time { get; set; }

    /// <summary>
    /// The threshold for the next move.
    /// </summary>
    public TimeSpan Threshold { get; set; }

    /// <summary>
    /// The tilemap for this enemy.
    /// </summary>
    public Tilemap Tilemap { get; set; }

    /// <summary>
    /// Define the distance to the next tile.
    /// </summary>
    public int Pace { get; set; }

    /// <summary>
    /// The value for the enemy to jump for the next row.
    /// </summary>
    public Vector2 NextRow { get; set; } = Vector2.UnitY / 2;

    /// <summary>
    /// Defines if the enemy is moving forward or backward.
    /// </summary>
    public bool IsMovingBackward { get; set; }

    /// <summary>
    /// Creates a new Enemy using a specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the enemy.
    /// </param>
    public Enemy(AnimatedSprite sprite, int layer)
    {
        Sprite = sprite;
        Layer = layer;
        Threshold = sprite.Animation.Delay;
    }

    /// <summary>
    /// Initializes the enemy, can be used to reset it back
    /// to an initial state.
    /// </summary>
    public virtual void Initialize(
        int desiredRow,
        int desiredColumn,
        Tilemap tilemap
    )
    {
        int enemyColumn = tilemap.Columns;
        int enemyRow = tilemap.Rows / 7;

        position = new(
            (enemyColumn - desiredColumn) * tilemap.TileWidth,
            (enemyRow + desiredRow) * tilemap.TileHeight
        );

        Tilemap = tilemap;
        Pace = (int)tilemap.TileWidth / 2;
    }

    /// <summary>
    /// Updates the enemy.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    public virtual void Update(GameTime gameTime)
    {
        Time += gameTime.ElapsedGameTime;

        if (Time >= Threshold)
        {
            // Update the animation
            Sprite.Update();

            // Will move the enemies backward or forward
            if (IsMovingBackward) { position.X -= Pace; }
            else { position.X += Pace; }

            // Always reset the time to zero
            Time -= Threshold;
        }
    }

    /// <summary>
    /// Draws the enemy.
    /// </summary>
    public virtual void Draw()
    {
        Sprite.Draw(Core.SpriteBatch, position);
    }

    /// <summary>
    /// Returns a Rectangle value that represents collision bounds.
    /// </summary>
    /// <returns>A Rectangle value.</returns>
    public virtual Rectangle GetBounds()
    {
        return new Rectangle(
            (int)position.X,
            (int)position.Y,
            (int)Sprite.Width,
            (int)Sprite.Height
        );
    }

    /// <summary>
    /// Check if a enemy is out of it's area limit.
    /// </summary>
    /// <param name="enemies">
    /// An Enemy list.
    /// </param>
    /// <param name="index">
    /// The Enemy index.
    /// </param>
    public virtual void CheckEnemyOutOfAreaLimit(
        List<Enemy> enemies,
        int index
    )
    {
        const int RIGHT_LIMIT = 4;
        const int LEFT_LIMIT = 6;

        float rightSideLimit = (Tilemap.Columns - RIGHT_LIMIT)
                               * Tilemap.TileWidth;

        float leftSideLimit = Tilemap.Columns / LEFT_LIMIT
                              * Tilemap.TileWidth;

        var firstLine = enemies.Where(x => x is Wavey)
                               .ToList();

        Rectangle firstBound = firstLine.FirstOrDefault()?
                                        .GetBounds()?? default;

        Rectangle lastBound = firstLine.LastOrDefault()?
                                       .GetBounds() ?? default;

        if (firstBound.X > rightSideLimit)
        {
            enemies[index].position += NextRow;
            enemies[index].IsMovingBackward = true;
        }

        float lastXPos = lastBound.X - (lastBound.Width / 2);
        if (lastXPos < leftSideLimit)
        {
            enemies[index].position += NextRow;
            enemies[index].IsMovingBackward = false;
        }
    }

    /// <summary>
    /// Checks for player collision.
    /// </summary>
    /// <param name="player">
    /// The player to check collision.
    /// </param>
    public virtual void CheckPlayerCollision(Player player)
    {
        if (GetBounds().Intersects(player.GetBounds()))
        {
            player.Initialize(player.ResetPlayerPosition);
        }
    }
}
