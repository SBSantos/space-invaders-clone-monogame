using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Graphics;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    private AnimatedSprite _player;

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
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        TextureAtlas atlas = TextureAtlas.FromFile(
            content: Content,
            filename: "images/atlas-definition.xml"
        );
        const float SCALE = 4.0f;

        _player = atlas.CreateAnimatedSprite(animationName: "player-animation");
        _player.Scale = new Vector2(x: SCALE, y: SCALE);
        _player.CenterOrigin();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        const float HALF = 0.5f;

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _player.Draw(
            spriteBatch: SpriteBatch,
            position: new Vector2(
                GraphicsDevice.PresentationParameters.BackBufferWidth * HALF,
                GraphicsDevice.PresentationParameters.BackBufferHeight * HALF
            )
        );
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
