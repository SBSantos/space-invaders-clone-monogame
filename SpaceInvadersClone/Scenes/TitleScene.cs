using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Scenes;

namespace SpaceInvadersClone.Scenes;

public class TitleScene : Scene
{
    private const string MY_TEXT = "My";
    private const string SPACE_INVADERS_TEXT = "Space Invaders";
    private const string PRESS_ENTER_TEXT = "Press Enter to Start";
    private const string SCORE_TABLE_TEXT = "#Score table#";
    private const string WAVEY_TEXT = "Wavey       = 30 Points";
    private const string ROACH_TEXT = "Roach       = 20 Points";
    private const string BIG_CRIMSON_TEXT = "Big Crimson = 10 Points";
    private const float HALF = 0.5f;

    // The width of the screen.
    private float _screenWidth;

    // The height of the screen.
    private float _screenHeight;

    // The font to use to render normal text.
    private SpriteFont _font;

    // The font used to render the title text;
    private SpriteFont _font5x;

    // The position to draw the "My" text at.
    private Vector2 _myTextPos;

    // The origin to set for the "My" text.
    private Vector2 _myTextOrigin;

    // The position to draw the "Space Invaders" text at.
    private Vector2 _spaceInvadersTextPos;

    // The origin to set for the "Space Invaders" text.
    private Vector2 _spaceInvadersTextOrigin;

    // The position to draw the press enter text at.
    private Vector2 _pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

    // The position to draw the score table text at.
    private Vector2 _scoreTablePos;

    // The origin to set for the score table text.
    private Vector2 _scoreTableOrigin;

    // The position to draw the Wavey text at.
    private Vector2 _waveyTextPos;

    // The origin to set for the score table text
    private Vector2 _waveyTextOrigin;

    // The position to draw the Roach text at.
    private Vector2 _roachTextPos;

    // The origin to set for the Roach text
    private Vector2 _roachTextOrigin;

    // The position to draw the Big Crimson text at.
    private Vector2 _bigCrimsonTextPos;

    // The origin to set for the Big Crimson text
    private Vector2 _bigCrimsonTextOrigin;

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        Core.ExitOnEscape = true;

        // Set the width and height.
        _screenWidth = Core.GraphicsDevice.
                            PresentationParameters.
                            BackBufferWidth;

        _screenHeight = Core.GraphicsDevice.
                             PresentationParameters.
                             BackBufferHeight;

        float halfScreenWidth = _screenWidth * HALF;

        // Set the position and origin for the "Space Invaders" text.
        float spaceInvadersTextYPos = _screenHeight * 0.2f;

        Vector2 size = _font5x.MeasureString(SPACE_INVADERS_TEXT);
        _spaceInvadersTextPos = new(halfScreenWidth, spaceInvadersTextYPos);
        _spaceInvadersTextOrigin = size * HALF;

        // Set the position and origin for the "My" text.
        float myTextXPos = 98;
        float myTextYPos = spaceInvadersTextYPos - 25;

        size = _font.MeasureString(MY_TEXT);
        _myTextPos = new(myTextXPos, myTextYPos);
        _myTextOrigin = size * HALF;

        // Set the position and origin for the press enter text.
        float pressEnterTextYPos = _screenHeight * 0.8f;

        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _pressEnterPos = new(halfScreenWidth, pressEnterTextYPos);
        _pressEnterOrigin = size * HALF;

        // Set the position and origin for the score table text.
        float scoreTableTextYPos = _screenHeight * 0.35f;

        size = _font.MeasureString(SCORE_TABLE_TEXT);
        _scoreTablePos = new(halfScreenWidth, scoreTableTextYPos);
        _scoreTableOrigin = size * HALF;

        // Set the position and origin for the Wavey text.
        float waveyTextYPos = _screenHeight * 0.45f;

        size = _font.MeasureString(WAVEY_TEXT);
        _waveyTextPos = new(halfScreenWidth, waveyTextYPos);
        _waveyTextOrigin = size * HALF;

        // Set the position and origin for the Roach text.
        // Since this text will be in the middle of the y coordinate,
        // there is no need to create a local variable.
        float roachTextYPos = _screenHeight * 0.55f;

        size = _font.MeasureString(ROACH_TEXT);
        _roachTextPos = new(halfScreenWidth, roachTextYPos);
        _roachTextOrigin = size * HALF;

        // Set the position and origin for the Big Crimson text.
        float bigCrimsonTextYPos = _screenHeight * 0.65f;

        size = _font.MeasureString(BIG_CRIMSON_TEXT);
        _bigCrimsonTextPos = new(halfScreenWidth, bigCrimsonTextYPos);
        _bigCrimsonTextOrigin = size * HALF;
    }

    public override void LoadContent()
    {
        // Load the font for the standart text.
        _font = Core.Content.Load<SpriteFont>("fonts/PressStart2P-Regular");

        // Load the font for the title text.
        _font5x = Content.Load<SpriteFont>("fonts/PressStart2P-Regular_5x");
    }

    public override void Update(GameTime gameTime)
    {
        // If the user presses enter, switch to the game scene.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(76, 76, 69));

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // The color to user for the drop shadow text.
        Color dropShadowColor = Color.Black * HALF;
        Vector2 dropShadowtextOffset = new(5, 5);

        // Draw the "Space Invaders" text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(
            _font5x,
            SPACE_INVADERS_TEXT,
            _spaceInvadersTextPos + dropShadowtextOffset,
            dropShadowColor,
            0.0f,
            _spaceInvadersTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the "Space Invaders" text on top of that at its original position.
        Core.SpriteBatch.DrawString(
            _font5x,
            SPACE_INVADERS_TEXT,
            _spaceInvadersTextPos,
            Color.White,
            0.0f,
            _spaceInvadersTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the "My" text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        float myTextRotation = MathHelper.ToRadians(-15);

        Core.SpriteBatch.DrawString(
            _font,
            MY_TEXT,
            _myTextPos + dropShadowtextOffset,
            dropShadowColor,
            myTextRotation,
            _myTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the "My" text on top of that at its original position.
        Core.SpriteBatch.DrawString(
            _font,
            MY_TEXT,
            _myTextPos,
            Color.Yellow,
            myTextRotation,
            _myTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the press enter text slightly from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(
            _font,
            PRESS_ENTER_TEXT,
            _pressEnterPos + dropShadowtextOffset,
            dropShadowColor,
            0.0f,
            _pressEnterOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the press enter text.
        Core.SpriteBatch.DrawString(
            _font,
            PRESS_ENTER_TEXT,
            _pressEnterPos,
            Color.White,
            0.0f,
            _pressEnterOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the score table text slightly from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(
            _font,
            SCORE_TABLE_TEXT,
            _scoreTablePos + dropShadowtextOffset,
            dropShadowColor,
            0.0f,
            _scoreTableOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the score table text.
        Core.SpriteBatch.DrawString(
            _font,
            SCORE_TABLE_TEXT,
            _scoreTablePos,
            Color.White,
            0.0f,
            _scoreTableOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the Wavey text slightly from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(
            _font,
            WAVEY_TEXT,
            _waveyTextPos + dropShadowtextOffset,
            dropShadowColor,
            0.0f,
            _waveyTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the Wavey text.
        Core.SpriteBatch.DrawString(
            _font,
            WAVEY_TEXT,
            _waveyTextPos,
            new Color(29, 147, 211),
            0.0f,
            _waveyTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the Roach text slightly from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(
            _font,
            ROACH_TEXT,
            _roachTextPos + dropShadowtextOffset,
            dropShadowColor,
            0.0f,
            _roachTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the Roach text.
        Core.SpriteBatch.DrawString(
            _font,
            ROACH_TEXT,
            _roachTextPos,
            new Color(239, 113, 23),
            0.0f,
            _roachTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the Big Crimson text slightly from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(
            _font,
            BIG_CRIMSON_TEXT,
            _bigCrimsonTextPos + dropShadowtextOffset,
            dropShadowColor,
            0.0f,
            _bigCrimsonTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        // Draw the Big Crimson text.
        Core.SpriteBatch.DrawString(
            _font,
            BIG_CRIMSON_TEXT,
            _bigCrimsonTextPos,
            new Color(237, 37, 64),
            0.0f,
            _bigCrimsonTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.End();
    }
}