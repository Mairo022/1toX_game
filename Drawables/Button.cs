using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Button : CustomRectangle
{
    string _text;

    Vector2 _textSize;
    Vector2 _textPosition;
    readonly float _fontScale = .2f;
    readonly SpriteFont _font;
    
    public Button(GraphicsDevice graphicsDevice, Point size, Point position, string text, SpriteFont font) 
        : base(graphicsDevice, size, position, Color.DarkGray)
    {
        _text = text;
        _font = font;
        
        _textSize = font.MeasureString(text) * _fontScale;
        
        _textPosition = new Vector2(
            Position.X + (Size.X - _textSize.X)/2, 
            Position.Y + (Size.Y - _textSize.Y) + 1);
    }

    public void ChangeText(string text)
    {
        _text = text;
        
        _textSize = _font.MeasureString(text) * _fontScale;
        
        _textPosition = new Vector2(
            Position.X + (Size.X - _textSize.X)/2, 
            Position.Y + (Size.Y - _textSize.Y) + 1);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawText(spriteBatch);
    }

    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(
            _font, _text, _textPosition, Color.Black, 
            0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.5f);
        spriteBatch.End();
    }
}