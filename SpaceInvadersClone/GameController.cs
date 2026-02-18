using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using GameLibrary.Input;

namespace SpaceInvadersClone;

public static class GameController
{
    private static KeyboardInfo Keyboard => Core.Input.Keyboard;
    private static GamePadInfo GamePad => Core.Input.GamePads[(int)PlayerIndex.One];
    private static MouseInfo Mouse => Core.Input.Mouse;

    /// <summary>
    /// Returns true if the player has triggered the "move left" action.
    /// </summary>
    public static bool MoveLeft()
    {
        return Keyboard.IsKeyDown(Keys.Left) ||
               Keyboard.IsKeyDown(Keys.A) ||
               GamePad.IsButtonDown(Buttons.DPadLeft) ||
               GamePad.IsButtonDown(Buttons.LeftThumbstickLeft);
    }

    /// <summary>
    /// Returns true if the player has triggered the "move right" action.
    /// </summary>
    public static bool MoveRight()
    {
        return Keyboard.IsKeyDown(Keys.Right) ||
               Keyboard.IsKeyDown(Keys.D) ||
               GamePad.IsButtonDown(Buttons.DPadRight) ||
               GamePad.IsButtonDown(Buttons.LeftThumbstickRight);
    }

    /// <summary>
    /// Returns true if the player has triggered the "pause" action.
    /// </summary>
    public static bool Pause()
    {
        return Keyboard.IsKeyDown(Keys.Escape) ||
               GamePad.IsButtonDown(Buttons.Start);
    }

    /// <summary>
    /// Returns true if the player has triggered the "action" action.
    /// </summary>
    public static bool Action()
    {
        return Keyboard.IsKeyDown(Keys.Enter) ||
               GamePad.IsButtonDown(Buttons.A);
    }

    /// <summary>
    /// Returns true if the player has triggered the "shoot" action.
    /// </summary>
    public static bool Shoot()
    {
        return Keyboard.WasKeyJustPressed(Keys.Space) ||
               GamePad.WasButtonJustPressed(Buttons.X) ||
               Mouse.WasButtonJustPressed(MouseInfo.MouseButton.Left);
    }

    /// <summary>
    /// Returns mouse xy-position to control the player.
    /// </summary>
    /// <returns>The mouse x and y position.</returns>
    public static Vector2 MousePosition()
    {
        return Mouse.Position.ToVector2();
    }

    /// <summary>
    /// Returns true if the mouse was moved
    /// </summary>
    /// <returns>
    /// True if it was moved. Otherwise, false.
    /// </returns>
    public static bool WasMouseMoved()
    {
        return Mouse.WasMoved;
    }
}