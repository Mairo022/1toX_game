using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Title
{
    readonly string _text = "1toX";
    readonly Color _textColor = new(119, 110, 101);

    readonly SpriteFont _font;
    readonly Vector2 _position;

    public Title(SpriteFont font)
    {
        _font = font;
        _position.X = Game1.PaddingX;
        _position.Y =  Game1.PaddingY - 10;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(
            _font, _text, _position, _textColor, 
            0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
        spriteBatch.End();
    }
}