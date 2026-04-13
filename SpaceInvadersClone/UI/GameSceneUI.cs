using System.ComponentModel.Design.Serialization;
using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvadersClone.Entities;

namespace SpaceInvadersClone.UI;

public class GameSceneUI
{
    private readonly string scoreFormat = "SCORE: {0:D6}";

    private readonly string livesFormat = "Lives: {0}";

    private readonly SpriteFont _font;

    private readonly Player _player;

    private Color _dropShadowColor;

    private Vector2 _dropShadowOffset;

    private bool _isPauseVisible;

    private bool _isGameOverVisible;

    private bool _isVictoryVisible;

    private Vector2 _scoreTextPosition;

    private Vector2 _scoreTextOrigin;

    private Vector2 _livesTextPosition;

    private Vector2 _livesTextOrigin;

    private string _pauseText;

    private Vector2 _pauseTextPosition;

    private Vector2 _pauseTextOrigin;

    private string _victoryText;

    private Vector2 _victoryTextPosition;

    private Vector2 _victoryTextOrigin;

    private string _gameOverText;

    private Vector2 _gameOverTextPosition;

    private Vector2 _gameOverTextOrigin;

    private string _resumeText;

    private Vector2 _resumeTextPosition;

    private Vector2 _resumeTextOrigin;

    private string _quitText;

    private Vector2 _quitTextPosition;

    private Vector2 _quitTextOrigin;

    public GameSceneUI(Player player)
    {
        _font = Core.Content.Load<SpriteFont>("fonts/PressStart2P-Regular");

        _player = player;
        _isPauseVisible = false;
        _isGameOverVisible = false;
        _isVictoryVisible = false;
    }

    public void Initialize(Rectangle roomBounds, Tilemap tilemap)
    {
        _scoreTextPosition = new(
            roomBounds.Left,
            tilemap.TileHeight / 2
        );

        float scoreTextYOrigin = _font.MeasureString(scoreFormat).Y / 2;
        _scoreTextOrigin = new(0, scoreTextYOrigin);

        _livesTextPosition = new(
            roomBounds.Center.X * 1.5f,
            tilemap.TileHeight / 2
        );

        _dropShadowColor = Color.Black;
        _dropShadowOffset = new(2, 2);

        float livesTextYOrigin = _font.MeasureString(livesFormat).Y / 2;
        _livesTextOrigin = new(0, livesTextYOrigin);

        _pauseText = "GAME PAUSED";
        _pauseTextPosition = new(roomBounds.Center.X, roomBounds.Center.Y / 1.2f);
        _pauseTextOrigin = _font.MeasureString(_pauseText) / 2;

        _victoryText = "Victory!";
        _victoryTextPosition = new(roomBounds.Center.X, roomBounds.Center.Y);
        _victoryTextOrigin = _font.MeasureString(_victoryText) / 2;

        _gameOverText = "Game Over.";
        _gameOverTextPosition = new(roomBounds.Center.X, roomBounds.Center.Y);
        _gameOverTextOrigin = _font.MeasureString(_gameOverText) / 2;

        _resumeText = "Press ESC to resume.";
        _resumeTextPosition = new(roomBounds.Center.X, roomBounds.Center.Y);
        _resumeTextOrigin = _font.MeasureString(_resumeText) / 2;

        _quitText = "Press Q to quit game.";
        _quitTextPosition = new(roomBounds.Center.X, roomBounds.Center.Y / 0.9f);
        _quitTextOrigin = _font.MeasureString(_quitText) / 2;
    }

    public void Draw()
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Core.SpriteBatch.DrawString(
            _font,
            string.Format(scoreFormat, _player.Score),
            _scoreTextPosition,
            Color.White,
            0.0f,
            _scoreTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.DrawString(
            _font,
            string.Format(livesFormat, _player.Lives),
            _livesTextPosition,
            Color.White,
            0.0f,
            _livesTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        if (_isPauseVisible)
        {
            DrawPauseScreen();
            DrawQuitOption();
            DrawResumeOption();
        }
        else if (_isVictoryVisible)
        {
            DrawVictoryScreen();
        }
        else if (_isGameOverVisible)
        {
            DrawGameOverScreen();
        }

        Core.SpriteBatch.End();
    }

    private void DrawPauseScreen()
    {
        Core.SpriteBatch.DrawString(
            _font,
            _pauseText,
            _pauseTextPosition + _dropShadowOffset,
            _dropShadowColor,
            0.0f,
            _pauseTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.DrawString(
            _font,
            _pauseText,
            _pauseTextPosition,
            Color.White,
            0.0f,
            _pauseTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
    }

    private void DrawResumeOption()
    {
        Core.SpriteBatch.DrawString(
            _font,
            _resumeText,
            _resumeTextPosition + _dropShadowOffset,
            _dropShadowColor,
            0.0f,
            _resumeTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.DrawString(
            _font,
            _resumeText,
            _resumeTextPosition,
            Color.White,
            0.0f,
            _resumeTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
    }

    private void DrawQuitOption()
    {
        Core.SpriteBatch.DrawString(
            _font,
            _quitText,
            _quitTextPosition + _dropShadowOffset,
            _dropShadowColor,
            0.0f,
            _quitTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.DrawString(
            _font,
            _quitText,
            _quitTextPosition,
            Color.White,
            0.0f,
            _quitTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
    }

    private void DrawVictoryScreen()
    {
        Core.SpriteBatch.DrawString(
            _font,
            _victoryText,
            _victoryTextPosition + _dropShadowOffset,
            _dropShadowColor,
            0.0f,
            _victoryTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.DrawString(
            _font,
            _victoryText,
            _victoryTextPosition,
            new Color(39, 213, 58),
            0.0f,
            _victoryTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
    }

    private void DrawGameOverScreen()
    {
        Core.SpriteBatch.DrawString(
            _font,
            _gameOverText,
            _gameOverTextPosition + _dropShadowOffset,
            _dropShadowColor,
            0.0f,
            _gameOverTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );

        Core.SpriteBatch.DrawString(
            _font,
            _gameOverText,
            _gameOverTextPosition,
            Color.Red,
            0.0f,
            _gameOverTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
    }

    public void ShowPauseScreen() => _isPauseVisible = true;

    public void HidePauseScreen() => _isPauseVisible = false;

    public void ShowGameOverScreen() => _isGameOverVisible = true;

    public void HideGameOverScreen() => _isGameOverVisible = false;

    public void ShowVictoryScreen() => _isVictoryVisible = true;

    public void HideVictoryScreen() => _isVictoryVisible = false;
}
