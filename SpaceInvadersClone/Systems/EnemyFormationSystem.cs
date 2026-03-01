using System;
using System.Collections.Generic;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvadersClone.GameObjects;

namespace SpaceInvadersClone.Systems;

public class EnemyFormationSystem
{
    private TimeSpan _shootTime;
    private TimeSpan _shootThreshold;
    private readonly Random _random = new();

    public List<Enemy> Enemies { get; set; } = [];

    public void Initialize(Tilemap tilemap)
    {
        // The tile offset
        const int OFFSET = 2;

        // The quantity of enemies
        const int LIMIT = 11 + OFFSET;

        // The x-coordinate where the enemies should start
        int x = OFFSET;

        for (int i = 0; i < Enemies.Count; i++)
        {
            // Set the threshold value for the enemy to shoot
            int thresholdValue = Enemies[0].ShootThreshold;
            _shootThreshold = TimeSpan.FromMilliseconds(thresholdValue);

            x++;

            float xPos = x * tilemap.TileWidth;

            // The y-coordinate of the enemies.
            float yPos = Enemies[i].Row * tilemap.TileHeight;

            // reset the x value to it's original value
            if (x >= LIMIT) { x = OFFSET; }

            if (Enemies[i] is Wavey)
            {
                Enemies[i].Initialize(xPos, yPos);
            }
            if (Enemies[i] is Roach)
            {
                Enemies[i].Initialize(xPos, yPos);
            }
            if (Enemies[i] is BigCrimson)
            {
                Enemies[i].Initialize(xPos, yPos);
            }
        }
    }

    public void LoadContent(
        TextureAtlas atlas,
        Tilemap tilemap,
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
                        tilemap,
                        row
                    );
                    Enemies.Add(wavey);
                }

                if (i > 0 && i <= 2)
                {
                    sprite = atlas.CreateAnimatedSprite("roach-animation");
                    sprite.Scale = new(scale, scale);

                    Roach roach = new(
                        sprite,
                        laserSprite,
                        tilemap,
                        row
                    );
                    Enemies.Add(roach);
                }

                if (i > 2 && i < 5)
                {
                    sprite = atlas.CreateAnimatedSprite("bigcrimson-animation");
                    sprite.Scale = new(scale, scale);

                    BigCrimson bigCrimson = new(
                        sprite,
                        laserSprite,
                        tilemap,
                        row
                    );
                    Enemies.Add(bigCrimson);
                }
            }
        }
    }

    public void Update(
        GameTime gameTime,
        Player player,
        Rectangle roomBounds
    )
    {
        if (player.PlayerState == PlayerState.Dead) { return; }

        _shootTime += gameTime.ElapsedGameTime;
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].Update(gameTime);
            Enemies[i].UpdateMovement(gameTime);
            Enemies[i].UpdateLaser();
            Enemies[i].ChangeDirection(Enemies, roomBounds);
            Enemies[i].CheckCollision(player, roomBounds);

            if (_shootTime >= _shootThreshold)
            {
                int index = _random.Next(Enemies.Count);
                Enemies[index].ShootLaser();
                _shootTime -= _shootThreshold;
            }
        }
    }

    public void Draw()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].Draw();
            Enemies[i].DrawLaser();
        }
    }
}