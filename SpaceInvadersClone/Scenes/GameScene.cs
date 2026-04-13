using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;
using GameLibrary.Graphics;
using GameLibrary.Scenes;
using SpaceInvadersClone.Entities;
using SpaceInvadersClone.Systems;
using SpaceInvadersClone.Managers;
using System.Collections.Generic;
using SpaceInvadersClone.UI;
using System;

namespace SpaceInvadersClone.Scenes;

public class GameScene : Scene
{
    private enum GameState
    {
        Winner,
        Playing,
        Paused,
        GameOver
    }

    private Player _player;

    private Vector2 _playerPosition;

    private List<Bullet> _bullets;

    private EnemyManager _enemy;

    private List<Laser> _lasers;

    private Tilemap _tilemap;

    private Rectangle _roomBounds;

    private float _playerDeathtimer;

    private float _endGameTimer;

    private GameState _gameState;

    private GameSceneUI _ui;

    private Effect _grayscaleEffect;

    private float _saturation = 1.0f;

    private const float FADE_SPEED = 0.1f;

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

        _playerPosition = new(
            playerColumn * _tilemap.TileWidth,
            row * _tilemap.TileHeight
        );

        _ui.Initialize(_roomBounds, _tilemap);

        NewGame();
    }

    private void NewGame()
    {
        _player.IsActive = true;
        _player.Lives = 3;
        _player.Score = 0;

        _player.Initialize(_playerPosition);
        _enemy.Initialize();

        _gameState = GameState.Playing;
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
        _bullets = [];

        _enemy = new(_tilemap);
        _lasers = [];
        _enemy.LoadContent(atlas, SCALE);

        _ui = new(_player);

        _grayscaleEffect = Content.Load<Effect>("effects/grayscaleEffect");
    }


    public override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        if (_gameState == GameState.Winner)
        {
            _endGameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_endGameTimer >= 5)
            {
                _ui.HideVictoryScreen();
                NewGame();
                _endGameTimer = 0;
            }
            return;
        }

        if (_gameState != GameState.Playing && _gameState != GameState.Winner)
        {
            // This is either Pause or Game Over state
            _saturation = Math.Max(0.0f, _saturation - FADE_SPEED);

            if (_gameState == GameState.GameOver)
            {
                _endGameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_endGameTimer >= 5)
                {
                    _ui.HideGameOverScreen();
                    Core.ChangeScene(new TitleScene());
                    _endGameTimer = 0;
                }
                return;
            }
        }

        // Pause
        if (GameController.Pause()) { TogglePause(); }

        if (_gameState == GameState.Paused)
        {
            if (GameController.Quit())
            {
                Core.ChangeScene(new TitleScene());
            }
            return;
        }

        // Game Win/Over
        CheckWinGame();
        CheckGameOver();

        // Player
        if (!_player.IsActive)
        {
            if (_player.Lives > 0)
            {
                _playerDeathtimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_playerDeathtimer >= 3)
                {
                    _player.IsActive = true;
                    _playerDeathtimer = 0;
                }
            }
            return;
        }

        _player.Update(gameTime, _bullets);

        for (int i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].Update();
        }

        CollisionSystem.CheckPlayerHitMapEdge(_player, _roomBounds);
        CollisionSystem.CheckPlayerVsEnemyCollision(
            _player,
            _bullets,
            _enemy.EnemyFormation.Enemies,
            _roomBounds
        );

        // Enemy
        _enemy.Update(gameTime, _lasers, _roomBounds);

        for (int i = 0; i < _lasers.Count; i++)
        {
            _lasers[i].Update();
        }

        CollisionSystem.CheckEnemyVsPlayerCollision(
            _enemy.EnemyFormation.Enemies, _player
        );

        CollisionSystem.CheckLaserVsPlayerCollision(
            _lasers,
            _player,
            _roomBounds
        );

        CollisionSystem.CheckLaserVsBulletCollision(_lasers, _bullets);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        if (_gameState != GameState.Playing && _gameState != GameState.Winner)
        {
            // If its a Game Over or Pause state, apply the saturation parameter.
            _grayscaleEffect.Parameters["Saturation"].SetValue(_saturation);

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _grayscaleEffect);
        }
        else
        {
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

        _tilemap.Draw(Core.SpriteBatch);

        _player.Draw();

        for (int i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].Draw();
        }

        _enemy.Draw();

        for (int i = 0; i < _lasers.Count; i++)
        {
            _lasers[i].Draw();
        }

        Core.SpriteBatch.End();

        _ui.Draw();

        base.Draw(gameTime);
    }

    private void CheckWinGame()
    {
        if (_enemy.AllEnemiesDefeated())
        {
            _ui.ShowVictoryScreen();
            _gameState = GameState.Winner;
        }
    }

    private void CheckGameOver()
    {
        if (_player.NoLivesLeft())
        {
            _ui.ShowGameOverScreen();
            _gameState = GameState.GameOver;
            _saturation = 1.0f;
        }
    }

    private void TogglePause()
    {
        if (_gameState == GameState.Paused)
        {
            _ui.HidePauseScreen();
            _gameState = GameState.Playing;
        }
        else
        {
            _ui.ShowPauseScreen();
            _gameState = GameState.Paused;
            _saturation = 1.0f;
        }
    }
}
