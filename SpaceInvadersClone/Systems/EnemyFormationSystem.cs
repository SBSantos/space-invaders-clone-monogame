using System.Collections.Generic;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvadersClone.GameObjects;

namespace SpaceInvadersClone.Systems;

public class EnemyFormationSystem
{
    // Enemies direction.
    // 1 = right, -1 = left.
    private int _direction;

    /// <summary>
    /// List of enemy.
    /// </summary>
    public List<Enemy> Enemies { get; private set; }

    /// <summary>
    /// Enemy speed value.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// The distance/pixels the enemy will descend.
    /// </summary>
    public float Descend { get; set; }

    public EnemyFormationSystem(Tilemap tilemap)
    {
        _direction = 1;
        Enemies = [];
        Speed = 20f;
        Descend = tilemap.TileHeight;
    }

    /// <summary>
    /// Update the enemy movement.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for current update cycle.
    /// </param>
    /// <param name="roomBounds">
    /// A rectangle representing the boundaries of the room.
    /// </param>
    public void Update(
        GameTime gameTime,
        Rectangle roomBounds
    )
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (CollisionSystem.CheckEnemyHitBorders(Enemies, _direction, roomBounds))
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Position.Y += Descend;
            }

            // Changes direction.
            _direction *= -1;
        }
        else
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Position.X += Speed * _direction * delta;
            }
        }
    }
}