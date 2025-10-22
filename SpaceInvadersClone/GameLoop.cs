using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Graphics;
using GameLibrary.Input;
using System.Globalization;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    private AnimatedSprite _player;

    private Vector2 _playerPosition;

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
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        TextureAtlas atlas = TextureAtlas.FromFile(
            content: Content,
            filename: "images/atlas-definition.xml"
        );
        const float SCALE = 10.0f;

        _player = atlas.CreateAnimatedSprite(animationName: "player-animation");
        _player.Scale = new Vector2(x: SCALE, y: SCALE);
        _player.CenterOrigin();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        _player.Update(gameTime);

        CheckKeyboardInput();

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

        // If the A or Left keys are down, play the animation and move the player left on the screen.
        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _player.PlayAnimation();
            _playerPosition.X -= speed;
        }

        // If the D or Right keys are down, play the animation and move the player right on the screen.
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _player.PlayAnimation();
            _playerPosition.X += speed;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        const float HALF = 0.5f;

        // Middle of the screen on the x-axis
        float MiddleX = GraphicsDevice.PresentationParameters.BackBufferWidth * HALF;

        // Middle of the screen on the y-axis
        float MiddleY = GraphicsDevice.PresentationParameters.BackBufferHeight * HALF;

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _player.Draw(
            spriteBatch: SpriteBatch,
            position: new Vector2(
                _playerPosition.X + MiddleX,
                _playerPosition.Y + MiddleY
            )
        );
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
