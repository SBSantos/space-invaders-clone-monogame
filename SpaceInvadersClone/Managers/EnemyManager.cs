using System;
using System.Collections.Generic;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvadersClone.Entities;
using SpaceInvadersClone.Systems;

namespace SpaceInvadersClone.Managers;

public class EnemyManager
{
    // Enemy's shooting time.
    private TimeSpan _shootingTimer;

    // The threshold for the next shot.
    private readonly TimeSpan _shootingTimerThreshold;

    // Randomly chooses a enemy to shoot.
    private readonly Random _random;

    // The tilemap value.
    private readonly Tilemap _tilemap;

    // Row value of the enemy grid.
    private const int ROWS = 11;

    // Column value of the enemy grid.
    private const int COLUMNS = 5;

    /// <summary>
    /// The enemy formation system class.
    /// </summary>
    public EnemyFormationSystem EnemyFormation { get; }

    /// <summary>
    /// Create a new EnemyManager.
    /// </summary>
    /// <param name="tilemap">The tilemap value.</param>
    public EnemyManager(Tilemap tilemap)
    {
        _random = new();
        _tilemap = tilemap;
        EnemyFormation = new(tilemap);
        _shootingTimerThreshold = TimeSpan.FromMilliseconds(1300);
    }

    /// <summary>
    /// Initializes the EnemyManager.
    /// </summary>
    public void Initialize()
    {
        for (int i = 0; i < EnemyFormation.Enemies.Count; i++)
        {
            float x = EnemyFormation.Enemies[i].Row * _tilemap.TileWidth;
            float y = EnemyFormation.Enemies[i].Column * _tilemap.TileHeight;

            EnemyFormation.Enemies[i].Initialize(x, y);
        }
    }

    /// <summary>
    /// Load the content of the enemies.
    /// </summary>
    /// <param name="atlas">The texture atlas to load.</param>
    /// <param name="scale">The scale value.</param>
    public void LoadContent(TextureAtlas atlas, float scale)
    {
        // Enemy's laser
        Sprite laserSprite = atlas.CreateSprite(
            "laser"
        );
        laserSprite.Scale = new(scale, scale);

        // The enemy formation will be 2 tiles of distance to the right and below.
        int offset = 2;
        for (int x = 0; x < ROWS; x++)
        {
            for (int y = 0; y < COLUMNS; y++)
            {
                AnimatedSprite sprite;

                int row = x + offset;
                int column = y + offset;

                if (y == 0)
                {
                    sprite = atlas.CreateAnimatedSprite("wavey-animation");
                    sprite.Scale = new(scale, scale);

                    Wavey wavey = new(
                        sprite,
                        laserSprite,
                        row, column
                    );

                    EnemyFormation.Enemies.Add(wavey);
                }
                else if (y > 0 && y <= 2)
                {
                    sprite = atlas.CreateAnimatedSprite("roach-animation");
                    sprite.Scale = new(scale, scale);

                    Roach roach = new(
                        sprite,
                        laserSprite,
                        row, column
                    );

                    EnemyFormation.Enemies.Add(roach);
                }
                else if (y > 2 && y < 5)
                {
                    sprite = atlas.CreateAnimatedSprite("bigcrimson-animation");
                    sprite.Scale = new(scale, scale);

                    BigCrimson bigCrimson = new(
                        sprite,
                        laserSprite,
                        row, column
                    );

                    EnemyFormation.Enemies.Add(bigCrimson);
                }
                else
                {
                    sprite = atlas.CreateAnimatedSprite("roach-animation");
                    sprite.Scale = new(scale, scale);

                    Roach roach = new(
                        sprite,
                        laserSprite,
                        row, column
                    );

                    EnemyFormation.Enemies.Add(roach);
                }
            }
        }
    }

    /// <summary>
    /// Update the enemies.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for current update cycle.</param>
    /// <param name="lasers">A list of enemy lasers.</param>
    /// <param name="player">The player.</param>
    /// <param name="roomBounds">A rectangle representing the boundaries of the room.</param>
    public void Update(
        GameTime gameTime,
        List<Laser> lasers,
        Player player,
        Rectangle roomBounds
    )
    {
        if (player.PlayerState == PlayerState.Dead) { return; }

        // update the formation enemy group, it's movement and 
        // if hit the borders of the map.
        EnemyFormation.Update(roomBounds);

        // update the sprite and shooting time.
        _shootingTimer += gameTime.ElapsedGameTime;
        for (int i = 0; i < EnemyFormation.Enemies.Count; i++)
        {
            EnemyFormation.Enemies[i].Update(gameTime);

            if (_shootingTimer >= _shootingTimerThreshold)
            {
                int index = _random.Next(EnemyFormation.Enemies.Count);
                EnemyFormation.Enemies[index].ShootLaser(lasers);
                _shootingTimer -= _shootingTimerThreshold;
            }
        }
    }

    /// <summary>
    /// Draw the enemies.
    /// </summary>
    public void Draw()
    {
        for (int i = 0; i < EnemyFormation.Enemies.Count; i++)
        {
            EnemyFormation.Enemies[i].Draw();
        }
    }
}