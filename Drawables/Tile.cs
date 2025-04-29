using _1toX.shared;
using _1toX.utils;
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
    readonly Color _textColor = new (21,21,21);

    readonly Color _correctBgColor = new (85, 231, 131);
    readonly Color _incorrectBgColor = new (231, 85, 85);

    readonly Border _border;
    
    public Tile(GraphicsDevice graphicsDevice, Point size, Point position, int value, FontSystem fontSystem) 
        : base(graphicsDevice, size, position, new Color(214, 214, 214))
    {
        Value = value;
        _font = fontSystem.GetFont(Constants.FontSizeTile);
        _valueStr = value.ToString();
        
        var textSize = _font.MeasureString(_valueStr);
        
        _textPosition = new Vector2(
            Position.X + (Size.X - textSize.X)/2, 
            Position.Y + (Size.Y - textSize.Y)/2);

        _border = new Border(graphicsDevice, position, new Point(Constants.TileSize), Color.White, Color.White, 4);
    }
    
    public void UseCorrectColor() => Color = _correctBgColor;
    public void UseIncorrectColor() => Color = _incorrectBgColor;
    public void Reveal() => IsRevealed = true;

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (IsRevealed) DrawText(spriteBatch);
        _border.Draw(spriteBatch, false);
    }

    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _valueStr, _textPosition, _textColor);
        spriteBatch.End();
    }
}