using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _1toX.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _1toX;

public class InputField : CustomRectangle
{
    readonly GraphicsDevice _graphicsDevice;

    string _text;
    readonly int _maxValue;

    int _curCharIndex;
    readonly int _avgCharSize;
    readonly Vector2 _textPosition;
    readonly Color _textColor = new (120, 120, 120);
    readonly float _fontScale = .2f;
    readonly SpriteFont _font;
    
    bool _isFocused;
    readonly Color _borderColorFocus = new(10, 10, 10);
    readonly Color _borderColorUnfocus = new(100, 100, 100);

    bool _isBorderActive;
    Color _borderColor;
    Texture2D _borderTexture;
    readonly List<Rectangle> _borders = new(4);

    Color _caretColor = Color.Black;
    Texture2D _caretTexture;
    Rectangle _caretRectangle;
    int _caretPosition;
    int _cyclesCaretPassed;
    readonly int _cyclesCaretStateLength = (int) (500 / 16.6);
    
    public InputField(GraphicsDevice graphicsDevice, Point size, Point position, string text, SpriteFont font, int maxValue, bool addBorder = true)
        : base(graphicsDevice, size, position, Color.White)
    {
        _graphicsDevice = graphicsDevice;
        _text = text;
        _font = font;
        _maxValue = maxValue;

        _borderColor = _borderColorUnfocus;
        _isBorderActive = addBorder;
        if (_isBorderActive) CreateBorder();
        
        var textSize = font.MeasureString(text) * _fontScale;
        _avgCharSize = (int) (textSize.X / _text.Length);
        
        _textPosition = new Vector2(
            Position.X + 16, 
            Position.Y + (Size.Y - textSize.Y) + 3);
        
        CreateCaretRectangle((int) _textPosition.X);
        _caretTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _caretTexture.SetData([Color.White]);
        _caretPosition = (int) _textPosition.X;
    }

    void CreateCaretRectangle(int xPos)
    {
        _caretRectangle = new Rectangle(xPos, (int)_textPosition.Y, 2, 32);
    }

    void CreateBorder()
    {
        // Top, bottom
        _borders.Add(new Rectangle(Position.X, Position.Y, Size.X, 1));
        _borders.Add(new Rectangle(Position.X, Position.Y + Size.Y, Size.X, 1));
        // Left, right
        _borders.Add(new Rectangle(Position.X, Position.Y, 1, Size.Y));
        _borders.Add(new Rectangle(Position.X + Size.X, Position.Y, 1, Size.Y + 1));
        
        _borderTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _borderTexture.SetData([Color.White]);
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
                MoveCaret(_text.Length - _curCharIndex);
                
                return;
            }

            var isMaxTextLength = _text.Length == 4;
            if (isMaxTextLength) return;
            
            _text = _text[.._curCharIndex] + key + _text[_curCharIndex..];
            value = _text;
            MoveCaret(1);
            
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
            MoveCaret(-1);
        }
        
        value = _text;

        if (_text.Length == 0) value = "0";
    }
    
    public void HandleMouseClick(Point mousePos)
    {
        _isFocused = true;
        _borderColor = _borderColorFocus;
        SetCaretFromMouseClick(mousePos);
    }

    void SetCaretFromMouseClick(Point mousePos)
    {
        _caretPosition = (int) _textPosition.X;
        _curCharIndex = 0;
        
        var minGap = Math.Abs(_textPosition.X - mousePos.X);
        
        for (var i = 0; i <= _text.Length; i++)
        {
            var curPos = _textPosition.X + _avgCharSize * i;
            var gap = Math.Abs(curPos - mousePos.X);

            if (gap >= minGap) continue;
            
            minGap = gap;
            _caretPosition = (int) curPos;
            _curCharIndex = i;
        }
        
        CreateCaretRectangle(_caretPosition);
    }
    
    void MoveCaret(int x)
    {
        _caretPosition += _avgCharSize * x;
        _curCharIndex += x;
        
        CreateCaretRectangle(_caretPosition);
    }

    public void HandleMouseOffClick()
    {
        _isFocused = false;
        _borderColor = _borderColorUnfocus;
        
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

    public void AddBorder()
    {
        if (_isBorderActive) return;
        _isBorderActive = true;
        CreateBorder();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawText(spriteBatch);
        if (_isBorderActive) DrawBorder(spriteBatch);
        if (_isFocused) DrawCaret(spriteBatch);
    }

    void DrawBorder(SpriteBatch spriteBatch)
    {
        foreach (var border in _borders)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_borderTexture, border, _borderColor);
            spriteBatch.End();           
        }
    }
    
    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(
            _font, _text, _textPosition, _textColor, 
            0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.5f);
        spriteBatch.End();
    }
    
    void DrawCaret(SpriteBatch spriteBatch)
    {
        if (_cyclesCaretPassed > 0)
        {
            if (_cyclesCaretPassed <= _cyclesCaretStateLength)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_caretTexture, _caretRectangle, _caretColor);
                spriteBatch.End();
            }
            else _cyclesCaretPassed = -_cyclesCaretStateLength + 1;
        }

        _cyclesCaretPassed++;
    }
}