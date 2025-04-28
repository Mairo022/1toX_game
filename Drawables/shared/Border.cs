
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX.shared;

public class Border
{
    readonly Color _color;
    readonly Color _colorFocused;
    readonly Texture2D _texture;
    readonly List<Rectangle> _borders = new(4);

    public Border(GraphicsDevice graphicsDevice, Point position, Point size, Color color, Color colorFocused, int width = 1)
    {
        // Top, bottom, left, right
        _borders.Add(new Rectangle(position.X, position.Y, size.X, width));
        _borders.Add(new Rectangle(position.X, position.Y + size.Y, size.X, width));
        _borders.Add(new Rectangle(position.X, position.Y, 1, size.Y));
        _borders.Add(new Rectangle(position.X + size.X, position.Y, width, size.Y + width));
        
        _texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _texture.SetData([Color.White]);

        _color = color;
        _colorFocused = colorFocused;
    }
    
    public void Draw(SpriteBatch spriteBatch, bool isFocused)
    {
        foreach (var border in _borders)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, border, isFocused ? _colorFocused : _color);
            spriteBatch.End();           
        }
    }
}