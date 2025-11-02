using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;
using System.Collections.Generic;

namespace SpaceInvadersClone.GameObjects;

public class Player
{
    // The AnimatedSprite used to draw the player.
    private readonly AnimatedSprite _sprite;

    // The projectile sprite.
    private readonly Sprite _projectile;

    // A list of Player's Projectile.
    private readonly List<Projectile> _projectileList;

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
    public Player(AnimatedSprite sprite, Sprite projectileSprite)
    {
        _sprite = sprite;
        _projectile = projectileSprite;
        _projectileList = [];
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
        if (GameController.Shoot())
        {
            CreateNewProjectile();
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

        // Manage the projectile.
        UpdateProjectile();
        CheckProjectileCollision();
    }

    /// <summary>
    /// Draws the player.
    /// </summary>
    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, Position);

        DrawProjectile();
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

    private void AddProjectile(Projectile projectile)
    {
        _projectileList.Add(projectile);
    }

    private void RemoveProjectile(int index)
    {
        _projectileList.RemoveAt(index);
    }

    private void CreateNewProjectile()
    {
        Projectile newProjectile = new(_projectile);

        // Place the Projectile in the middle of the player sprite.
        const float HALF = 0.5f;

        // Dividing in half isn't enought to place the projectile exactly
        // in the middle, so the x-coordinate will be reduced by 1.
        // It sucks to see the projectile just one pixel off, so this helps.
        float middle = (_sprite.Width * HALF) - 1;

        // The projectile is on the player sprite, so it's y-coordinate
        // will be increased slightly.
        float correctYPosition = newProjectile.Height;

        newProjectile.Position.X = Position.X + middle;
        newProjectile.Position.Y = Position.Y - correctYPosition;

        AddProjectile(newProjectile);
    }

    private void UpdateProjectile()
    {
        for (int i = 0; i < _projectileList.Count; i++)
        {
            _projectileList[i].Update();
        }
    }

    private void DrawProjectile()
    {
        for (int i = 0; i < _projectileList.Count; i++)
        {
            _projectileList[i].Draw();
        }
    }

    private void CheckProjectileCollision()
    {
        Rectangle roomBounds = Core.GraphicsDevice
                                   .PresentationParameters
                                   .Bounds;

        for (int i = 0; i < _projectileList.Count; i++)
        {
            Rectangle projectileBounds = _projectileList[i].GetBounds();
            if (projectileBounds.Top <= roomBounds.Top)
            {
                RemoveProjectile(i);
                i--;
            }
        }
    }
}
