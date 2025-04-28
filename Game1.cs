using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace _1toX;

public class Game1 : Game
{
    public const int PaddingX = 24;
    public const int PaddingY = 24;

    int _tilesRevealedCycles;
    int _tilesRevealedMs;
    int _cyclesPassed;
    bool _tilesRevealed = true;
    int _totalTiles = 7;

    SpriteBatch _spriteBatch;
    FontSystem _fontSystem = new();
    Title _title;
    Board _board;
    Button _startBtn;
    InputField _timeInput;
    InputField _tilesInput;
    
    Color _bgColor = new(250, 248, 239);
    GameState _gameState;
    
    bool _lmbPressed;
    MouseState _lastMouseState;
    KeyboardState _keyboard;
    KeyboardState _keyboardLast;
    
    List<Tile> _tiles = [];
    int _lastPickedValue = -1;

    public Game1()
    {
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = 440;
        graphics.PreferredBackBufferHeight = 640;
        Window.Position = new Point(0, 600);
    }

    protected override void Initialize()
    {
        _gameState = GameState.Menu;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _tilesRevealedMs = 600;
        _tilesRevealedCycles = GetTilesRevealedCycles(_tilesRevealedMs);
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _fontSystem.AddFont(File.ReadAllBytes("Content/Roboto-Regular.ttf"));
        
        _title = new Title(_fontSystem);
        _board = new Board(GraphicsDevice);
        _startBtn = new Button(GraphicsDevice,
            new Point(120, 50), 
            new Point(GraphicsDevice.Viewport.Width - PaddingX - 120, _board.Position.Y - PaddingY/2 - 50), 
            "Start", _fontSystem);
        
        _timeInput = new InputField(GraphicsDevice,
            new Point(100, 50), 
            new Point(PaddingX, _startBtn.Position.Y), 
            _tilesRevealedMs.ToString(), _fontSystem, 1000);
        
        _tilesInput = new InputField(GraphicsDevice,
            new Point(100, 50), 
            new Point(_timeInput.Position.X + PaddingX/2 + _timeInput.Size.X, _startBtn.Position.Y), 
            _totalTiles.ToString(), _fontSystem, 16);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)
            ) Exit();

        _keyboard = Keyboard.GetState();
        _lastMouseState = Mouse.GetState();

        if (!IsWithinGameBounds(_lastMouseState.Position))
        {
            base.Update(gameTime);
            return;
        }
        
        if (_lastPickedValue == _tiles.Count)
        {
            StartGame();
        }
        
        HandleTileIntersect();
        HandleStartBtnIntersect();
        HandleTileHiding();
        HandleInputIntersect();
        HandleInputWrite();

        _lmbPressed = _lastMouseState.LeftButton == ButtonState.Pressed;
        _keyboardLast = _keyboard;
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_bgColor);
        
        _title.Draw(_spriteBatch);
        _board.Draw(_spriteBatch);
        _startBtn.Draw(_spriteBatch);
        _timeInput.Draw(_spriteBatch);
        _tilesInput.Draw(_spriteBatch);
        
        foreach (var tile in _tiles) 
            tile.Draw(_spriteBatch);
        
        base.Draw(gameTime);
    }
    
    
    // Helpers

    bool IsWithinGameBounds(Point point)
    {
        return point.X >= 0 
               && point.X < Window.ClientBounds.Width
               && point.Y >= 0
               && point.Y < Window.ClientBounds.Height;
    }
    
    void CreateTiles()
    {
        _tiles.Clear();
        var posIndexes = Enumerable.Range(0, _board.Positions.Count).OrderBy(_ => Guid.NewGuid()).ToArray();
        
        var value = 0;
        for (var i = 0; i < _totalTiles; i++)
        {
            value++;
            _tiles.Add(new Tile(GraphicsDevice, new Point(70, 70), 
                    _board.Positions[posIndexes[i]], value, _fontSystem));
        }
    }

    void HandleInputWrite()
    {
        if (_tilesInput.IsFocused())
        {
            _tilesInput.HandleWrite(_keyboard, _keyboardLast, out var output);
            _totalTiles = Convert.ToInt32(output);
        }
        else if (_timeInput.IsFocused())
        {
            _timeInput.HandleWrite(_keyboard, _keyboardLast, out var output);
            _tilesRevealedMs = Convert.ToInt32(output);
            _tilesRevealedCycles = GetTilesRevealedCycles(_tilesRevealedMs);
        }
    }

    void HandleInputIntersect()
    {
        var intersectsTimeInput = _timeInput.Intersects(_lastMouseState.Position);
        var intersectsTilesInput = _tilesInput.Intersects(_lastMouseState.Position);
        
        if (intersectsTilesInput)
        {
            _tilesInput.HandleMouseHover();

            if (_lmbPressed) return;
            if (_lastMouseState.LeftButton == ButtonState.Pressed)
            {
                UnfocusInputFields();
                _tilesInput.HandleMouseClick(_lastMouseState.Position);
            }
        }
        else if (intersectsTimeInput)
        {
            _timeInput.HandleMouseHover();
            
            if (_lmbPressed) return;
            if (_lastMouseState.LeftButton == ButtonState.Pressed)
            {
                UnfocusInputFields();
                _timeInput.HandleMouseClick(_lastMouseState.Position);
            }
        }
        else
        {
            if (Cursor.Current != Cursors.Arrow)
                Mouse.SetCursor(MouseCursor.Arrow);

            if (_lmbPressed) return;
            if (_lastMouseState.LeftButton == ButtonState.Pressed)
                UnfocusInputFields();
        }
    }

    void UnfocusInputFields()
    {
        _tilesInput.HandleMouseOffClick();
        _timeInput.HandleMouseOffClick();
    }

    void HandleStartBtnIntersect()
    {
        var intersects = _startBtn.Intersects(_lastMouseState.Position);
        
        if (intersects) _startBtn.UseHoverColor();
        else _startBtn.UsePrimaryColor();

        if (_lmbPressed) return;
        
        if (intersects && _lastMouseState.LeftButton == ButtonState.Pressed)
        {
            _gameState = GameState.Running;
            _startBtn.ChangeText("Restart");
            StartGame();
        }
    }
    
    void HandleTileIntersect()
    {
        foreach (var tile in _tiles)
        {
            var tileIntersects = tile.Intersects(_lastMouseState.Position);

            if (tileIntersects) tile.UseHoverColor(); else tile.UsePrimaryColor();
            
            if (_lmbPressed) return;
            
            if (tileIntersects && _lastMouseState.LeftButton == ButtonState.Pressed)
            {
                if (tile.IsRevealed) continue;
                
                if (_lastPickedValue + 1 != tile.Value)
                {
                    tile.UseIncorrectColor();
                    foreach (var tile2 in _tiles) tile2.Reveal();
                    _gameState = GameState.Menu;
                    continue;
                }

                if (_lastPickedValue + 1 == tile.Value)
                {
                    tile.Reveal();
                    tile.UseCorrectColor();
                    _lastPickedValue++;                           
                }
            }
        }
    }
    
    void HandleTileHiding()
    {
        if (_gameState != GameState.Running || !_tilesRevealed) return;
        
        _cyclesPassed++;
        if (_cyclesPassed != _tilesRevealedCycles) return;
        
        _cyclesPassed = 0;
        _tilesRevealed = false;

        foreach (var tile in _tiles) tile.IsRevealed = false;
    }
    
    void StartGame()
    {
        _lastPickedValue = 0;
        _tilesRevealed = true;
        CreateTiles();
    }
    
    Func<int, int> GetTilesRevealedCycles => ms => (int) Math.Ceiling(ms / 16.6);
}

enum GameState
{
    Menu,
    Running
}