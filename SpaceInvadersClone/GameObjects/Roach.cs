using System;
using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;


namespace SpaceInvadersClone.GameObjects;

public class Roach
{
    // The AnimatedSprite
    private readonly AnimatedSprite _sprite;

    // The time threshold for the next move
    private readonly TimeSpan _threshold = TimeSpan.FromSeconds(1);

    // Time until the threshold
    private TimeSpan _time;

    // Define the distance to the next tile.
    private int _pace;

    // The roach position
    public Vector2 Position;

    // Define if the enemy will move forward or backward
    public bool IsMovingBackward;

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
    public void Initialize(Vector2 startPosition, Tilemap tilemap)
    {
        Position = startPosition;
        _pace = (int)tilemap.TileWidth / 2;
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
        Movement(gameTime);
    }

    /// <summary>
    /// Draws the roach.
    /// </summary>
    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, Position);
    }

    /// <summary>
    /// Returns a Rectangle value that represents collsiion bounds.
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

    private void Movement(GameTime gameTime)
    {
        _time += gameTime.ElapsedGameTime;

        if (_time >= _threshold)
        {
            // Will move the enemies backward or forward
            if (IsMovingBackward) { Position.X -= _pace; }
            else { Position.X += _pace; }

            // Always reset the time to zero
            _time = TimeSpan.Zero;
        }
    }
}
