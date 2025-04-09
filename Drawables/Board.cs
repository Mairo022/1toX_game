using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Board
{
    readonly int _tileSize = 70;
    readonly int _size;
    public readonly List<Point> Positions = [];
    public Point Position;

    readonly Rectangle _rect;
    readonly Texture2D _texture;

    public Board(GraphicsDevice graphicsDevice)
    {
        _size = graphicsDevice.Viewport.Width - Game1.PaddingX * 2;
        Position.X = Game1.PaddingX;
        Position.Y = graphicsDevice.Viewport.Height - Game1.PaddingY - _size;
        
        _rect = new Rectangle(Position.X, Position.Y, _size, _size);
        _texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _texture.SetData([Color.White]);

        CreateTilePositions();
    }

    void CreateTilePositions()
    {
        var x = Position.X + 24;
        var y = Position.Y + 24;
        var gap = (_size - Game1.PaddingX * 2 - _tileSize * 4) / 3;

        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                Positions.Add(new Point(x, y));
                x += _tileSize + gap;
            }

            x = Position.X + 24;
            y += _tileSize + gap;
            
            if (i == 2) y += 1;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_texture, _rect, Color.DarkGray);
        spriteBatch.End();
    }
}