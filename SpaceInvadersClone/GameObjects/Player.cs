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
    public List<Projectile> Projectiles;

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
        Projectiles = [];
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

        // Update the projectile.
        UpdateProjectile();
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

    /// <summary>
    /// Add a projectile to the list.
    /// </summary>
    /// <param name="projectile">
    /// The projectile to add.
    /// </param>
    public void AddProjectile(Projectile projectile)
    {
        Projectiles.Add(projectile);
    }

    /// <summary>
    /// Removes a projectile in the list by a given index.
    /// </summary>
    /// <param name="index">
    /// The index to remove the projectile.
    /// </param>
    public void RemoveProjectile(int index)
    {
        Projectiles.RemoveAt(index);
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
        for (int i = 0; i < Projectiles.Count; i++)
        {
            Projectiles[i].Update();
        }
    }

    private void DrawProjectile()
    {
        for (int i = 0; i < Projectiles.Count; i++)
        {
            Projectiles[i].Draw();
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

        CheckProjectileCollision(enemies, roomBounds);
    }

    // Manages the collision of projectiles.
    private void CheckProjectileCollision(
        List<Enemy> enemies,
        Rectangle roomBounds
    )
    {
        for (int i = 0; i < Projectiles.Count; i++)
        {
            Rectangle projectileBounds = Projectiles[i].GetBounds();
            if (projectileBounds.Top <= roomBounds.Top)
            {
                RemoveProjectile(i);
                i--;
            }

            for (int j = 0; j < enemies.Count; j++)
            {
                Rectangle enemyBounds = enemies[j].GetBounds();
                if (projectileBounds.Intersects(enemyBounds))
                {
                    RemoveProjectile(i);
                    enemies.RemoveAt(j);
                }
            }
        }
    }
}
