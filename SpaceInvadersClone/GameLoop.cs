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

        const float DESIRED_ROW = 1.2f;
        int row = (int)(_tilemap.Rows / DESIRED_ROW);
        int playerColumn = _tilemap.Columns / 2;

        Vector2 playerPosition = new(
            playerColumn * _tilemap.TileWidth,
            row * _tilemap.TileHeight
        );
        _player.Initialize(playerPosition);

        int offset = 6;
        int roachColumn = _tilemap.Columns;
        int roachRow = _tilemap.Rows / 7;
        for (int i = 0; i < _enemies.Count; i++)
        {
            Vector2 roachPosition = new(
                (roachColumn - offset) * _tilemap.TileWidth,
                roachRow * _tilemap.TileHeight
            );

            _enemies[i].Initialize(
                roachPosition,
                _tilemap
            );
            offset++;
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

        AnimatedSprite playerAnimation = atlas.CreateAnimatedSprite(animationName: "player-animation");
        playerAnimation.Scale = new Vector2(x: SCALE, y: SCALE);

        Sprite projectileSprite = atlas.CreateSprite(regionName: "projectile");
        projectileSprite.Scale = new(x: SCALE, y: SCALE);

        _player = new(playerAnimation, projectileSprite);

        for (int i = 0; i < 11; i++)
        {
            AnimatedSprite roachAnimation = atlas.CreateAnimatedSprite(animationName: "roach-animation");
            roachAnimation.Scale = new(SCALE, SCALE);

            Roach newEnemy = new(roachAnimation);
            _enemies.Add(newEnemy);
        }

        _tilemap = Tilemap.FromFile(
            content: Content,
            filename: "images/tilemap-definition.xml"
        );
        _tilemap.Scale = new(x: SCALE, y: SCALE);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        _player.Update(gameTime);
        _player.CheckCollision(_roomBounds, _enemies);

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].Update(gameTime);
            _enemies[i].CheckEnemyOutOfAreaLimit(_enemies, i);
            _enemies[i].CheckPlayerCollision(_player);
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
