using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Player
{
    // The AnimatedSprite used to draw the player.
    private readonly AnimatedSprite _sprite;

    // The player position.
    public Vector2 Position;

    // The player position to reset.
    public Vector2 ResetPlayerPosition;

    private const float MOVEMENT_SPEED = 5.0f;

    /// <summary>
    /// Creates a new Player using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the player.
    /// </param>
    public Player(AnimatedSprite sprite)
    {
        _sprite = sprite;
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
    }

    /// <summary>
    /// Updates the player.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    public void Update(GameTime gameTime)
    {
        // Update the animated sprite.
        _sprite.Update(gameTime);

        // Handle any player input.
        HandleInput();
    }

    /// <summary>
    /// Draws the player.
    /// </summary>
    public void Draw()
    {
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
}
