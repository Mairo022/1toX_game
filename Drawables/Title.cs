using System;
using _1toX.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;

namespace _1toX;

public class Title
{
    const string Text = "1toX";
    readonly SpriteFontBase _font;
    readonly Color _color = new(81, 81, 81);
    
    readonly Vector2 _position;

    public Title(FontSystem fontSystem, GraphicsDevice graphicsDevice)
    {
        _font = fontSystem.GetFont(Constants.FontSizeHeader);
        var size = _font.MeasureString(Text);

        _position.X = (graphicsDevice.Viewport.Width - size.X) / 2;
        _position.Y = Constants.PaddingHeaderY;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, Text, _position, _color);
        spriteBatch.End();
    }
}