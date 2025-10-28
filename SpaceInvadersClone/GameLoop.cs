using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Graphics;
using GameLibrary.Input;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    private AnimatedSprite _player;
    private AnimatedSprite _croach;

    private Vector2 _playerPosition;
    private Vector2 _resetPlayerPosition;

    private Vector2 _croachPosition;

    private Tilemap _tilemap;

    private Rectangle _roomBounds;

    private const float MOVEMENT_SPEED = 5.0f;

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
        _playerPosition = new(
            x: playerColumn * _tilemap.TileWidth,
            y: row * _tilemap.TileHeight
        );
        _resetPlayerPosition = _playerPosition;

        int croachColumn = (int)(_tilemap.Columns / DESIRED_ROW);
        _croachPosition = new(
            x: croachColumn * _tilemap.TileWidth,
            y: row * _tilemap.TileHeight
        );
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        TextureAtlas atlas = TextureAtlas.FromFile(
            content: Content,
            filename: "images/atlas-definition.xml"
        );
        const float SCALE = 2.0f;

        _player = atlas.CreateAnimatedSprite(animationName: "player-animation");
        _player.Scale = new Vector2(x: SCALE, y: SCALE);

        _croach = atlas.CreateAnimatedSprite(animationName: "croach-animation");
        _croach.Scale = new Vector2(x: SCALE, y: SCALE);

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
        _croach.Update(gameTime);

        CheckKeyboardInput();

        Rectangle playerBounds = new(
            x: (int)_playerPosition.X,
            y: (int)_playerPosition.Y,
            width: (int)_player.Width,
            height: (int)_player.Height
        );

        Rectangle croachBounds = new(
            x: (int)_croachPosition.X,
            y: (int)_croachPosition.Y,
            width: (int)_croach.Width,
            height: (int)_croach.Height
        );

        if (playerBounds.Left < _roomBounds.Left)
        {
            _playerPosition.X = _roomBounds.Left;
        }
        else if (playerBounds.Right > _roomBounds.Right)
        {
            _playerPosition.X = _roomBounds.Right - _player.Width;
        }

        if (playerBounds.Intersects(croachBounds))
        {
            _playerPosition = _resetPlayerPosition;
        }

        base.Update(gameTime);
    }

    private void CheckKeyboardInput()
    {
        // Stops the animation until an action occurs.
        _player.StopAnimation();

        // If the space key is held down, the movement speed increases by 1.5f
        float speed = MOVEMENT_SPEED;
        if (Input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        if (GameController.MoveLeft())
        {
            _player.PlayAnimation();
            _playerPosition.X -= speed;
        }

        if (GameController.MoveRight())
        {
            _player.PlayAnimation();
            _playerPosition.X += speed;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tilemap.Draw(SpriteBatch);

        _player.Draw(
            spriteBatch: SpriteBatch,
            position: _playerPosition
        );

        _croach.Draw(
            spriteBatch: SpriteBatch,
            position: _croachPosition
        );

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
