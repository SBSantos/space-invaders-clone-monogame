using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Roach
{
    // The AnimatedSprite
    private readonly AnimatedSprite _sprite;

    // The roach position
    private Vector2 _position;

    /// <summary>
    /// Creates a new Roach.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the roach.
    /// </param>
    public Roach(AnimatedSprite sprite)
    {
        _sprite = sprite;
    }

    /// <summary>
    /// Initializes the roach, can be used to reset it back
    /// to an initial state.
    /// </summary>
    public void Initialize(Vector2 startPosition)
    {
        _position = startPosition;
    }

    /// <summary>
    /// Updates the roach.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    public void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);
    }

    /// <summary>
    /// Draws the roach.
    /// </summary>
    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, _position);
    }

    /// <summary>
    /// Returns a Rectangle value that represents collsiion bounds.
    /// </summary>
    /// <returns>A Rectangle value.</returns>
    public Rectangle GetBounds()
    {
        return new Rectangle(
            (int)_position.X,
            (int)_position.Y,
            (int)_sprite.Width,
            (int)_sprite.Height
        );
    }
}
