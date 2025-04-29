using System.Collections.Generic;
using _1toX.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1toX;

public class Board
{
    public readonly List<Point> Positions = [];
    readonly Point _position;

    readonly Rectangle _rect;
    readonly Texture2D _texture;
    readonly Color _color = new(106, 165, 220);

    public Board(GraphicsDevice graphicsDevice)
    {
        _position.Y = Constants.HeaderHeight;
        
        _rect = new Rectangle(_position.X, _position.Y, Constants.BoardWidth, Constants.BoardHeight);
        _texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _texture.SetData([Color.White]);

        CreateTilePositions();
    }

    void CreateTilePositions()
    {
        var x = Constants.PaddingBoardX;
        var y = _position.Y + Constants.PaddingBoardY;

        for (var i = 0; i < Constants.TilesPerRow; i++)
        {
            for (var j = 0; j < Constants.TilesPerColumn; j++)
            {
                Positions.Add(new Point(x, y));
                x += Constants.TileSize + Constants.TileGap;
            }

            x = Constants.PaddingBoardX;
            y += Constants.TileSize + Constants.TileGap;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_texture, _rect, _color);
        spriteBatch.End();
    }
}