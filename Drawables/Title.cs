using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;

namespace _1toX;

public class Title
{
    const string Text = "1toX";
    SpriteFontBase _font;
    readonly Color _textColor = new(119, 110, 101);
    
    readonly Vector2 _position;

    public Title(FontSystem fontSystem)
    {
        _position.X = Game1.PaddingX;
        _position.Y =  Game1.PaddingY - 10;
        
        _font = fontSystem.GetFont(20);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.DrawString(_font, Text, _position, _textColor);
        spriteBatch.End();
    }
}