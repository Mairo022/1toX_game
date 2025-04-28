using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Tile : CustomRectangle
{
    public bool IsRevealed = true;
    
    public readonly int Value;
    readonly string _valueStr;

    readonly SpriteFontBase _font;

    readonly Vector2 _textPosition;
    readonly Color _textColor = new (22,22,22);

    readonly Color _correctColor = Color.PaleGreen;
    readonly Color _incorrectColor = Color.Firebrick;
    
    public Tile(GraphicsDevice graphicsDevice, Point size, Point position, int value, FontSystem fontSystem) 
        : base(graphicsDevice, size, position, Color.White)
    {
        Color = Color.White;
        Value = value;
        _font = fontSystem.GetFont(32);
        _valueStr = value.ToString();
        
        var textSize = _font.MeasureString(_valueStr);
        
        _textPosition = new Vector2(
            Position.X + (Size.X - textSize.X)/2, 
            Position.Y + (Size.Y - textSize.Y)/2);
    }
    
    public void UseCorrectColor() => Color = _correctColor;
    public void UseIncorrectColor() => Color = _incorrectColor;
    public void Reveal() => IsRevealed = true;

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (IsRevealed) DrawText(spriteBatch);
    }

    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _valueStr, _textPosition, _textColor);
        spriteBatch.End();
    }
}