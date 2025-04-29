using _1toX.shared;
using _1toX.utils;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Button : CustomRectangle
{
    string _text;
    readonly Border _border;

    Vector2 _textSize;
    Vector2 _textPosition;
    readonly Color _textColor = new (66, 66, 66);
    readonly SpriteFontBase _font;
    
    public Button(GraphicsDevice graphicsDevice, Point size, Point position, string text, FontSystem fontSystem) 
        : base(graphicsDevice, size, position, new Color(230, 230, 230), new Color(217, 217, 217))
    {
        _text = text;
        _font = fontSystem.GetFont(Constants.FontSizeHeader);
        _border = new Border(graphicsDevice, position, size, new Color(135, 135, 135), Color.Black);
        
        _textSize = _font.MeasureString(text);
        
        _textPosition = new Vector2(
            Position.X + (Size.X - _textSize.X)/2, 
            Position.Y + (Size.Y - _textSize.Y)/2 - 1);
    }

    public void ChangeText(string text)
    {
        _text = text;
        
        _textSize = _font.MeasureString(text);
        
        _textPosition = new Vector2(
            Position.X + (Size.X - _textSize.X)/2, 
            Position.Y + (Size.Y - _textSize.Y)/2-1);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawText(spriteBatch);
        _border.Draw(spriteBatch, false);
    }

    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _text, _textPosition, _textColor);
        spriteBatch.End();
    }
}