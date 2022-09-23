﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DungeonCrawler;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DungeonCrawler
{
    public class DialogBox
    {
        /// <summary>
        /// All text contained in this dialog box
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Signal to toggle dialog.
        /// </summary>
        public bool StartDialog { get; set; }

        /// <summary>
        /// Bool that determines active state of this dialog box
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// X,Y coordinates of this dialog box
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Width and Height of this dialog box
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Color used to fill dialog box background
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Color used for border around dialog box
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Color used for text in dialog box
        /// </summary>
        public Color DialogColor { get; set; }

        /// <summary>
        /// Thickness of border
        /// </summary>
        public int BorderWidth { get; set; }

        /// <summary>
        /// Background fill texture (built from FillColor)
        /// </summary>
        private readonly Texture2D _fillTexture;

        /// <summary>
        /// Border fill texture (built from BorderColor)
        /// </summary>
        private readonly Texture2D _borderTexture;

        /// <summary>
        /// Collection of pages contained in this dialog box
        /// </summary>
        private List<string> _pages;

        /// <summary>
        /// Margin surrounding the text inside the dialog box
        /// </summary>
        private const float DialogBoxMargin = 24f;

        /// <summary>
        /// Size (in pixels) of a wide alphabet letter (W is the widest letter in almost every font) 
        /// </summary>
        private Vector2 _characterSize;

        /// <summary>
        /// The amount of characters allowed on a given line
        /// NOTE: If you want to use a font that is not monospaced, this will need to be reevaluated
        /// </summary>
        private int MaxCharsPerLine => 30;

        /// <summary>
        /// Determine the maximum amount of lines allowed per page
        /// NOTE: This will change automatically with font size
        /// </summary>
        private int MaxLines => 2;

        /// <summary>
        /// The index of the current page
        /// </summary>
        private int _currentPage;

        /// <summary>
        /// The stopwatch interval (used for blinking indicator)
        /// </summary>
        private int _interval;

        /// <summary>
        /// The position and size of the dialog box fill rectangle
        /// </summary>
        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        /// <summary>
        /// The position and size of the bordering sides on the edges of the dialog box
        /// </summary>
        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            // Top (contains top-left & top-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Right
            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            // Bottom (contains bottom-left & bottom-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Left
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        /// <summary>
        /// The starting position of the text inside the dialog box
        /// </summary>
        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin / 2, Position.Y + DialogBoxMargin / 2);

        /// <summary>
        /// Stopwatch used for the blinking (next page) indicator
        /// </summary>
        private Stopwatch _stopwatch;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// 
        private SpriteFont font;
        private Game game;
        private Stopwatch stopWatch;


        public DialogBox(Game game, SpriteFont font)
        {
            this.font = font;
            this.game = game;
            BorderWidth = 2;
            DialogColor = Color.White;
            _characterSize = font.MeasureString(new StringBuilder("W", 1));
            stopWatch = new Stopwatch();
            stopWatch.Start();
            FillColor = new Color(0f, 0f, 0f, 0.8f);

            BorderColor = new Color(0f, 0f, 0f, 0.8f);

            _fillTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _fillTexture.SetData(new[] { FillColor });

            _borderTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _borderTexture.SetData(new[] { BorderColor });

            _pages = new List<string>();
            _currentPage = 0;

            var sizeX = (int)(250);
            var sizeY = (int)(50);

            Size = new Vector2(sizeX, sizeY);

            var posX = Game1.CenterScreen.X - (Size.X / 2f);
            var posY = game.GraphicsDevice.Viewport.Height - Size.Y - 30;

            Position = new Vector2(posX, posY);
        }

        /// <summary>
        /// Initialize a dialog box
        /// - can be used to reset the current dialog box in case of "I didn't quite get that..."
        /// </summary>
        /// <param name="text"></param>
        public void Initialize(string text = null)
        {
            Text = text ?? Text;

            _currentPage = 0;

            Show();
        }

        /// <summary>
        /// Show the dialog box on screen
        /// - invoke this method manually if Text changes
        /// </summary>
        public void Show()
        {
            Active = true;

            // use stopwatch to manage blinking indicator
            _stopwatch = new Stopwatch();

            _stopwatch.Start();

            _pages = WordWrap(Text);
        }

        /// <summary>
        /// Manually hide the dialog box
        /// </summary>
        public void Hide()
        {
            Active = false;

            _stopwatch.Stop();

            _stopwatch = null;
        }

        public bool IsActive()
        {
            return Active;
        }

        /// <summary>
        /// Process input for dialog box
        /// </summary>
        public void Update()
        {
            if (Active)
            {
                // Button press will proceed to the next page of the dialog box
                if ((Init.KeyBoardNewState.IsKeyDown(Keys.E) && Init.KeyBoardOldState.IsKeyUp(Keys.E)))
                {
                    if (_currentPage >= _pages.Count - 1)
                    {
                        Hide();
                    }
                    else
                    {
                        pageContent = "";
                        timer = 0;
                        index = 0;
                        _currentPage++;
                        _stopwatch.Restart();
                    }
                }

                // Shortcut button to skip entire dialog box
                if ((Init.KeyBoardNewState.IsKeyDown(Keys.X) && Init.KeyBoardOldState.IsKeyUp(Keys.X)))
                {
                    Hide();
                }
            }
            else
            {
                _currentPage = 0;
            }
        }

        /// <summary>
        /// Draw the dialog box on screen if it's currently active
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// 

        float timer = 0;
        int index = 0;
        string pageContent = "";

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                // Draw each side of the border rectangle
                foreach (var side in BorderRectangles)
                {
                    spriteBatch.Draw(_borderTexture, null, side);
                }

                // Draw background fill texture (in this example, it's 50% transparent white)
                spriteBatch.Draw(_fillTexture, null, TextRectangle);

                timer += stopWatch.ElapsedMilliseconds;

                if (stopWatch.ElapsedMilliseconds > 25 && index < _pages[_currentPage].Length)
                {
                    pageContent += _pages[_currentPage][index];
                    index++;
                    stopWatch.Restart();
                }

                spriteBatch.DrawString(font, pageContent, TextPosition, DialogColor);

                // Draw a blinking indicator to guide the player through to the next page
                // This stops blinking on the last page
                // NOTE: You probably want to use an image here instead of a string
                if (BlinkIndicator() || _currentPage == _pages.Count - 1)
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4,
                        TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));

                  //  spriteBatch.DrawString(font, "", indicatorPosition, Color.White);
                }
            }
            else
            {
                StartDialog = false;
            }
        }

        /// <summary>
        /// Whether the indicator should be visible or not
        /// </summary>
        /// <returns></returns>
        private bool BlinkIndicator()
        {
            _interval = (int)Math.Floor((double)(_stopwatch.ElapsedMilliseconds % 1000));

            return _interval < 500;
        }

        /// <summary>
        /// Wrap words to the next line where applicable
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine * MaxLines > text.Length ? text.Length : MaxCharsPerLine * MaxLines;

            var result = new StringBuilder(capacity);
            var resultLines = 0;

            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;

                currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine)
                    {
                        result.AppendLine(currentLine.ToString());

                        currentLine.Clear();

                        resultLines++;
                    }

                    currentLine.Append(currentWord);

                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
                    }
                }
            }

            return pages;
        }
    }
}
