using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzleGame;

public class InputManager
{
    private static MouseState _currentMouseState;
    private static MouseState _lastMouseState;
    
    private static KeyboardState _currentKeyboardState;
    private static KeyboardState _lastKeyboardState;
    
    public static void Update()
    {
        _lastKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();
        
        _lastMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }

    public static bool IsKeyJustPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
    }

    public static bool IsMouse1JustPressed()
    {
        return _currentMouseState.LeftButton.Equals(ButtonState.Pressed) && _lastMouseState.LeftButton.Equals(ButtonState.Released);
    }

    public static bool IsMouse1Held()
    {
        return _currentMouseState.LeftButton.Equals(ButtonState.Pressed) && _lastMouseState.LeftButton.Equals(ButtonState.Pressed);
    }
    
    public static Point GetMousePosition()
    {
        return _currentMouseState.Position;
    }

    public static bool MouseHoveringRect(Rectangle rect)
    {
        return rect.Contains(_currentMouseState.Position);
    }

    public static bool MouseClickedRect(Rectangle rect)
    {
        return rect.Contains(_currentMouseState.Position) && IsMouse1JustPressed();
    }
}