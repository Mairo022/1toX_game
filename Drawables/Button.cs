using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Button : CustomRectangle
{
    string _text;

    Vector2 _textSize;
    Vector2 _textPosition;
    readonly SpriteFontBase _font;
    
    public Button(GraphicsDevice graphicsDevice, Point size, Point position, string text, FontSystem fontSystem) 
        : base(graphicsDevice, size, position, Color.DarkGray)
    {
        _text = text;
        _font = fontSystem.GetFont(24);
        
        _textSize = _font.MeasureString(text);
        
        _textPosition = new Vector2(
            Position.X + (Size.X - _textSize.X)/2, 
            Position.Y + (Size.Y - _textSize.Y)/2);
    }

    public void ChangeText(string text)
    {
        _text = text;
        
        _textSize = _font.MeasureString(text);
        
        _textPosition = new Vector2(
            Position.X + (Size.X - _textSize.X)/2, 
            Position.Y + (Size.Y - _textSize.Y)/2);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawText(spriteBatch);
    }

    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _text, _textPosition, Color.Black);
        spriteBatch.End();
    }
}