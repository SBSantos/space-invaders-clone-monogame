using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace SpaceInvadersClone.GameObjects;

public enum PlayerState
{
    Dead,
    Alive
}

public class Player
{
    // The AnimatedSprite used to draw the player.
    private readonly AnimatedSprite _sprite;

    // The bullet sprite.
    private readonly Sprite _bullet;

    private TimeSpan _deadPlayerTimer;

    private TimeSpan _deadPlayerTimerThreshold;

    // A list of Player's bullet.
    public List<Bullet> Bullets;

    // The player position.
    public Vector2 Position;

    // The player position to reset.
    public Vector2 ResetPlayerPosition;

    private const float MOVEMENT_SPEED = 5.0f;

    public int Score { get; set; }

    public int Lives { get; set; }

    public PlayerState PlayerState { get; set; }

    /// <summary>
    /// Creates a new Player using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the player.
    /// </param>
    /// <param name="bulletSprite">
    /// The Sprite to use when drawing the bullet.
    /// </param>
    public Player(AnimatedSprite sprite, Sprite bulletSprite)
    {
        _sprite = sprite;
        _bullet = bulletSprite;
        Bullets = [];
        Score = 0;
        Lives = 3;
        PlayerState = PlayerState.Alive;
        _deadPlayerTimerThreshold = TimeSpan.FromSeconds(3);
    }

    /// <summary>
    /// Initializes the player, can be used to reset it back
    /// to an initial state.
    /// </summary>
    public void Initialize(Vector2 startingPosition)
    {
        Position = startingPosition;
        ResetPlayerPosition = startingPosition;
    }

    // Handles input given by the player.
    private void HandleInput()
    {
        // Stops animation until an action occurs
        _sprite.StopAnimation();

        // Plays the animation and move the player to the left
        if (GameController.MoveLeft())
        {
            _sprite.PlayAnimation();
            Position.X -= MOVEMENT_SPEED;
        }
        else if (GameController.MoveRight())
        {
            _sprite.PlayAnimation();
            Position.X += MOVEMENT_SPEED;
        }
        else
        {
            if (GameController.WasMouseMoved())
            {
                _sprite.PlayAnimation();
            }

            Position.X = GameController.MousePosition().X - _sprite.Width / 2;
        }

        if (GameController.Shoot())
        {
            CreateNewBullet();
        }
    }

    /// <summary>
    /// Updates the player.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    public void Update(GameTime gameTime)
    {
        // Stop updating
        if (Lives <= 0) { return; }

        // Update the animated sprite.
        _sprite.Update(gameTime);

        // When player state is dead state, switch it to alive state.
        if (PlayerState == PlayerState.Dead)
        {
            _deadPlayerTimer += gameTime.ElapsedGameTime;
            if (_deadPlayerTimer >= _deadPlayerTimerThreshold)
            {
                PlayerState = PlayerState.Alive;
                _deadPlayerTimer = TimeSpan.Zero;
            }
        }
        else
        {
            // Handle any player input.
            HandleInput();

            // Update the bullet.
            UpdateBullet();
        }
    }

    /// <summary>
    /// Draws the player.
    /// </summary>
    public void Draw()
    {
        // Stop drawing the player.
        if (Lives <= 0 || PlayerState == PlayerState.Dead) { return; }

        _sprite.Draw(Core.SpriteBatch, Position);

        DrawBullet();
    }

    /// <summary>
    /// Returns a Rectangle value that represents collision bounds
    /// of the player
    /// </summary>
    /// <returns>A Rectangle value.</returns>
    public Rectangle GetBounds()
    {
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)_sprite.Width,
            (int)_sprite.Height
        );
    }

    /// <summary>
    /// Add a bullet to the list.
    /// </summary>
    /// <param name="bullet">
    /// The bullet to add.
    /// </param>
    public void AddBullet(Bullet bullet)
    {
        Bullets.Add(bullet);
    }

    /// <summary>
    /// Removes a bullet in the list by a given index.
    /// </summary>
    /// <param name="index">
    /// The index to remove the bullet.
    /// </param>
    public void RemoveBullet(int index)
    {
        Bullets.RemoveAt(index);
    }

    private void CreateNewBullet()
    {
        Bullet newBullet = new(_bullet);

        // Place the bullet in the middle of the player sprite.
        const float HALF = 0.5f;

        // Dividing in half isn't enought to place the bullet exactly
        // in the middle, so the x-coordinate will be reduced by 1.
        // It sucks to see the bullet just one pixel off, so this helps.
        float middle = (_sprite.Width * HALF) - 1;

        // The bullet is on the player sprite, so it's y-coordinate
        // will be increased slightly.
        float correctYPosition = newBullet.Height;

        newBullet.Position.X = Position.X + middle;
        newBullet.Position.Y = Position.Y - correctYPosition;

        AddBullet(newBullet);
    }

    private void UpdateBullet()
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            Bullets[i].Update();
        }
    }

    private void DrawBullet()
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            Bullets[i].Draw();
        }
    }

    /// <summary>
    /// Checks the player collision.
    /// </summary>
    /// <param name="roomBounds">
    /// A rectangle representing the boundaries of the room
    /// </param>
    /// <param name="enemies">
    /// An Enemy list.
    /// </param>
    public void CheckCollision(
        Rectangle roomBounds,
        List<Enemy> enemies
    )
    {
        Rectangle playerBounds = GetBounds();
        if (playerBounds.Left < roomBounds.Left)
        {
            Position.X = roomBounds.Left;
        }
        else if (playerBounds.Right > roomBounds.Right)
        {
            Position.X = roomBounds.Right - playerBounds.Width;
        }

        CheckBulletCollision(enemies, roomBounds);
    }

    // Manages the collision of bullets.
    public void CheckBulletCollision(
        List<Enemy> enemies,
        Rectangle roomBounds
    )
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            Rectangle bulletBounds = Bullets[i].GetBounds();
            if (bulletBounds.Top <= roomBounds.Top)
            {
                RemoveBullet(i);
                i--;
            }

            for (int j = 0; j < enemies.Count; j++)
            {
                Rectangle enemyBounds = enemies[j].GetBounds();
                if (bulletBounds.Intersects(enemyBounds))
                {
                    Score += enemies[j].Score;
                    RemoveBullet(i);
                    enemies.RemoveAt(j);
                    i--;
                }
            }
        }
    }
}
