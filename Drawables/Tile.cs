using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Tile : CustomRectangle
{
    public bool IsRevealed = true;
    
    public readonly int Value;
    readonly string _valueStr;

    readonly SpriteFont _font;
    readonly float _fontScale = 0.32f;

    readonly Vector2 _textPosition;
    readonly Color _textColor = new (22,22,22);

    readonly Color _correctColor = Color.PaleGreen;
    readonly Color _incorrectColor = Color.Firebrick;
    
    public Tile(GraphicsDevice graphicsDevice, Point size, Point position, int value, SpriteFont font) 
        : base(graphicsDevice, size, position, Color.White)
    {
        Color = Color.White;
        Value = value;
        _font = font;
        _valueStr = value.ToString();
        
        var textSize = _font.MeasureString(_valueStr) * _fontScale;
        
        _textPosition = new Vector2(
            Position.X + (Size.X - textSize.X)/2 + 3, 
            Position.Y + (Size.Y - textSize.Y) + 8);
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
        spriteBatch.DrawString(
            _font, _valueStr, _textPosition, _textColor, 
            0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.5f);
        spriteBatch.End();
    }
}