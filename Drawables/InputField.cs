﻿using System.Windows.Forms;
using _1toX.shared;
using _1toX.utils;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Label = _1toX.shared.Label;

namespace _1toX;

public class InputField : CustomRectangle
{
    readonly Vector2 _textPosition;
    readonly Color _textColor = new (120, 120, 120);
    readonly SpriteFontBase _font;
    
    readonly int _maxValue;
    int _curCharIndex;
    string _text;
    
    bool _isFocused;
    
    readonly bool _isBorderActive;
    readonly Border _border;
    
    readonly Caret _caret;
    readonly Label _label;
    
    public InputField(GraphicsDevice graphicsDevice, Point size, Point position, string text, FontSystem fontSystem,
        int maxValue, string labelText, bool addBorder = true) : base(graphicsDevice, size, position, Color.White)
    {
        _text = text;
        _font = fontSystem.GetFont(Constants.FontSizeHeader);
        _maxValue = maxValue;
        
        _isBorderActive = addBorder;

        if (_isBorderActive)
            _border = new Border(
                graphicsDevice,
                position, size,
                new Color(135, 135, 135),
                new Color(75, 75, 75));
        
        var textSize = _font.MeasureString(text);
        var avgCharSize = (int) (textSize.X / _text.Length);
        
        _textPosition = new Vector2(
            Position.X + 16, 
            Position.Y + (Size.Y - textSize.Y)/2 - 1);
        
        _caret = new Caret(graphicsDevice, _textPosition, avgCharSize, _text.Length);
        _label = new Label(fontSystem, labelText, position, size);
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
        _label.Draw(spriteBatch);
        if (_isBorderActive) _border.Draw(spriteBatch, _isFocused);
        if (_isFocused) _caret.Draw(spriteBatch);
    }
    
    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _text, _textPosition, _textColor);
        spriteBatch.End();
    }
}