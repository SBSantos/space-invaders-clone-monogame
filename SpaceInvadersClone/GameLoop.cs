using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;
using GameLibrary.Graphics;
using SpaceInvadersClone.GameObjects;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    private Player _player;
    private Roach _roach;

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

        int croachColumn = (int)(_tilemap.Columns / DESIRED_ROW);

        Vector2 croachPosition = new(
            croachColumn * _tilemap.TileWidth,
            row * _tilemap.TileHeight
        );
        _roach.Initialize(croachPosition);
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

        _player = new(playerAnimation);

        AnimatedSprite roachAnimation = atlas.CreateAnimatedSprite(animationName: "roach-animation");
        roachAnimation.Scale = new Vector2(x: SCALE, y: SCALE);

        _roach = new(roachAnimation);

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
        _roach.Update(gameTime);

        CheckCollision();

        base.Update(gameTime);
    }

    private void CheckCollision()
    {
        Rectangle playerBounds = _player.GetBounds();
        Rectangle roachBounds = _roach.GetBounds();

        if (playerBounds.Left < _roomBounds.Left)
        {
            _player.Position.X = _roomBounds.Left;
        }
        else if (playerBounds.Right > _roomBounds.Right)
        {
            _player.Position.X = _roomBounds.Right - playerBounds.Width;
        }

        if (playerBounds.Intersects(roachBounds))
        {
            _player.Initialize(_player.ResetPlayerPosition);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tilemap.Draw(SpriteBatch);

        _player.Draw();

        _roach.Draw();

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
