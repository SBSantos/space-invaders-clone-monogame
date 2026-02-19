using System;
using System.Collections.Generic;
using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvadersClone.GameObjects;

public class Enemy
{
    #region Fields
    // The animated sprite.
    protected AnimatedSprite Sprite;

    // The Laser sprite.
    protected Sprite LaserSprite;

    // The enemy position
    public Vector2 Position;

    // The value, in milliseconds, to move the enemy down
    private readonly double _moveDownValue;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or Set the row of multiples enemies,
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Time until it hit the threshold.
    /// </summary>
    public TimeSpan Time { get; set; }

    /// <summary>
    /// The threshold for the next move.
    /// </summary>
    public TimeSpan Threshold { get; set; }

    /// <summary>
    /// The time threshold value, in milliseconds,
    /// for the enemy to shoot
    /// </summary>
    public int ShootThreshold { get; set; }

    /// <summary>
    /// The tilemap for this enemy.
    /// </summary>
    public Tilemap Tilemap { get; set; }

    /// <summary>
    /// Define the distance to the next tile.
    /// </summary>
    public float Pace { get; set; }

    /// <summary>
    /// The value for the enemy to jump for the next row.
    /// </summary>
    public float NextRow { get; set; }

    /// <summary>
    /// Defines if the enemy is moving forward or backward.
    /// </summary>
    public bool IsMovingBackward { get; set; }

    /// <summary>
    /// Defines if the is moving down to the next row.
    /// </summary>
    public bool IsMovingDown { get; set; }

    /// <summary>
    /// The list of enemy's laser.
    /// </summary>
    public List<Laser> Lasers { get; set; }

    /// <summary>
    /// Timer for the enemy to move down.
    /// </summary>
    public TimeSpan MoveDownTimer { get; set; }

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
    /// <param name="tilemap">
    /// The tilemap.
    /// </param>
    /// <param name="row">
    /// The value of the row where the enemy should be.
    /// </param>
    public Enemy(
        AnimatedSprite sprite,
        Sprite laserSprite,
        Tilemap tilemap,
        int row
    )
    {
        Sprite = sprite;
        LaserSprite = laserSprite;
        Lasers = [];
        Tilemap = tilemap;
        Pace = 0.5f;
        Row = row;
        Threshold = sprite.Animation.Delay;
        ShootThreshold = 1300;
        _moveDownValue = 500d;
        MoveDownTimer = TimeSpan.FromMilliseconds(_moveDownValue);
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
        Time += gameTime.ElapsedGameTime;

        if (Time >= Threshold)
        {
            // Update the animation
            Sprite.Update();

            // Always reset the time to zero
            Time -= Threshold;
        }

        Movement();
        MoveDown(IsMovingDown, gameTime);

        UpdateLaser();
    }

    /// <summary>
    /// Draws the enemy.
    /// </summary>
    public virtual void Draw()
    {
        Sprite.Draw(Core.SpriteBatch, Position);

        DrawLaser();
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

    // Moves the enemies left or right
    private void Movement()
    {
        if (IsMovingBackward)
        {
            Position.X -= Pace;
            return;
        }

        Position.X += Pace;
    }

    private void MoveDown(bool movingDown, GameTime gameTime)
    {
        if (movingDown)
        {
            Position.Y += Pace;

            MoveDownTimer -= gameTime.ElapsedGameTime;
            if (MoveDownTimer <= TimeSpan.Zero)
            {
                IsMovingDown = false;
                MoveDownTimer = TimeSpan.FromMilliseconds(_moveDownValue);
            }
        }
    }

    /// <summary>
    /// Manages the direction change of the enemies when
    /// they reaches a limit of area
    /// </summary>
    /// <param name="enemies">
    /// A enemy list
    /// </param>
    /// <param name="roomBounds">
    /// A rectangle representing the boundaries of the room.
    /// </param>
    public virtual void ChangeDirection(
        List<Enemy> enemies,
        Rectangle roomBounds
    )
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Rectangle enemyBounds = enemies[i].GetBounds();

            if (enemyBounds.Right > roomBounds.Right)
            {
                IsMovingBackward = true;
                IsMovingDown = true;
            }

            float leftSidePos = enemyBounds.X - (enemyBounds.Width / 2);
            if (leftSidePos < roomBounds.Left)
            {
                IsMovingBackward = false;
                IsMovingDown = true;
            }
        }
    }

    /// <summary>
    /// Checks for player collision.
    /// </summary>
    /// <param name="player">
    /// The player to check collision.
    /// </param>
    /// <param name="roomBounds">
    /// A rectangle representing the boundaries of the room.
    /// </param>
    public virtual void CheckCollision(
        Player player,
        Rectangle roomBounds)
    {
        Rectangle playerBounds = player.GetBounds();
        for (int i = 0; i < Lasers.Count; i++)
        {
            Rectangle laserBounds = Lasers[i].GetBounds();
            if (laserBounds.Bottom >= roomBounds.Bottom)
            {
                RemoveLaser(i);
                i--;
            }
            else if (laserBounds.Intersects(playerBounds))
            {
                RemoveLaser(i);
                // player.Initialize(player.ResetPlayerPosition);
                i--;
            }

            for (int j = 0; j < player.Bullets.Count; j++)
            {
                Rectangle bulletBounds = player.Bullets[j].GetBounds();
                if (laserBounds.Intersects(bulletBounds))
                {
                    RemoveLaser(i);
                    i--;

                    player.RemoveBullet(j);
                    j--;
                }
            }
        }
    }

    private void AddLaser(Laser laser)
    {
        Lasers.Add(laser);
    }

    private void RemoveLaser(int index)
    {
        Lasers.RemoveAt(index);
    }

    /// <summary>
    /// Create a shoot a new Laser sprite.
    /// </summary>
    public void ShootLaser()
    {
        Laser newLaser = new(LaserSprite);

        const float HALF = 0.5f;

        float middle = (Sprite.Width * HALF) - (LaserSprite.Width * HALF);

        float correctYPosition = Sprite.Height;

        newLaser.Position.X = Position.X + middle;
        newLaser.Position.Y = Position.Y + correctYPosition;

        AddLaser(newLaser);
    }

    private void UpdateLaser()
    {
        for (int i = 0; i < Lasers.Count; i++)
        {
            Lasers[i].Update();
        }
    }

    private void DrawLaser()
    {
        for (int i = 0; i < Lasers.Count; i++)
        {
            Lasers[i].Draw();
        }
    }
    #endregion
}
