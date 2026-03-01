using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;
using GameLibrary.Graphics;
using GameLibrary.Scenes;
using SpaceInvadersClone.GameObjects;
using SpaceInvadersClone.Systems;

namespace SpaceInvadersClone.Scenes;

public class GameScene : Scene
{
    private Player _player;

    private EnemyFormationSystem _enemyFormation = new();

    private Tilemap _tilemap;

    private Rectangle _roomBounds;

    private SpriteFont _font;

    private Vector2 _scoreTextPosition;

    private Vector2 _scoreTextOrigin;

    private Vector2 _livesTextPosition;

    private Vector2 _livesTextOrigin;

    public override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        // Disable exit on escape
        Core.ExitOnEscape = false;

        Rectangle screenBounds = Core.GraphicsDevice.
                                      PresentationParameters.
                                      Bounds;

        _roomBounds = new(
            (int)_tilemap.TileWidth,
            (int)_tilemap.TileHeight,
            screenBounds.Width - (int)_tilemap.TileWidth * 2,
            screenBounds.Height - (int)_tilemap.TileHeight * 2
        );

        const float PLAYER_ROW = 1.1f;
        int row = (int)(_tilemap.Rows / PLAYER_ROW);
        int playerColumn = _tilemap.Columns / 2;

        Vector2 playerPosition = new(
            playerColumn * _tilemap.TileWidth,
            row * _tilemap.TileHeight
        );
        _player.Initialize(playerPosition);

        _enemyFormation.Initialize(_tilemap);

        _scoreTextPosition = new(
            _roomBounds.Left,
            _tilemap.TileHeight / 2
        );

        float scoreTextYOrigin = _font.MeasureString("Score").Y / 2;
        _scoreTextOrigin = new(0, scoreTextYOrigin);

        _livesTextPosition = new(
            _roomBounds.Center.X * 1.5f,
            _tilemap.TileHeight / 2
        );

        float livesTextYOrigin = _font.MeasureString("Lives").Y / 2;
        _livesTextOrigin = new(0, livesTextYOrigin);
    }

    public override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        TextureAtlas atlas = TextureAtlas.FromFile(
            content: Core.Content,
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

        _enemyFormation.LoadContent(atlas, _tilemap, SCALE);

        _font = Core.Content.Load<SpriteFont>("fonts/PressStart2P-Regular");
    }

    public override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        _player.Update(gameTime);
        _player.CheckCollision(_roomBounds, _enemyFormation);
        _enemyFormation.Update(gameTime, _player, _roomBounds);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tilemap.Draw(Core.SpriteBatch);

        _player.Draw();

        _enemyFormation.Draw();

        // Draw player score
        Core.SpriteBatch.DrawString(
            _font,
            $"Score: {_player.Score}",
            _scoreTextPosition,
            Color.White,
            0.0f,
            _scoreTextOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );

        // Draw player lives count
        Core.SpriteBatch.DrawString(
            _font,
            $"Lives: {_player.Lives}",
            _livesTextPosition,
            Color.White,
            0.0f,
            _livesTextOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );

        Core.SpriteBatch.End();

        base.Draw(gameTime);
    }
}