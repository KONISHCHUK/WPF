using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MinesweeperCalculator
{
    /// <summary>
    /// Main window for the calculator-themed hidden bomb game.
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        private const int GridRows = 5;
        private const int GridColumns = 4;
        private const int BombCount = 10;

        private const double DefaultWindowWidth = 340;
        private const double DefaultWindowHeight = 610;
        private const double ExpandedWindowWidth = 500;
        private const double ExpandedWindowHeight = 800;

        private const int DefaultButtonSize = 65;
        private const int ExpandedButtonSize = 90;
        private const int DefaultFontSize = 28;
        private const int ExpandedFontSize = 38;
        private const double ExpandedButtonAngle = 180;
        private const double DefaultButtonAngle = 0;
        private const int DoubleZeroWidthPadding = 8;
        private const double ScatterDistanceMin = 300.0;
        private const double ScatterDistanceRange = 400.0;

        private const double MinimizeRotationAngle = 90;
        private static readonly TimeSpan CollapseDuration = TimeSpan.FromSeconds(0.7);
        private static readonly TimeSpan ButtonTransformDuration = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan RotationRestoreDuration = TimeSpan.FromSeconds(0.7);
        private static readonly TimeSpan OopsFadeDuration = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan OopsDelay = TimeSpan.FromSeconds(2.5);
        private static readonly TimeSpan OopsStartDelay = TimeSpan.FromSeconds(0.8);
        private static readonly TimeSpan ScatterDurationMin = TimeSpan.FromSeconds(0.6);
        private static readonly TimeSpan ScatterDurationMax = TimeSpan.FromSeconds(1.2);
        private static readonly TimeSpan ResetRotationDuration = TimeSpan.FromSeconds(0.3);

        private static readonly string[] CalculatorLayout = { "C", "+/-", "%", "/", "7", "8", "9", "*", "4", "5", "6", "-", "1", "2", "3", "+", "0", "0", ".", "=" };

        private static readonly SolidColorBrush OperatorBrush = new(Color.FromRgb(255, 149, 0));
        private static readonly SolidColorBrush FunctionBrush = new(Color.FromRgb(166, 166, 166));
        private static readonly SolidColorBrush NumberBrush = new(Color.FromRgb(51, 51, 51));
        private static readonly SolidColorBrush AccentRevealedBrush = new(Color.FromRgb(255, 200, 150));
        private static readonly SolidColorBrush NumberRevealedBrush = new(Color.FromRgb(100, 100, 100));
        private static readonly SolidColorBrush MineBrush = new(Colors.DarkRed);
        private static readonly SolidColorBrush FlagBrush = new(Color.FromRgb(255, 200, 100));

        private readonly CalculatorGridGame game;
        private readonly Random random = new();
        private Button?[,] buttons = new Button?[GridRows, GridColumns];
        private bool isExpanded;

        public CalculatorWindow()
        {
            InitializeComponent();

            game = new CalculatorGridGame(GridRows, GridColumns, BombCount);
            game.StateChanged += UpdateBoard;

            InitializeCalculatorGrid();
            game.StartNewGame();
        }

        private void InitializeCalculatorGrid()
        {
            GameGrid.Children.Clear();
            buttons = new Button[GridRows, GridColumns];

            for (int row = 0; row < GridRows; row++)
            {
                for (int column = 0; column < GridColumns; column++)
                {
                    if (row == GridRows - 1 && column == 1)
                    {
                        continue;
                    }

                    string label = GetLabel(row, column);
                    Button button = CreateCalculatorButton(label, row, column);
                    buttons[row, column] = button;
                    GameGrid.Children.Add(button);
                }
            }
        }

        private Button CreateCalculatorButton(string label, int row, int column)
        {
            Button button = new Button
            {
                Content = label,
                Tag = new CellPosition(row, column),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(DefaultButtonAngle)
            };

            CalculatorButtonType type = GetButtonType(row, column, label);
            ApplyDefaultAppearance(button, type);

            button.MouseRightButtonDown += HandleRightClick;
            button.Click += HandleLeftClick;

            if (row == GridRows - 1 && column == 0)
            {
                Grid.SetColumnSpan(button, 2);
                button.Width = DefaultButtonSize * 2 + DoubleZeroWidthPadding;
            }

            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);

            return button;
        }

        private void HandleLeftClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not CellPosition position)
            {
                return;
            }

            if (position.Row == 0 && position.Column == 0)
            {
                ResetGame();
                return;
            }

            game.RevealCell(position.Row, position.Column);

            if (game.HasLost)
            {
                RevealMines();
            }
        }

        private void HandleRightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button && button.Tag is CellPosition position)
            {
                game.ToggleFlag(position.Row, position.Column);
                e.Handled = true;
            }
        }

        private void ResetGame()
        {
            ResetAnimations();
            game.StartNewGame();
            ResetButton.Visibility = Visibility.Collapsed;
        }

        private void UpdateBoard()
        {
            if (buttons == null)
            {
                return;
            }

            for (int row = 0; row < GridRows; row++)
            {
                for (int column = 0; column < GridColumns; column++)
                {
                    if (row == GridRows - 1 && column == 1)
                    {
                        continue;
                    }

                    Button? button = buttons[row, column];
                    if (button == null)
                    {
                        continue;
                    }

                    string label = GetLabel(row, column);
                    CalculatorButtonType type = GetButtonType(row, column, label);

                    if (game.IsFlagged(row, column))
                    {
                        button.Content = "F";
                        button.Background = FlagBrush;
                        button.Foreground = Brushes.Black;
                        button.IsEnabled = true;
                    }
                    else if (game.IsRevealed(row, column))
                    {
                        button.IsEnabled = false;
                        int cellValue = game.GetCellValue(row, column);
                        bool useAccentBackground = type != CalculatorButtonType.Number;

                        if (cellValue == -1)
                        {
                            button.Content = "E";
                            button.Background = MineBrush;
                            button.Foreground = Brushes.White;
                        }
                        else if (cellValue == 0)
                        {
                            button.Content = label;
                            button.Background = useAccentBackground ? AccentRevealedBrush : NumberRevealedBrush;
                            button.Foreground = Brushes.White;
                        }
                        else
                        {
                            button.Content = cellValue.ToString();
                            button.Background = useAccentBackground ? AccentRevealedBrush : NumberRevealedBrush;
                            button.Foreground = GetNumberColor(cellValue);
                        }
                    }
                    else
                    {
                        button.Content = label;
                        ApplyDefaultAppearance(button, type);
                        button.IsEnabled = true;
                    }
                }
            }

            DisplayLabel.Text = game.RemainingBombs.ToString();

            if (game.HasLost)
            {
                DisplayLabel.Text = "ERROR";
                DisplayLabel.Foreground = Brushes.Red;
                ResetButton.Visibility = Visibility.Visible;
                ScatterButtonsOnLoss();
            }
            else if (game.HasWon)
            {
                DisplayLabel.Text = "WIN";
                DisplayLabel.Foreground = Brushes.Green;
                ResetButton.Visibility = Visibility.Visible;
            }
            else
            {
                DisplayLabel.Foreground = Brushes.White;
                ResetButton.Visibility = Visibility.Collapsed;
            }
        }

        private void RevealMines()
        {
            for (int row = 0; row < GridRows; row++)
            {
                for (int column = 0; column < GridColumns; column++)
                {
                    if (row == GridRows - 1 && column == 1)
                    {
                        continue;
                    }

                    Button? button = buttons[row, column];
                    if (button == null)
                    {
                        continue;
                    }

                    int value = game.GetCellValue(row, column);
                    if (value == -1)
                    {
                        button.Content = "E";
                        button.Background = MineBrush;
                        button.Foreground = Brushes.White;
                        button.IsEnabled = false;
                    }
                }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Width = ExpandedWindowWidth;
            Height = ExpandedWindowHeight;

            RotateTransform? rotateTransform = FindName("WindowRotateTransform") as RotateTransform ?? RenderTransform as RotateTransform;
            if (rotateTransform != null)
            {
                DoubleAnimation rotationAnimation = CreateCubicAnimation(rotateTransform.Angle, MinimizeRotationAngle, CollapseDuration, EasingMode.EaseInOut);
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
            }

            FadeOutBoard();

            DispatcherTimer timer = new()
            {
                Interval = OopsStartDelay
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                ShowOopsOverlay();
            };
            timer.Start();
        }

        private void FadeOutBoard()
        {
            if (buttons != null)
            {
                for (int row = 0; row < GridRows; row++)
                {
                    for (int column = 0; column < GridColumns; column++)
                    {
                        Button? button = buttons[row, column];
                        if (button != null)
                        {
                            DoubleAnimation animation = CreateCubicAnimation(1.0, 0.0, CollapseDuration, EasingMode.EaseOut);
                            button.BeginAnimation(UIElement.OpacityProperty, animation);
                        }
                    }
                }
            }

            FadeOutElement(MinimizeButton);
            FadeOutElement(ExpandButton);
            FadeOutElement(DisplayLabel);

            if (ResetButton.Visibility == Visibility.Visible)
            {
                FadeOutElement(ResetButton);
            }
        }

        private void FadeOutElement(UIElement element)
        {
            DoubleAnimation animation = CreateCubicAnimation(1.0, 0.0, CollapseDuration, EasingMode.EaseOut);
            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void ShowOopsOverlay()
        {
            OopsLabel.Visibility = Visibility.Visible;

            DoubleAnimation fadeIn = CreateCubicAnimation(0.0, 1.0, OopsFadeDuration, EasingMode.EaseIn);
            OopsLabel.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            DispatcherTimer timer = new()
            {
                Interval = OopsDelay
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                RestoreAfterOops();
            };
            timer.Start();
        }

        private void RestoreAfterOops()
        {
            OopsLabel.Visibility = Visibility.Collapsed;
            OopsLabel.Opacity = 0;

            RotateTransform? rotateTransform = FindName("WindowRotateTransform") as RotateTransform ?? RenderTransform as RotateTransform;
            if (rotateTransform != null)
            {
                DoubleAnimation rotationAnimation = CreateCubicAnimation(rotateTransform.Angle, DefaultButtonAngle, RotationRestoreDuration, EasingMode.EaseInOut);
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
            }

            if (!isExpanded)
            {
                Width = DefaultWindowWidth;
                Height = DefaultWindowHeight;
            }

            ResetVisibilityAfterOops();
        }

        private void ResetVisibilityAfterOops()
        {
            if (buttons != null)
            {
                for (int row = 0; row < GridRows; row++)
                {
                    for (int column = 0; column < GridColumns; column++)
                    {
                        Button? button = buttons[row, column];
                        if (button != null)
                        {
                            button.Opacity = 1.0;
                            button.BeginAnimation(UIElement.OpacityProperty, null);
                        }
                    }
                }
            }

            ResetOpacity(MinimizeButton);
            ResetOpacity(ExpandButton);
            ResetOpacity(DisplayLabel);
            ResetOpacity(ResetButton);
        }

        private void ResetOpacity(UIElement element)
        {
            element.Opacity = 1.0;
            element.BeginAnimation(UIElement.OpacityProperty, null);
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (isExpanded)
            {
                Width = DefaultWindowWidth;
                Height = DefaultWindowHeight;
                ExpandButton.Content = "[]";
                ExpandButton.ToolTip = "Expand/Collapse Buttons";
                isExpanded = false;
                AnimateButtonsForExpansion(DefaultButtonAngle, DefaultButtonSize);
            }
            else
            {
                Width = ExpandedWindowWidth;
                Height = ExpandedWindowHeight;
                ExpandButton.Content = "<>";
                ExpandButton.ToolTip = "Shrink Buttons";
                isExpanded = true;
                AnimateButtonsForExpansion(ExpandedButtonAngle, ExpandedButtonSize);
            }
        }

        private void AnimateButtonsForExpansion(double targetAngle, int targetSize)
        {
            if (buttons == null)
            {
                return;
            }

            for (int row = 0; row < GridRows; row++)
            {
                for (int column = 0; column < GridColumns; column++)
                {
                    Button? button = buttons[row, column];
                    if (button == null)
                    {
                        continue;
                    }

                    RotateTransform rotateTransform = EnsureRotateTransform(button);
                    DoubleAnimation rotateAnimation = CreateCubicAnimation(rotateTransform.Angle, targetAngle, ButtonTransformDuration, EasingMode.EaseInOut);
                    rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

                    double currentWidth = button.ActualWidth <= 0 ? DefaultButtonSize : button.ActualWidth;
                    double widthTarget = (row == GridRows - 1 && column == 0) ? targetSize * 2 + DoubleZeroWidthPadding : targetSize;
                    DoubleAnimation widthAnimation = CreateCubicAnimation(currentWidth, widthTarget, ButtonTransformDuration, EasingMode.EaseInOut);
                    button.BeginAnimation(Button.WidthProperty, widthAnimation);

                    double currentHeight = button.ActualHeight <= 0 ? DefaultButtonSize : button.ActualHeight;
                    DoubleAnimation heightAnimation = CreateCubicAnimation(currentHeight, targetSize, ButtonTransformDuration, EasingMode.EaseInOut);
                    button.BeginAnimation(Button.HeightProperty, heightAnimation);

                    button.FontSize = targetSize == DefaultButtonSize ? DefaultFontSize : ExpandedFontSize;
                }
            }
        }

        private void ScatterButtonsOnLoss()
        {
            if (buttons == null)
            {
                return;
            }

            for (int row = 0; row < GridRows; row++)
            {
                for (int column = 0; column < GridColumns; column++)
                {
                    Button? button = buttons[row, column];
                    if (button == null)
                    {
                        continue;
                    }

                    TransformGroup transformGroup = EnsureTransformGroup(button);

                    TranslateTransform translate = transformGroup.Children.OfType<TranslateTransform>().FirstOrDefault() ?? new TranslateTransform();
                    if (!transformGroup.Children.Contains(translate))
                    {
                        transformGroup.Children.Add(translate);
                    }

                    double direction = random.NextDouble() * 2.0 * Math.PI;
                    double distance = ScatterDistanceMin + random.NextDouble() * ScatterDistanceRange;
                    double targetX = Math.Cos(direction) * distance;
                    double targetY = Math.Sin(direction) * distance;

                    TimeSpan durationX = TimeSpan.FromSeconds(ScatterDurationMin.TotalSeconds + random.NextDouble() * (ScatterDurationMax.TotalSeconds - ScatterDurationMin.TotalSeconds));
                    TimeSpan durationY = TimeSpan.FromSeconds(ScatterDurationMin.TotalSeconds + random.NextDouble() * (ScatterDurationMax.TotalSeconds - ScatterDurationMin.TotalSeconds));

                    DoubleAnimation translateX = CreateCubicAnimation(0, targetX, durationX, EasingMode.EaseOut);
                    DoubleAnimation translateY = CreateCubicAnimation(0, targetY, durationY, EasingMode.EaseOut);

                    RotateTransform rotation = transformGroup.Children.OfType<RotateTransform>().FirstOrDefault() ?? new RotateTransform();
                    if (!transformGroup.Children.Contains(rotation))
                    {
                        transformGroup.Children.Add(rotation);
                    }

                    double additionalRotation = (random.NextDouble() - 0.5) * 720.0;
                    TimeSpan rotationDuration = TimeSpan.FromSeconds(ScatterDurationMin.TotalSeconds + random.NextDouble() * (ScatterDurationMax.TotalSeconds - ScatterDurationMin.TotalSeconds));
                    DoubleAnimation rotateAnimation = CreateCubicAnimation(rotation.Angle, rotation.Angle + additionalRotation, rotationDuration, EasingMode.EaseOut);
                    rotation.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

                    TimeSpan fadeDuration = TimeSpan.FromSeconds(ScatterDurationMin.TotalSeconds + random.NextDouble() * 0.3);
                    DoubleAnimation fadeAnimation = CreateCubicAnimation(1.0, 0.0, fadeDuration, EasingMode.EaseOut);
                    button.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);

                    translate.BeginAnimation(TranslateTransform.XProperty, translateX);
                    translate.BeginAnimation(TranslateTransform.YProperty, translateY);
                }
            }
        }

        private void ResetAnimations()
        {
            if (buttons == null)
            {
                return;
            }

            for (int row = 0; row < GridRows; row++)
            {
                for (int column = 0; column < GridColumns; column++)
                {
                    Button? button = buttons[row, column];
                    if (button == null)
                    {
                        continue;
                    }

                    if (button.RenderTransform is TransformGroup group)
                    {
                        TranslateTransform? translate = group.Children.OfType<TranslateTransform>().FirstOrDefault();
                        if (translate != null)
                        {
                            translate.BeginAnimation(TranslateTransform.XProperty, null);
                            translate.BeginAnimation(TranslateTransform.YProperty, null);
                            translate.X = 0;
                            translate.Y = 0;
                        }

                        RotateTransform? rotation = group.Children.OfType<RotateTransform>().FirstOrDefault();
                        if (rotation != null)
                        {
                            rotation.BeginAnimation(RotateTransform.AngleProperty, null);
                            double targetAngle = isExpanded ? ExpandedButtonAngle : DefaultButtonAngle;
                            DoubleAnimation rotateBack = CreateCubicAnimation(rotation.Angle, targetAngle, ResetRotationDuration, EasingMode.EaseInOut);
                            rotation.BeginAnimation(RotateTransform.AngleProperty, rotateBack);
                        }
                    }
                    else if (button.RenderTransform is RotateTransform rotation)
                    {
                        rotation.BeginAnimation(RotateTransform.AngleProperty, null);
                        rotation.Angle = isExpanded ? ExpandedButtonAngle : DefaultButtonAngle;
                    }

                    button.Opacity = 1.0;
                    button.BeginAnimation(UIElement.OpacityProperty, null);
                }
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private static string GetLabel(int row, int column)
        {
            int index = row * GridColumns + column;
            return index >= 0 && index < CalculatorLayout.Length ? CalculatorLayout[index] : string.Empty;
        }

        private static CalculatorButtonType GetButtonType(int row, int column, string label)
        {
            if (label == "=")
            {
                return CalculatorButtonType.Equals;
            }

            if (column == GridColumns - 1)
            {
                return CalculatorButtonType.Operator;
            }

            if (row == 0)
            {
                return CalculatorButtonType.Function;
            }

            return CalculatorButtonType.Number;
        }

        private void ApplyDefaultAppearance(Button button, CalculatorButtonType type)
        {
            switch (type)
            {
                case CalculatorButtonType.Operator:
                case CalculatorButtonType.Equals:
                    button.Style = (Style)FindResource("OperatorButtonStyle");
                    button.Background = OperatorBrush;
                    button.Foreground = Brushes.White;
                    break;
                case CalculatorButtonType.Function:
                    button.Style = (Style)FindResource("FunctionButtonStyle");
                    button.Background = FunctionBrush;
                    button.Foreground = Brushes.Black;
                    break;
                default:
                    button.Style = (Style)FindResource("NumberButtonStyle");
                    button.Background = NumberBrush;
                    button.Foreground = Brushes.White;
                    break;
            }
        }

        private Brush GetNumberColor(int value)
        {
            return value switch
            {
                1 => Brushes.Blue,
                2 => Brushes.Green,
                3 => Brushes.Red,
                4 => Brushes.DarkBlue,
                5 => Brushes.DarkRed,
                6 => Brushes.Teal,
                7 => Brushes.Black,
                8 => Brushes.Gray,
                _ => Brushes.Black
            };
        }

        private RotateTransform EnsureRotateTransform(Button button)
        {
            if (button.RenderTransform is RotateTransform existingRotate)
            {
                return existingRotate;
            }

            RotateTransform rotate = new()
            {
                Angle = isExpanded ? ExpandedButtonAngle : DefaultButtonAngle
            };
            button.RenderTransformOrigin = new Point(0.5, 0.5);
            button.RenderTransform = rotate;
            return rotate;
        }

        private TransformGroup EnsureTransformGroup(Button button)
        {
            if (button.RenderTransform is TransformGroup group)
            {
                return group;
            }

            TransformGroup transformGroup = new();
            if (button.RenderTransform is RotateTransform rotate)
            {
                transformGroup.Children.Add(rotate);
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform(isExpanded ? ExpandedButtonAngle : DefaultButtonAngle));
            }

            button.RenderTransform = transformGroup;
            return transformGroup;
        }

        private static DoubleAnimation CreateCubicAnimation(double from, double to, TimeSpan duration, EasingMode easingMode)
        {
            return new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(duration),
                EasingFunction = new CubicEase { EasingMode = easingMode }
            };
        }

        private enum CalculatorButtonType
        {
            Number,
            Function,
            Operator,
            Equals
        }

        private readonly record struct CellPosition(int Row, int Column);
    }
}
