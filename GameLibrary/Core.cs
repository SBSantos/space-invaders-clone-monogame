using System;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary;

public class Core : Game
{
    internal static Core s_instance;

    /// <summary>
    /// Gets a reference to the core instance.
    /// </summary>
    public static Core Intance => s_instance;

    // Controls the presentation of graphics.
    public static GraphicsDeviceManager Graphics { get; private set; }

    // Create resources and perform primitive rendering
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    // SpriteBatch for 2D rendering
    public static SpriteBatch SpriteBatch { get; private set; }

    // Load global assets
    public static new ContentManager Content { get; private set; }

    // Input manager system
    public static InputManager Input { get; private set; }

    // Exit the game on ESC key
    public static bool ExitOnEscape { get; set; }

    public Core(string title, int width, int height, bool fullscreen)
    {
        if (s_instance != null)
        {
            throw new InvalidOperationException("Only a single Core instance can be created.");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        Graphics = new(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = fullscreen
        };
        Graphics.ApplyChanges();

        Window.Title = title;

        Content = base.Content;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;

        ExitOnEscape = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Set the core's graphics device to a reference of the game's graphics device
        GraphicsDevice = base.GraphicsDevice;

        // Create a SpriteBatch instance
        SpriteBatch = new(GraphicsDevice);

        // Create a new InputManager
        Input = new();
    }

    protected override void Update(GameTime gameTime)
    {
        // Update the input manager.
        Input.Update(gameTime);

        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }
}
