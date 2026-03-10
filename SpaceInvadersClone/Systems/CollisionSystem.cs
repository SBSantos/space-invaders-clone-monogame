using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceInvadersClone.Entities;

namespace SpaceInvadersClone.Systems;

public static class CollisionSystem
{
    /// <summary>
    /// Check the player collision against enemies.
    /// </summary>
    /// <param name="player">
    /// The player to check collision.
    /// </param>
    /// <param name="bullets">
    /// A list of player's bullets.
    /// </param>
    /// <param name="enemies">
    /// The list of enemies to check collision.
    /// </param>
    /// <param name="roomBounds">
    /// A rectangle representing the boundaries of the room.
    /// </param>
    public static void CheckPlayerVsEnemyCollision(
        Player player,
        List<Bullet> bullets,
        List<Enemy> enemies,
        Rectangle roomBounds
    )
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            Rectangle bulletBounds = bullets[i].GetBounds();
            if (bulletBounds.Top <= roomBounds.Top)
            {
                bullets.RemoveAt(i);
                i--;
            }

            for (int j = 0; j < enemies.Count; j++)
            {
                Rectangle enemyBounds = enemies[j].GetBounds();
                if (bulletBounds.Intersects(enemyBounds))
                {
                    player.IncreaseScore(enemies, j);

                    bullets.RemoveAt(i);
                    i--;

                    enemies.RemoveAt(j);
                    j--;
                }
            }
        }
    }

    /// <summary>
    /// Checks the player's collision on the boundaries of the room.
    /// </summary>
    /// <param name="player">
    /// The player.
    /// </param>
    /// <param name="roomBounds">
    /// A rectangle representing the boundaries of the room
    /// </param>
    public static void CheckPlayerHitMapEdge(
        Player player,
        Rectangle roomBounds
    )
    {
        Rectangle playerBounds = player.GetBounds();
        if (playerBounds.Left < roomBounds.Left)
        {
            player.Position.X = roomBounds.Left;
        }
        else if (playerBounds.Right > roomBounds.Right)
        {
            player.Position.X = roomBounds.Right - playerBounds.Width;
        }
    }

    /// <summary>
    /// Check enemies collision against the player.
    /// </summary>
    /// <param name="enemies">
    /// The list of enemies to check collision.
    /// </param>
    /// <param name="enemyIndex">
    /// The enemy index.
    /// </param>
    /// <param name="player">
    /// The player to check collision.
    /// </param>
    public static void CheckEnemyVsPlayerCollision(
        List<Enemy> enemies,
        int enemyIndex,
        Player player
    )
    {
        Rectangle enemyBounds = enemies[enemyIndex].GetBounds();

        Rectangle playerBounds = player.GetBounds();

        if (enemyBounds.Intersects(playerBounds))
        {
            // Player's death logic here, but to avoid infinite death 
            // this will be blanked.
        }
    }

    /// <summary>
    /// Check enemy lasers collision against the player.
    /// </summary>
    /// <param name="lasers">A list of enemy lasers.</param>
    /// <param name="player">The player to check collision.</param>
    /// <param name="roomBounds">A rectangle representing the boundaries of the room.</param>
    public static void CheckLaserVsPlayerCollision(
        List<Laser> lasers,
        Player player,
        Rectangle roomBounds
    )
    {
        Rectangle playerBounds = player.GetBounds();

        for (int i = 0; i < lasers.Count; i++)
        {
            Rectangle laserBounds = lasers[i].GetBounds();

            if (laserBounds.Bottom >= roomBounds.Bottom)
            {
                lasers.RemoveAt(i);
                i--;
            }
            else if (laserBounds.Intersects(playerBounds))
            {
                lasers.RemoveAt(i);
                i--;

                player.Death();
            }
        }
    }

    /// <summary>
    /// Check both enemies and player's projectiles.
    /// </summary>
    /// <param name="bullets">
    /// A list of player's bullets.
    /// </param>
    /// <param name="lasers">
    /// A list of enemy lasers.
    /// </param>
    public static void CheckLaserVsBulletCollision(
        List<Laser> lasers,
        List<Bullet> bullets
    )
    {
        for (int i = 0; i < lasers.Count; i++)
        {
            Rectangle laserBounds = lasers[i].GetBounds();

            for (int j = 0; j < bullets.Count; j++)
            {
                Rectangle bulletBounds = bullets[j].GetBounds();
                if (bulletBounds.Intersects(laserBounds))
                {
                    lasers.RemoveAt(i);
                    i--;

                    bullets.RemoveAt(j);
                    j--;
                }
            }
        }
    }

    /// <summary>
    /// Check enemies collision on the boundaries of the room.
    /// </summary>
    /// <param name="enemies">The enemy list</param>
    /// <param name="direction">Enemy's direction.</param>
    /// <param name="roomBounds">A rectangle representing the boundaries of the room.</param>
    /// <returns>True if collides. Otherwise, false.</returns>
    public static bool CheckEnemyHitMapEdge(
        List<Enemy> enemies,
        int direction,
        Rectangle roomBounds
    )
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Rectangle enemyBounds = enemies[i].GetBounds();

            float leftSidePos = enemyBounds.X - (enemyBounds.Width / 2);

            if (direction == 1)
            {
                if (enemyBounds.Right >= roomBounds.Right)
                {
                    return true;
                }
            }
            else
            {
                if (leftSidePos <= roomBounds.Left)
                {
                    return true;
                }
            }
        }

        return false;
    }
}