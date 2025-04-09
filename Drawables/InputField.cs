using System.Collections.Generic;
using System.Windows.Forms;
using _1toX.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _1toX;

public class InputField : CustomRectangle
{
    readonly GraphicsDevice _graphicsDevice;

    string _text;
    readonly int _maxValue;

    readonly Vector2 _textPosition;
    readonly Color _textColor = new (120, 120, 120);
    readonly float _fontScale = .2f;
    readonly SpriteFont _font;
    
    bool _inFocus;
    readonly Color _borderColorFocus = new(10, 10, 10);
    readonly Color _borderColorUnfocus = new(100, 100, 100);

    bool _isBorderActive;
    Color _borderColor;
    Texture2D _borderTexture;
    readonly List<Rectangle> _borders = new(4);
    
    public InputField(GraphicsDevice graphicsDevice, Point size, Point position, string text, SpriteFont font, int maxValue, bool addBorder = true)
        : base(graphicsDevice, size, position, Color.White)
    {
        _graphicsDevice = graphicsDevice;
        _text = text;
        _font = font;
        _maxValue = maxValue;

        _borderColor = _borderColorUnfocus;
        _isBorderActive = addBorder;
        if (_isBorderActive) CreateBorder();
        
        var textSize = font.MeasureString(text) * _fontScale;
        
        _textPosition = new Vector2(
            Position.X + 16, 
            Position.Y + (Size.Y - textSize.Y) + 3);
    }

    void CreateBorder()
    {
        // Top, bottom
        _borders.Add(new Rectangle(Position.X, Position.Y, Size.X, 1));
        _borders.Add(new Rectangle(Position.X, Position.Y + Size.Y, Size.X, 1));
        // Left, right
        _borders.Add(new Rectangle(Position.X, Position.Y, 1, Size.Y));
        _borders.Add(new Rectangle(Position.X + Size.X, Position.Y, 1, Size.Y + 1));
        
        _borderTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        _borderTexture.SetData([Color.White]);
    }

    public void HandleWrite(KeyboardState keyboard, KeyboardState oldKeyboard, out string value)
    {
        value = _text;
        
        if (KeyboardUtils.TryConvertKeyboardInput(keyboard, oldKeyboard, out var key) && 
            KeyboardUtils.IsCharNumber(key))
        {
            if (_text.Length > 0 && int.Parse(_text + key) > _maxValue)
            {
                _text = _maxValue.ToString();
                value = _text;
                return;
            }

            if (_text.Length == 4) return;
            
            _text += key.ToString();
            value = _text;
            return;
        }

        if (_text.Length == 0)
        {
            value = "0";
            return;
        }
        
        if (KeyboardUtils.IsBackKey(keyboard, oldKeyboard))
            _text = _text[..^1];
        
        value = _text;

        if (_text.Length == 0) value = "0";
    }
    
    public void HandleMouseClick()
    {
        _inFocus = true;
        _borderColor = _borderColorFocus;
    }

    public void HandleMouseOffClick()
    {
        _inFocus = false;
        _borderColor = _borderColorUnfocus;
    }

    public bool IsFocused()
    {
        return _inFocus;
    }

    public void HandleMouseHover()
    {
        if (Cursor.Current == Cursors.IBeam) return;
        Mouse.SetCursor(MouseCursor.IBeam);
    }

    public void AddBorder()
    {
        if (_isBorderActive) return;
        _isBorderActive = true;
        CreateBorder();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawText(spriteBatch);
        if (_isBorderActive) DrawBorder(spriteBatch);
    }

    void DrawBorder(SpriteBatch spriteBatch)
    {
        foreach (var border in _borders)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_borderTexture, border, _borderColor);
            spriteBatch.End();           
        }
    }
    
    void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(
            _font, _text, _textPosition, _textColor, 
            0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.5f);
        spriteBatch.End();
    }
}