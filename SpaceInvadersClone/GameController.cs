using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Input;

namespace SpaceInvadersClone;

public static class GameController
{
    private static KeyboardInfo s_keyboard => Core.Input.Keyboard;
    private static GamePadInfo s_gamePad => Core.Input.GamePads[(int)PlayerIndex.One];

    /// <summary>
    /// Returns true if the player has triggered the "move left" action.
    /// </summary>
    public static bool MoveLeft()
    {
        return s_keyboard.IsKeyDown(Keys.Left) ||
               s_keyboard.IsKeyDown(Keys.A) ||
               s_gamePad.IsButtonDown(Buttons.DPadLeft) ||
               s_gamePad.IsButtonDown(Buttons.LeftThumbstickLeft);
    }

    /// <summary>
    /// Returns true if the player has triggered the "move right" action.
    /// </summary>
    public static bool MoveRight()
    {
        return s_keyboard.IsKeyDown(Keys.Right) ||
               s_keyboard.IsKeyDown(Keys.D) ||
               s_gamePad.IsButtonDown(Buttons.DPadRight) ||
               s_gamePad.IsButtonDown(Buttons.LeftThumbstickRight);
    }

    /// <summary>
    /// Returns true if the player has triggered the "pause" action.
    /// </summary>
    public static bool Pause()
    {
        return s_keyboard.IsKeyDown(Keys.Escape) ||
               s_gamePad.IsButtonDown(Buttons.Start);
    }

    /// <summary>
    /// Returns true if the player has triggered the "action" action.
    /// </summary>
    public static bool Action()
    {
        return s_keyboard.IsKeyDown(Keys.Enter) ||
               s_gamePad.IsButtonDown(Buttons.A);
    }

    /// <summary>
    /// Returns true if the player has triggered the "shoot" action.
    /// </summary>
    public static bool Shoot()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Space) ||
               s_gamePad.WasButtonJustPressed(Buttons.X);
    }
}

