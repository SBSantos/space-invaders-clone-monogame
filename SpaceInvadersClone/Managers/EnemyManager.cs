using System;
using System.Collections.Generic;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvadersClone.GameObjects;
using SpaceInvadersClone.Systems;

namespace SpaceInvadersClone.Managers;

public class EnemyManager
{
    // Enemy's shooting time.
    private TimeSpan _shootingTimer;

    // The threshold for the next shot.
    private TimeSpan _shootingTimerThreshold;

    // Randomly chooses a enemy to shoot.
    private readonly Random _random;

    // The tilemap value.
    private readonly Tilemap _tilemap;

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
    }

    /// <summary>
    /// Initializes the EnemyManager.
    /// </summary>
    public void Initialize()
    {
        // The tile offset
        const int OFFSET = 2;

        // The quantity of enemies
        const int LIMIT = 11 + OFFSET;

        // The x-coordinate where the enemies should start
        int x = OFFSET;

        for (int i = 0; i < EnemyFormation.Enemies.Count; i++)
        {
            // Set the threshold value for the enemy to shoot
            int thresholdValue = EnemyFormation.Enemies[0].ShootThreshold;
            _shootingTimerThreshold = TimeSpan.FromMilliseconds(thresholdValue);

            x++;

            float xPos = x * _tilemap.TileWidth;

            // The y-coordinate of the enemies.
            float yPos = EnemyFormation.Enemies[i].Row * _tilemap.TileHeight;

            // reset the x value to it's original value
            if (x >= LIMIT) { x = OFFSET; }

            if (EnemyFormation.Enemies[i] is Wavey)
            {
                EnemyFormation.Enemies[i].Initialize(xPos, yPos);
            }
            if (EnemyFormation.Enemies[i] is Roach)
            {
                EnemyFormation.Enemies[i].Initialize(xPos, yPos);
            }
            if (EnemyFormation.Enemies[i] is BigCrimson)
            {
                EnemyFormation.Enemies[i].Initialize(xPos, yPos);
            }
        }
    }

    /// <summary>
    /// Load the content of the enemies.
    /// </summary>
    /// <param name="atlas">The texture atlas to load.</param>
    /// <param name="scale">The scale value.</param>
    public void LoadContent(
        TextureAtlas atlas,
        float scale
    )
    {
        // Enemy's laser
        Sprite laserSprite = atlas.CreateSprite(
            "laser"
        );
        laserSprite.Scale = new(scale, scale);

        // row
        const int ROW_OFFSET = 2;
        for (int i = 0; i < 5; i++)
        {
            int row = i + ROW_OFFSET;
            // column
            for (int j = 0; j < 11; j++)
            {
                AnimatedSprite sprite;

                if (i == 0)
                {
                    sprite = atlas.CreateAnimatedSprite("wavey-animation");
                    sprite.Scale = new(scale, scale);

                    Wavey wavey = new(
                        sprite,
                        laserSprite,
                        row
                    );
                    EnemyFormation.Enemies.Add(wavey);
                }

                if (i > 0 && i <= 2)
                {
                    sprite = atlas.CreateAnimatedSprite("roach-animation");
                    sprite.Scale = new(scale, scale);

                    Roach roach = new(
                        sprite,
                        laserSprite,
                        row
                    );
                    EnemyFormation.Enemies.Add(roach);
                }

                if (i > 2 && i < 5)
                {
                    sprite = atlas.CreateAnimatedSprite("bigcrimson-animation");
                    sprite.Scale = new(scale, scale);

                    BigCrimson bigCrimson = new(
                        sprite,
                        laserSprite,
                        row
                    );
                    EnemyFormation.Enemies.Add(bigCrimson);
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