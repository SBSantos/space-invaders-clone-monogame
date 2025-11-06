using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;
using GameLibrary.Graphics;
using SpaceInvadersClone.GameObjects;
using System.Linq;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    private Player _player;
    private readonly List<Roach> _roachs = [];

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
        for (int i = 0; i < _roachs.Count; i++)
        {
            Vector2 roachPosition = new(
                (roachColumn - offset) * _tilemap.TileWidth,
                roachRow * _tilemap.TileHeight
            );

            _roachs[i].Initialize(roachPosition, _tilemap);
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

        AnimatedSprite roachAnimation = atlas.CreateAnimatedSprite(animationName: "roach-animation");
        roachAnimation.Scale = new Vector2(x: SCALE, y: SCALE);
        roachAnimation.StopAnimation();

        for (int i = 0; i < 11; i++)
        {
            Roach newRoach = new(roachAnimation);
            _roachs.Add(newRoach);
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
        CheckProjectileCollisionOnEnemies();

        for (int i = 0; i < _roachs.Count; i++)
        {
            _roachs[i].Update(gameTime);
        }
        CheckEnemyOutOfAreaLimit();

        CheckCollision();

        base.Update(gameTime);
    }

    private void CheckCollision()
    {
        Rectangle playerBounds = _player.GetBounds();

        if (playerBounds.Left < _roomBounds.Left)
        {
            _player.Position.X = _roomBounds.Left;
        }
        else if (playerBounds.Right > _roomBounds.Right)
        {
            _player.Position.X = _roomBounds.Right - playerBounds.Width;
        }

        for (int i = 0; i < _roachs.Count; i++)
        {
            Rectangle roachBounds = _roachs[i].GetBounds();
            if (playerBounds.Intersects(roachBounds))
            {
                _player.Initialize(_player.ResetPlayerPosition);
            }
        }
    }

    private void CheckProjectileCollisionOnEnemies()
    {
        for (int i = 0; i < _player.Projectiles.Count; i++)
        {
            Rectangle projectileBounds = _player.Projectiles[i]
                                                .GetBounds();

            for (int j = 0; j < _roachs.Count; j++)
            {
                Rectangle roachBounds = _roachs[j].GetBounds();

                if (projectileBounds.Intersects(roachBounds))
                {
                    _player.RemoveProjectile(i);
                    i--;

                    _roachs.RemoveAt(j);
                    j--;
                }
            }
        }
    }

    private void CheckEnemyOutOfAreaLimit()
    {
        const int RIGHT_LIMIT = 4;
        const int LEFT_LIMIT = 6;
        const float NEXT_TILE = 0.5f;

        float rightSideLimit = (_tilemap.Columns - RIGHT_LIMIT) * _tilemap.TileWidth;
        float leftSideLimit = _tilemap.Columns / LEFT_LIMIT * _tilemap.TileWidth;

        for (int i = 0; i < _roachs.Count; i++)
        {
            Rectangle first = _roachs[0].GetBounds();
            if (first.X > rightSideLimit)
            {
                _roachs[i].Position.Y += NEXT_TILE;
                _roachs[i].IsMovingBackward = true;
            }

            Rectangle last = _roachs.LastOrDefault()?
                                    .GetBounds() ?? default;

            float lastXPos = last.X - (last.Width / 2);
            if (lastXPos < leftSideLimit)
            {
                _roachs[i].Position.Y += NEXT_TILE;
                _roachs[i].IsMovingBackward = false;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tilemap.Draw(SpriteBatch);

        _player.Draw();

        for (int i = 0; i < _roachs.Count; i++)
        {
            _roachs[i].Draw();
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
