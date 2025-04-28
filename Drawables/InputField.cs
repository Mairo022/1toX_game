using System.Windows.Forms;
using _1toX.shared;
using _1toX.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _1toX;

public class InputField : CustomRectangle
{
    readonly Vector2 _textPosition;
    readonly Color _textColor = new (120, 120, 120);
    readonly float _fontScale = .2f;
    readonly SpriteFont _font;
    
    readonly int _maxValue;
    int _curCharIndex;
    string _text;
    
    bool _isFocused;
    
    readonly bool _isBorderActive;
    readonly Border _border;
    readonly Caret _caret;
    
    public InputField(GraphicsDevice graphicsDevice, Point size, Point position, string text, SpriteFont font, int maxValue, bool addBorder = true)
        : base(graphicsDevice, size, position, Color.White)
    {
        _text = text;
        _font = font;
        _maxValue = maxValue;
        
        _isBorderActive = addBorder;

        if (_isBorderActive)
            _border = new Border(
                graphicsDevice,
                position, size,
                new Color(140, 140, 140),
                new Color(10, 10, 10));
        
        var textSize = font.MeasureString(text) * _fontScale;
        var avgCharSize = (int) (textSize.X / _text.Length);
        
        _textPosition = new Vector2(
            Position.X + 16, 
            Position.Y + (Size.Y - textSize.Y) + 3);
        
        _caret = new Caret(graphicsDevice, _textPosition, avgCharSize, _text.Length);
    }
    
    public void HandleWrite(KeyboardState keyboard, KeyboardState oldKeyboard, out string value)
    {
        value = _text;
        
        if (KeyboardUtils.TryConvertKeyboardInput(keyboard, oldKeyboard, out var key) && 
            KeyboardUtils.IsCharNumber(key))
        {
            var curValue = _text[.._curCharIndex] + key + _text[_curCharIndex..];
            var isAboveMaxValue = _text.Length > 0 && int.Parse(curValue) > _maxValue;
            if (isAboveMaxValue)
            {
                _text = _maxValue.ToString();
                value = _text;
                _caret.MoveCaret(_text.Length - _curCharIndex, _text.Length, out _curCharIndex);
                
                return;
            }

            var isMaxTextLength = _text.Length == 4;
            if (isMaxTextLength) return;
            
            _text = _text[.._curCharIndex] + key + _text[_curCharIndex..];
            value = _text;
            _caret.MoveCaret(1, _text.Length, out _curCharIndex);
            
            return;
        }

        if (_text.Length == 0)
        {
            value = "0";
            return;
        }

        if (KeyboardUtils.IsBackKey(keyboard, oldKeyboard) && _curCharIndex != 0)
        {
            _text = _text[..(_curCharIndex-1)] + _text[_curCharIndex..];
            _caret.MoveCaret(-1, _text.Length, out _curCharIndex);
        }
        
        value = _text;

        if (_text.Length == 0) value = "0";
    }
    
    public void HandleMouseClick(Point mousePos)
    {
        _isFocused = true;
        _caret.SetCaretFromMouseClick(mousePos, out _curCharIndex);
    }
    
    public void HandleMouseOffClick()
    {
        _isFocused = false;
        
    }

    public bool IsFocused()
    {
        return _isFocused;
    }

    public void HandleMouseHover()
    {
        if (Cursor.Current == Cursors.IBeam) return;
        Mouse.SetCursor(MouseCursor.IBeam);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawText(spriteBatch);
        if (_isBorderActive) _border.Draw(spriteBatch, _isFocused);
        if (_isFocused) _caret.Draw(spriteBatch);
    }
    
    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(
            _font, _text, _textPosition, _textColor, 
            0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.5f);
        spriteBatch.End();
    }
}