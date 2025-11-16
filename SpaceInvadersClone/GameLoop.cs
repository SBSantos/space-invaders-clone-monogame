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

    private int _enemyRow;

    private const int ENEMY_COLUMN = 6;

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

        int _waveyColumn = ENEMY_COLUMN;
        int _roachColumnL1 = ENEMY_COLUMN;
        int _roachColumnL2 = ENEMY_COLUMN;
        int _bigCrimsonColumnL1 = ENEMY_COLUMN;
        int _bigCrimsonColumnL2 = ENEMY_COLUMN;
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] is Wavey)
            {
                _enemyRow = 0;
                _enemies[i].Initialize(
                    _enemyRow,
                    _waveyColumn,
                    _tilemap
                );
                _waveyColumn++;
            }

            if (_enemies[i] is Roach)
            {
                if (_enemies[i].Layer == 2)
                {
                    _enemyRow = 2;
                    _roachColumnL1 = _roachColumnL2;
                    _roachColumnL2++;
                }
                else { _enemyRow = 1; }

                _enemies[i].Initialize(
                    _enemyRow,
                    _roachColumnL1,
                    _tilemap
                );
                _roachColumnL1++;
            }

            if (_enemies[i] is BigCrimson)
            {
                if (_enemies[i].Layer == 2)
                {
                    _enemyRow = 4;
                    _bigCrimsonColumnL1 = _bigCrimsonColumnL2;
                    _bigCrimsonColumnL2++;
                }
                else { _enemyRow = 3; }

                _enemies[i].Initialize(
                    _enemyRow,
                    _bigCrimsonColumnL1,
                    _tilemap
                );
                _bigCrimsonColumnL1++;
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

        AnimatedSprite playerAnimation = atlas.CreateAnimatedSprite(animationName: "player-animation");
        playerAnimation.Scale = new Vector2(x: SCALE, y: SCALE);

        Sprite projectileSprite = atlas.CreateSprite(regionName: "projectile");
        projectileSprite.Scale = new(x: SCALE, y: SCALE);

        _player = new(playerAnimation, projectileSprite);

        // Wavey
        for (int i = 0; i < 11; i++)
        {
            AnimatedSprite waveyAnimation = atlas.CreateAnimatedSprite(
                "wavey-animation"
            );
            waveyAnimation.Scale = new(SCALE, SCALE);

            const int LAYER = 1;
            Wavey newWavey = new(waveyAnimation, LAYER);
            _enemies.Add(newWavey);
        }

        // Roach
        AnimatedSprite roachAnimation = atlas.CreateAnimatedSprite(
            "roach-animation"
        );
        roachAnimation.Scale = new(SCALE, SCALE);

        for (int i = 0; i < 11; i++)
        {
            const int FIRST_LAYER = 1;

            AnimatedSprite rLayer1 = roachAnimation.Clone();
            rLayer1.Scale = new(SCALE, SCALE);

            Roach enemyLayer1 = new(rLayer1, FIRST_LAYER);
            _enemies.Add(enemyLayer1);

            const int SECOND_LAYER = 2;

            AnimatedSprite rLayer2 = roachAnimation.Clone();
            rLayer2.Scale = new(SCALE, SCALE);

            Roach enemyLayer2 = new(rLayer2, SECOND_LAYER);
            _enemies.Add(enemyLayer2);
        }

        // Big Crimson
        AnimatedSprite bigCrimsonAnimation = atlas.CreateAnimatedSprite(
            "bigcrimson-animation"
        );
        bigCrimsonAnimation.Scale = new(SCALE, SCALE);

        for (int i = 0; i < 11; i++)
        {
            const int FIRST_LAYER = 1;

            AnimatedSprite bcL1 = bigCrimsonAnimation.Clone();
            bcL1.Scale = new(SCALE, SCALE);

            BigCrimson enemyLayer1 = new(bcL1, FIRST_LAYER);
            _enemies.Add(enemyLayer1);

            const int SECOND_LAYER = 2;

            AnimatedSprite bcL2 = bigCrimsonAnimation.Clone();
            bcL2.Scale = new(SCALE, SCALE);

            BigCrimson enemyLayer2 = new(bcL2, SECOND_LAYER);
            _enemies.Add(enemyLayer2);
        }

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
