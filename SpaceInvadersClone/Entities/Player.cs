using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;
using System.Collections.Generic;
using System;

namespace SpaceInvadersClone.Entities;

public enum PlayerState
{
    Dead,
    Alive
}

public class Player : Entity
{
    // The AnimatedSprite used to draw the player.
    private readonly AnimatedSprite _sprite;

    // The bullet sprite.
    private readonly Sprite _bullet;

    // The timer when the player is dead.
    private TimeSpan _deadPlayerTimer;

    // The threshold of the timer.
    private TimeSpan _deadPlayerTimerThreshold;

    // The player position to reset.
    public Vector2 ResetPlayerPosition;

    private const float MOVEMENT_SPEED = 5.0f;

    /// <summary>
    /// Gets or sets the player's score.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the player's lives.
    /// </summary>
    public int Lives { get; set; }

    /// <summary>
    /// Gets or sets the player's state.
    /// </summary>
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
    private void HandleInput(List<Bullet> bullets)
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

        if (GameController.Shoot())
        {
            CreateNewBullet(bullets);
        }
    }

    /// <summary>
    /// Updates the player.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    /// <param name="bullets">
    /// A list of player's bullets.
    /// </param>
    public void Update(GameTime gameTime, List<Bullet> bullets)
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
            HandleInput(bullets);
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

    private void CreateNewBullet(List<Bullet> bullets)
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

        bullets.Add(newBullet);
    }

    /// <summary>
    /// Increase player's score.
    /// </summary>
    /// <param name="enemies">
    /// The list of enemies.
    /// </param>
    /// <param name="index">
    /// The enemy index.
    /// </param>
    public void IncreaseScore(
        List<Enemy> enemies,
        int index
    )
    {
        Score += enemies[index].Score;
    }

    /// <summary>
    /// The player's death.
    /// </summary>
    public void Death()
    {
        // if player is dead, make it imortal for a little time.

        Lives--;
        PlayerState = PlayerState.Dead;
    }
}
