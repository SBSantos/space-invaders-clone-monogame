using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;
using GameLibrary.Graphics;
using SpaceInvadersClone.GameObjects;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    private Player _player;

    private readonly List<Enemy> _enemies = [];

    private Tilemap _tilemap;

    private Rectangle _roomBounds;

    private readonly Random _random = new();

    private TimeSpan ShootTime;

    private TimeSpan ShootThreshold;

    public GameLoop() : base(
        title: "SIC",
        width: 640,
        height: 480,
        fullscreen: false
    ) {}

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        Rectangle screenBounds = GraphicsDevice.PresentationParameters.Bounds;

        _roomBounds = new(
            (int)_tilemap.TileWidth,
            (int)_tilemap.TileHeight,
            screenBounds.Width - (int)_tilemap.TileWidth * 2,
            screenBounds.Height - (int)_tilemap.TileHeight * 2
        );

        const float PLAYER_ROW = 1.2f;
        int row = (int)(_tilemap.Rows / PLAYER_ROW);
        int playerColumn = _tilemap.Columns / 2;

        Vector2 playerPosition = new(
            playerColumn * _tilemap.TileWidth,
            row * _tilemap.TileHeight
        );
        _player.Initialize(playerPosition);

        // The tile offset
        const int OFFSET = 2;

        // The quantity of enemies
        const int LIMIT = 11 + OFFSET;

        // The x-coordinate where the enemies should start
        int x = OFFSET;

        for (int i = 0; i < _enemies.Count; i++)
        {
            // Set the threshold value for the enemy to shoot
            int thresholdValue = _enemies[0].ShootThreshold;
            ShootThreshold = TimeSpan.FromMilliseconds(thresholdValue);

            x++;

            float xPos = x * _tilemap.TileWidth;

            // The y-coordinate of the enemies.
            // The Layer value defines their y position on the tilemap
            float yPos = _enemies[i].Row * _tilemap.TileHeight;

            // reset the x value to it's original value
            if (x >= LIMIT) { x = OFFSET; }

            if (_enemies[i] is Wavey)
            {
                _enemies[i].Initialize(xPos, yPos);
            }
            if (_enemies[i] is Roach)
            {
                _enemies[i].Initialize(xPos, yPos);
            }
            if (_enemies[i] is BigCrimson)
            {
                _enemies[i].Initialize(xPos, yPos);
            }
        }
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        TextureAtlas atlas = TextureAtlas.FromFile(
            content: Content,
            filename: "images/atlas-definition.xml"
        );
        const float SCALE = 2.0f;

        _tilemap = Tilemap.FromFile(
            content: Content,
            filename: "images/tilemap-definition.xml"
        );
        _tilemap.Scale = new(x: SCALE, y: SCALE);

        AnimatedSprite playerAnimation = atlas.CreateAnimatedSprite(
            "player-animation"
        );
        playerAnimation.Scale = new Vector2(x: SCALE, y: SCALE);

        Sprite bulletSprite = atlas.CreateSprite(
            "bullet"
        );
        bulletSprite.Scale = new(x: SCALE, y: SCALE);

        _player = new(playerAnimation, bulletSprite);

        // Enemy's laser
        Sprite laserSprite = atlas.CreateSprite(
            "laser"
        );
        laserSprite.Scale = new(SCALE, SCALE);

        // row
        for (int i = 0; i < 5; i++)
        {
            int row = i + 1;
            // column
            for (int j = 0; j < 11; j++)
            {
                AnimatedSprite sprite;

                if (i == 0)
                {
                    sprite = atlas.CreateAnimatedSprite("wavey-animation");
                    sprite.Scale = new(SCALE, SCALE);

                    Wavey wavey = new(
                        sprite,
                        laserSprite,
                        _tilemap,
                        row
                    );
                    _enemies.Add(wavey);
                }

                if (i > 0 && i <= 2)
                {
                    sprite = atlas.CreateAnimatedSprite("roach-animation");
                    sprite.Scale = new(SCALE, SCALE);

                    Roach roach = new(
                        sprite,
                        laserSprite,
                        _tilemap,
                        row
                    );
                    _enemies.Add(roach);
                }

                if (i > 2 && i < 5)
                {
                    sprite = atlas.CreateAnimatedSprite("bigcrimson-animation");
                    sprite.Scale = new(SCALE, SCALE);

                    BigCrimson bigCrimson = new(
                        sprite,
                        laserSprite,
                        _tilemap,
                        row
                    );
                    _enemies.Add(bigCrimson);
                }
            }
        }

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        _player.Update(gameTime);
        _player.CheckCollision(_roomBounds, _enemies);

        ShootTime += gameTime.ElapsedGameTime;
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].Update(gameTime, _enemies);
            _enemies[i].ChangeDirection(_enemies, _roomBounds);
            _enemies[i].CheckCollision(_player, _roomBounds);

            if (ShootTime >= ShootThreshold)
            {
                int index = _random.Next(_enemies.Count);
                _enemies[index].ShootLaser();
                ShootTime -= ShootThreshold;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tilemap.Draw(SpriteBatch);

        _player.Draw();

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].Draw();
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
