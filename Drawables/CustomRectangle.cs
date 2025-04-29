using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class CustomRectangle
{
    public Point Position;
    public Point Size;

    readonly Rectangle _rect;
    readonly Texture2D _texture;

    Color _activeColor;
    public Color Color;
    public Color HoverColor;

    public CustomRectangle(GraphicsDevice graphicsDevice, Point size, Point position, Color color, Color hoverColor = default)
    {
        Size = size;
        Position = position;
        Color = color;
        _activeColor = color;
        
        _rect = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
        _texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _texture.SetData([Color.White]);
    }

    public void UsePrimaryColor() { _activeColor = Color; }

    public void UseHoverColor()
    {
        _activeColor = HoverColor.A == 0 ? DarkenColor(Color, 0.9f) : HoverColor;
    }
    
    protected static Color DarkenColor(Color color, float factor)
    {
        factor = MathHelper.Clamp(factor, 0f, 1f);
        return new Color((int)(color.R * factor), (int)(color.G * factor), (int)(color.B * factor), color.A);
    }

    public bool Intersects(Point inputPosition)
    {
        return inputPosition.X >= Position.X
               && inputPosition.X <= Position.X + Size.X
               && inputPosition.Y >= Position.Y
               && inputPosition.Y <= Position.Y + Size.Y;
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_texture, _rect, _activeColor);
        spriteBatch.End();
    }
}