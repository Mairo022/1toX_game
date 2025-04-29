using _1toX.utils;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX.shared;

public class Label
{
    readonly string _text;
    readonly SpriteFontBase _font;
    readonly Color _color = new(115, 115, 115);
    readonly Vector2 _position;
    
    public Label(FontSystem fontSystem, string text, Point basePosition, Point baseSize)
    {
        _font = fontSystem.GetFont(Constants.FontSizeHeader);
        _text = text;
        
        var fontSize = _font.MeasureString(_text);
        
        _position.X = basePosition.X + (baseSize.X - fontSize.X) / 2;
        _position.Y = basePosition.Y - fontSize.Y - Constants.InputLabelGapY;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _text, _position, _color);
        spriteBatch.End();
    }
}