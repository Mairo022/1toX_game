using System;
using _1toX.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX.shared;

public class Caret
{
    int _curCharIndex;
    readonly int _avgCharSize;
    readonly Vector2 _textPosition;
    int _textLength;
    
    readonly Color _color = new (30, 30, 30);
    readonly Texture2D _texture;
    Rectangle _caret;
    int _caretPosition;
    
    int _cyclesPassed;
    readonly int _cyclesStateLength = (int) (500 / 16.6);
    
    public Caret(GraphicsDevice graphicsDevice, Vector2 textPosition, int avgCharSize, int textLength)
    {
        _textPosition = textPosition;
        _avgCharSize = avgCharSize;
        _textLength = textLength;
        
        _texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _texture.SetData([Color.White]);
        _caretPosition = (int) _textPosition.X;
        
        CreateCaretRectangle((int) _textPosition.X);
    }
    
    void CreateCaretRectangle(int xPos)
    {
        _caret = new Rectangle(xPos, (int)_textPosition.Y-2, 1, Constants.FontSizeHeader + 2);
    }
    
    public void MoveCaret(int x, int textLength, out int curCharIndex)
    {
        _caretPosition += _avgCharSize * x;
        _curCharIndex += x;
        _textLength = textLength;

        curCharIndex = _curCharIndex;
        
        CreateCaretRectangle(_caretPosition);
    }
    
    public void SetCaretFromMouseClick(Point mousePos, out int charIndex)
    {
        _caretPosition = (int) _textPosition.X;
        _curCharIndex = 0;
        
        var minGap = Math.Abs(_textPosition.X - mousePos.X);
        
        for (var i = 0; i <= _textLength; i++)
        {
            var curPos = _textPosition.X + _avgCharSize * i;
            var gap = Math.Abs(curPos - mousePos.X);

            if (gap >= minGap) continue;
            
            minGap = gap;
            _caretPosition = (int) curPos;
            _curCharIndex = i;
        }
        
        charIndex = _curCharIndex;
        CreateCaretRectangle(_caretPosition);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_cyclesPassed > 0)
        {
            if (_cyclesPassed <= _cyclesStateLength)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_texture, _caret, _color);
                spriteBatch.End();
            }
            else _cyclesPassed = -_cyclesStateLength + 1;
        }

        _cyclesPassed++;
    }
}