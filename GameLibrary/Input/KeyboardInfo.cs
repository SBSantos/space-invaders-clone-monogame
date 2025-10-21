using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input;

public class KeyboardInfo
{
    /// <summary>
    /// Gets the state of keyboard input during the previous update cycle.
    /// </summary>
    public KeyboardState PreviousState { get; private set; }

    /// <summary>
    /// Gets the state of keyboard input during the current update cycle.
    /// </summary>
    public KeyboardState CurrentState { get; private set; }

    /// <summary>
    /// Creates a new KeyboardInfo.
    /// </summary>
    public KeyboardInfo()
    {
        PreviousState = new();
        CurrentState = Keyboard.GetState();
    }

    /// <summary>
    /// Updates the state information about keyboard input.
    /// </summary>
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    /// <summary>
    /// Returns a value that indicates if the specified value is currently down.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the specified key is currently down; Otherwise, false.</returns>
    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    /// <summary>
    /// Returns a value that indicates if the specified value is currently up.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the specified key is currently up; Otherwise, false.</returns>
    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    /// <summary>
    /// Returns a value that indicates if the specified value whas just pressed on the current frame.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the specified key was just pressed on the current frame; Otherwise, false.</returns>
    public bool WasKeyJustPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key)
            && PreviousState.IsKeyUp(key);
    }

    /// <summary>
    /// Returns a value that indicates if the specified value whas just released on the current frame.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the specified key was just released on the current frame; Otherwise, false.</returns>
    public bool WasKeyJustReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key)
            && PreviousState.IsKeyDown(key);
    }
}
