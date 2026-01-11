using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MinesweeperCalculator
{
    public partial class lol : Window
    {
        private stuff a2;
        private Button[,] b2;
        private bool b3 = false;
        private const int c1 = 340;
        private const int c2 = 610;
        private const int c3 = 500;
        private const int c4 = 800;
        private const int c5 = 65;
        private const int c6 = 90;
        private string[] d1 = { "C", "±", "%", "÷", "7", "8", "9", "×", "4", "5", "6", "-", "1", "2", "3", "+", "0", "0", ".", "=" };

        public lol()
        {
            InitializeComponent();
            a2 = new stuff(5, 4, 10);
            a2.a16 += e1;
            e2();
        }

        private void e2()
        {
            GameGrid.Children.Clear();
            b2 = new Button[5, 4];
            int f1 = 0;
            int row = 0;
            int col = 0;
            for (row = 0; row < 5; row = row + 1)
            {
                for (col = 0; col < 4; col = col + 1)
                {
                    Button g1 = new Button();
                    string g2 = "";
                    if (f1 < d1.Length)
                    {
                        g2 = d1[f1];
                    }
                    else
                    {
                        g2 = "";
                    }

                    if (row == 4 && col == 0)
                    {
                        g1.Content = "0";
                        Grid.SetColumnSpan(g1, 2);
                        g1.Width = 138;
                        Grid.SetRow(g1, row);
                        Grid.SetColumn(g1, col);
                    }
                    else
                    {
                        if (row == 4 && col == 1)
                        {
                            f1 = f1 + 1;
                            continue;
                        }
                        else
                        {
                            g1.Content = g2;
                            Grid.SetRow(g1, row);
                            Grid.SetColumn(g1, col);
                        }
                    }

                    bool h1 = false;
                    if (col == 3 && g2 != "")
                    {
                        h1 = true;
                    }
                    else
                    {
                        h1 = false;
                    }

                    bool h2 = false;
                    if (row == 0 && col < 3)
                    {
                        h2 = true;
                    }
                    else
                    {
                        h2 = false;
                    }

                    bool h3 = false;
                    if (g2 == "=")
                    {
                        h3 = true;
                    }
                    else
                    {
                        h3 = false;
                    }

                    if (h1 == true || h3 == true)
                    {
                        g1.Style = (Style)FindResource("OperatorButtonStyle");
                    }
                    else
                    {
                        if (h2 == true)
                        {
                            g1.Style = (Style)FindResource("FunctionButtonStyle");
                        }
                        else
                        {
                            g1.Style = (Style)FindResource("NumberButtonStyle");
                        }
                    }

                    g1.Tag = (row, col);
                    g1.MouseRightButtonDown += i1;
                    g1.Click += i2;
                    g1.RenderTransformOrigin = new Point(0.5, 0.5);
                    g1.RenderTransform = new RotateTransform(0);

                    b2[row, col] = g1;
                    GameGrid.Children.Add(g1);
                    f1 = f1 + 1;
                }
            }

            a2.a17();
        }

        private void i2(object j1, RoutedEventArgs j2)
        {
            if (calcMode == true)
            {
                return;
            }

            Button k1 = j1 as Button;
            if (k1 == null)
            {
                return;
            }

            var (l1, l2) = ((int, int))k1.Tag;

            if (l1 == 0 && l2 == 0)
            {
                e2();
                return;
            }

            a2.a22(l1, l2);

            if (a2.a14 == true)
            {
                m1();
            }
            else
            {
                if (a2.a15 == true)
                {
                }
            }
        }

        private void i1(object j1, MouseButtonEventArgs j2)
        {
            if (calcMode == true)
            {
                return;
            }

            Button k1 = j1 as Button;
            if (k1 == null)
            {
                return;
            }

            var (l1, l2) = ((int, int))k1.Tag;
            a2.a23(l1, l2);
            j2.Handled = true;
        }

        private void e1()
        {
            if (calcMode == true)
            {
                return;
            }

            if (b2 == null)
            {
                return;
            }

            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            for (f2 = 0; f2 < 5; f2 = f2 + 1)
            {
                for (f3 = 0; f3 < 4; f3 = f3 + 1)
                {
                    if (f2 == 4 && f3 == 1)
                    {
                        f1 = f1 + 1;
                        continue;
                    }

                    Button g1 = b2[f2, f3];
                    if (g1 == null)
                    {
                        continue;
                    }

                    string g2 = "";
                    if (f1 < d1.Length)
                    {
                        g2 = d1[f1];
                    }
                    else
                    {
                        g2 = "";
                    }

                    if (f2 == 4 && f3 == 0)
                    {
                        g2 = "0";
                    }

                    bool h1 = false;
                    if (f3 == 3 && g2 != "")
                    {
                        h1 = true;
                    }

                    bool h2 = false;
                    if (f2 == 0 && f3 < 3)
                    {
                        h2 = true;
                    }

                    bool h3 = false;
                    if (g2 == "=")
                    {
                        h3 = true;
                    }

                    bool h4 = false;
                    if (h1 == true || h2 == true || h3 == true)
                    {
                        h4 = true;
                    }
                    else
                    {
                        h4 = false;
                    }

                    if (a2.a21(f2, f3) == true)
                    {
                        g1.Content = "±";
                        g1.Background = new SolidColorBrush(Color.FromRgb(255, 200, 100));
                        g1.Foreground = new SolidColorBrush(Colors.Black);
                        g1.IsEnabled = true;
                    }
                    else
                    {
                        if (a2.a20(f2, f3) == true)
                        {
                            int n1 = a2.a19(f2, f3);
                            if (n1 == -1)
                            {
                                g1.Content = "E";
                                g1.Background = new SolidColorBrush(Colors.DarkRed);
                                g1.Foreground = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                if (n1 == 0)
                                {
                                    g1.Content = g2;
                                    if (h4 == true)
                                    {
                                        g1.Background = new SolidColorBrush(Color.FromRgb(255, 200, 150));
                                    }
                                    else
                                    {
                                        g1.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                                    }
                                    g1.Foreground = new SolidColorBrush(Colors.White);
                                }
                                else
                                {
                                    if (h4 == true)
                                    {
                                        g1.Content = g2;
                                        g1.Background = new SolidColorBrush(Color.FromRgb(255, 200, 150));
                                        g1.Foreground = new SolidColorBrush(Colors.White);
                                    }
                                    else
                                    {
                                        g1.Content = n1.ToString();
                                        g1.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                                        g1.Foreground = o1(n1);
                                    }
                                }
                            }
                            g1.IsEnabled = false;
                        }
                        else
                        {
                            g1.Content = g2;
                            if (h1 == true || h3 == true)
                            {
                                g1.Background = new SolidColorBrush(Color.FromRgb(255, 149, 0));
                                g1.Foreground = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                if (h2 == true)
                                {
                                    g1.Background = new SolidColorBrush(Color.FromRgb(166, 166, 166));
                                    g1.Foreground = new SolidColorBrush(Colors.Black);
                                }
                                else
                                {
                                    g1.Background = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                                    g1.Foreground = new SolidColorBrush(Colors.White);
                                }
                            }
                            g1.IsEnabled = true;
                        }
                    }
                    f1 = f1 + 1;
                }
            }

            int remaining = a2.a24();
            string text = remaining.ToString();
            DisplayLabel.Text = text;

            if (a2.a14 == true)
            {
                DisplayLabel.Text = "ERROR";
                DisplayLabel.Foreground = new SolidColorBrush(Colors.Red);
                ResetButton.Visibility = Visibility.Visible;
                p1();
            }
            else
            {
                if (a2.a15 == true)
                {
                    DisplayLabel.Text = "WIN";
                    DisplayLabel.Foreground = new SolidColorBrush(Colors.Green);
                    ResetButton.Visibility = Visibility.Visible;
                }
                else
                {
                    DisplayLabel.Foreground = new SolidColorBrush(Colors.White);
                    ResetButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private Brush o1(int q1)
        {
            if (q1 == 1)
            {
                return new SolidColorBrush(Colors.Blue);
            }
            else if (q1 == 2)
            {
                return new SolidColorBrush(Colors.Green);
            }
            else if (q1 == 3)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (q1 == 4)
            {
                return new SolidColorBrush(Colors.DarkBlue);
            }
            else if (q1 == 5)
            {
                return new SolidColorBrush(Colors.DarkRed);
            }
            else if (q1 == 6)
            {
                return new SolidColorBrush(Colors.Teal);
            }
            else if (q1 == 7)
            {
                return new SolidColorBrush(Colors.Black);
            }
            else if (q1 == 8)
            {
                return new SolidColorBrush(Colors.Gray);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        private void m1()
        {
            int r1 = 0;
            int r2 = 0;
            for (r1 = 0; r1 < 5; r1 = r1 + 1)
            {
                for (r2 = 0; r2 < 4; r2 = r2 + 1)
                {
                    if (b2[r1, r2] != null)
                    {
                        int value = a2.a19(r1, r2);
                        if (value == -1)
                        {
                            Button s1 = b2[r1, r2];
                            s1.Content = "E";
                            s1.Background = new SolidColorBrush(Colors.DarkRed);
                            s1.Foreground = new SolidColorBrush(Colors.White);
                            s1.IsEnabled = false;
                        }
                    }
                }
            }
        }

        private void NewGameButton_Click(object t1, RoutedEventArgs t2)
        {
            e2();
        }

        private void ResetButton_Click(object t1, RoutedEventArgs t2)
        {
            u1();
            e2();
            ResetButton.Visibility = Visibility.Collapsed;
        }

        private void MinimizeButton_Click(object t1, RoutedEventArgs t2)
        {
            this.Width = c3;
            this.Height = c4;

            RotateTransform v1 = this.FindName("WindowRotateTransform") as RotateTransform;
            if (v1 == null)
            {
                v1 = this.RenderTransform as RotateTransform;
            }

            if (v1 != null)
            {
                DoubleAnimation v2 = new DoubleAnimation();
                v2.From = 0;
                v2.To = 90;
                v2.Duration = new Duration(TimeSpan.FromSeconds(0.7));
                v2.EasingFunction = new CubicEase();
                v2.EasingFunction.EasingMode = EasingMode.EaseInOut;
                v1.BeginAnimation(RotateTransform.AngleProperty, v2);
            }

            w1();

            System.Windows.Threading.DispatcherTimer v3 = new System.Windows.Threading.DispatcherTimer();
            v3.Interval = TimeSpan.FromSeconds(0.8);
            v3.Tick += (s, args) =>
            {
                v3.Stop();
                x1();
            };
            v3.Start();
        }

        private void w1()
        {
            if (b2 == null)
            {
                return;
            }

            int y1 = 0;
            int y2 = 0;
            for (y1 = 0; y1 < 5; y1 = y1 + 1)
            {
                for (y2 = 0; y2 < 4; y2 = y2 + 1)
                {
                    if (b2[y1, y2] != null)
                    {
                        Button z1 = b2[y1, y2];
                        DoubleAnimation z2 = new DoubleAnimation();
                        z2.From = 1.0;
                        z2.To = 0.0;
                        z2.Duration = new Duration(TimeSpan.FromSeconds(0.7));
                        z2.EasingFunction = new CubicEase();
                        z2.EasingFunction.EasingMode = EasingMode.EaseOut;
                        z1.BeginAnimation(UIElement.OpacityProperty, z2);
                    }
                }
            }

            DoubleAnimation anim1 = new DoubleAnimation();
            anim1.From = 1.0;
            anim1.To = 0.0;
            anim1.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            MinimizeButton.BeginAnimation(UIElement.OpacityProperty, anim1);

            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.From = 1.0;
            anim2.To = 0.0;
            anim2.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            ExpandButton.BeginAnimation(UIElement.OpacityProperty, anim2);

            if (ResetButton.Visibility == Visibility.Visible)
            {
                DoubleAnimation anim3 = new DoubleAnimation();
                anim3.From = 1.0;
                anim3.To = 0.0;
                anim3.Duration = new Duration(TimeSpan.FromSeconds(0.7));
                ResetButton.BeginAnimation(UIElement.OpacityProperty, anim3);
            }

            DoubleAnimation anim4 = new DoubleAnimation();
            anim4.From = 1.0;
            anim4.To = 0.0;
            anim4.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            DisplayLabel.BeginAnimation(UIElement.OpacityProperty, anim4);
        }

        private void x1()
        {
            OopsLabel.Visibility = Visibility.Visible;

            DoubleAnimation aa1 = new DoubleAnimation();
            aa1.From = 0.0;
            aa1.To = 1.0;
            aa1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            aa1.EasingFunction = new CubicEase();
            aa1.EasingFunction.EasingMode = EasingMode.EaseIn;
            OopsLabel.BeginAnimation(UIElement.OpacityProperty, aa1);

            System.Windows.Threading.DispatcherTimer aa2 = new System.Windows.Threading.DispatcherTimer();
            aa2.Interval = TimeSpan.FromSeconds(2.5);
            aa2.Tick += (s, args) =>
            {
                aa2.Stop();
                bb1();
            };
            aa2.Start();
        }

        private void bb1()
        {
            OopsLabel.Visibility = Visibility.Collapsed;
            OopsLabel.Opacity = 0;

            RotateTransform cc1 = this.FindName("WindowRotateTransform") as RotateTransform;
            if (cc1 == null)
            {
                cc1 = this.RenderTransform as RotateTransform;
            }

            if (cc1 != null)
            {
                DoubleAnimation cc2 = new DoubleAnimation();
                cc2.From = 90;
                cc2.To = 0;
                cc2.Duration = new Duration(TimeSpan.FromSeconds(0.7));
                cc2.EasingFunction = new CubicEase();
                cc2.EasingFunction.EasingMode = EasingMode.EaseInOut;
                cc1.BeginAnimation(RotateTransform.AngleProperty, cc2);
            }

            if (b3 == false)
            {
                this.Width = c1;
                this.Height = c2;
            }

            if (b2 != null)
            {
                int dd1 = 0;
                int dd2 = 0;
                for (dd1 = 0; dd1 < 5; dd1 = dd1 + 1)
                {
                    for (dd2 = 0; dd2 < 4; dd2 = dd2 + 1)
                    {
                        if (b2[dd1, dd2] != null)
                        {
                            b2[dd1, dd2].Opacity = 1.0;
                            b2[dd1, dd2].BeginAnimation(UIElement.OpacityProperty, null);
                        }
                    }
                }
            }

            MinimizeButton.Opacity = 1.0;
            MinimizeButton.BeginAnimation(UIElement.OpacityProperty, null);
            ExpandButton.Opacity = 1.0;
            ExpandButton.BeginAnimation(UIElement.OpacityProperty, null);
            if (ResetButton.Visibility == Visibility.Visible)
            {
                ResetButton.Opacity = 1.0;
                ResetButton.BeginAnimation(UIElement.OpacityProperty, null);
            }
            DisplayLabel.Opacity = 1.0;
            DisplayLabel.BeginAnimation(UIElement.OpacityProperty, null);
        }

        private void ExpandButton_Click(object t1, RoutedEventArgs t2)
        {
            if (b3 == true)
            {
                this.Width = c1;
                this.Height = c2;
                ExpandButton.Content = "⛶";
                ExpandButton.ToolTip = "Расширить окно";
                b3 = false;
                ee1(0, c5);
            }
            else
            {
                this.Width = c3;
                this.Height = c4;
                ExpandButton.Content = "⊟";
                ExpandButton.ToolTip = "Свернуть окно";
                b3 = true;
                ee1(180, c6);
            }
        }

        private void ee1(double ff1, int ff2)
        {
            if (b2 == null)
            {
                return;
            }

            int gg1 = 0;
            int gg2 = 0;
            for (gg1 = 0; gg1 < 5; gg1 = gg1 + 1)
            {
                for (gg2 = 0; gg2 < 4; gg2 = gg2 + 1)
                {
                    if (b2[gg1, gg2] != null)
                    {
                        Button hh1 = b2[gg1, gg2];
                        RotateTransform hh2 = hh1.RenderTransform as RotateTransform;
                        if (hh2 == null)
                        {
                            hh2 = new RotateTransform();
                            hh1.RenderTransformOrigin = new Point(0.5, 0.5);
                            hh1.RenderTransform = hh2;
                        }

                        DoubleAnimation ii1 = new DoubleAnimation();
                        ii1.From = hh2.Angle;
                        ii1.To = ff1;
                        ii1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                        ii1.EasingFunction = new CubicEase();
                        ii1.EasingFunction.EasingMode = EasingMode.EaseInOut;
                        hh2.BeginAnimation(RotateTransform.AngleProperty, ii1);

                        double currentWidth = hh1.ActualWidth;
                        double targetWidth = 0;
                        if (gg1 == 4 && gg2 == 0)
                        {
                            targetWidth = ff2 * 2 + 8;
                        }
                        else
                        {
                            targetWidth = ff2;
                        }
                        if (currentWidth <= 0)
                        {
                            currentWidth = c5;
                        }

                        DoubleAnimation ii2 = new DoubleAnimation();
                        ii2.From = currentWidth;
                        ii2.To = targetWidth;
                        ii2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                        ii2.EasingFunction = new CubicEase();
                        ii2.EasingFunction.EasingMode = EasingMode.EaseInOut;
                        hh1.BeginAnimation(Button.WidthProperty, ii2);

                        double currentHeight = hh1.ActualHeight;
                        double targetHeight = ff2;
                        if (currentHeight <= 0)
                        {
                            currentHeight = c5;
                        }

                        DoubleAnimation ii3 = new DoubleAnimation();
                        ii3.From = currentHeight;
                        ii3.To = targetHeight;
                        ii3.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                        ii3.EasingFunction = new CubicEase();
                        ii3.EasingFunction.EasingMode = EasingMode.EaseInOut;
                        hh1.BeginAnimation(Button.HeightProperty, ii3);

                        if (ff2 == c5)
                        {
                            hh1.FontSize = 28;
                        }
                        else
                        {
                            hh1.FontSize = 38;
                        }
                    }
                }
            }
        }

        private void p1()
        {
            if (b2 == null)
            {
                return;
            }

            Random jj1 = new Random();
            int kk1 = 0;
            int kk2 = 0;
            for (kk1 = 0; kk1 < 5; kk1 = kk1 + 1)
            {
                for (kk2 = 0; kk2 < 4; kk2 = kk2 + 1)
                {
                    if (b2[kk1, kk2] != null)
                    {
                        Button ll1 = b2[kk1, kk2];
                        TransformGroup ll2 = ll1.RenderTransform as TransformGroup;
                        if (ll2 == null)
                        {
                            ll2 = new TransformGroup();
                            if (ll1.RenderTransform != null)
                            {
                                if (ll1.RenderTransform is RotateTransform)
                                {
                                    RotateTransform mm1 = ll1.RenderTransform as RotateTransform;
                                    ll2.Children.Add(mm1);
                                }
                            }
                            ll1.RenderTransform = ll2;
                        }

                        TranslateTransform mm2 = ll2.Children.OfType<TranslateTransform>().FirstOrDefault();
                        if (mm2 == null)
                        {
                            mm2 = new TranslateTransform();
                            ll2.Children.Add(mm2);
                        }

                        double nn1 = jj1.NextDouble() * 2.0 * Math.PI;
                        double nn2 = 300.0 + jj1.NextDouble() * 400.0;
                        double nn3 = Math.Cos(nn1) * nn2;
                        double nn4 = Math.Sin(nn1) * nn2;

                        DoubleAnimation oo1 = new DoubleAnimation();
                        oo1.From = 0;
                        oo1.To = nn3;
                        double duration1 = 0.8 + jj1.NextDouble() * 0.4;
                        oo1.Duration = new Duration(TimeSpan.FromSeconds(duration1));
                        oo1.EasingFunction = new CubicEase();
                        oo1.EasingFunction.EasingMode = EasingMode.EaseOut;

                        DoubleAnimation oo2 = new DoubleAnimation();
                        oo2.From = 0;
                        oo2.To = nn4;
                        double duration2 = 0.8 + jj1.NextDouble() * 0.4;
                        oo2.Duration = new Duration(TimeSpan.FromSeconds(duration2));
                        oo2.EasingFunction = new CubicEase();
                        oo2.EasingFunction.EasingMode = EasingMode.EaseOut;

                        RotateTransform pp1 = ll2.Children.OfType<RotateTransform>().FirstOrDefault();
                        if (pp1 == null)
                        {
                            pp1 = new RotateTransform();
                            ll2.Children.Add(pp1);
                        }

                        double pp2 = (jj1.NextDouble() - 0.5) * 720.0;
                        DoubleAnimation qq1 = new DoubleAnimation();
                        qq1.From = pp1.Angle;
                        qq1.To = pp1.Angle + pp2;
                        double duration3 = 0.8 + jj1.NextDouble() * 0.4;
                        qq1.Duration = new Duration(TimeSpan.FromSeconds(duration3));
                        qq1.EasingFunction = new CubicEase();
                        qq1.EasingFunction.EasingMode = EasingMode.EaseOut;
                        pp1.BeginAnimation(RotateTransform.AngleProperty, qq1);

                        DoubleAnimation qq2 = new DoubleAnimation();
                        qq2.From = 1.0;
                        qq2.To = 0.0;
                        double duration4 = 0.6 + jj1.NextDouble() * 0.3;
                        qq2.Duration = new Duration(TimeSpan.FromSeconds(duration4));
                        qq2.EasingFunction = new CubicEase();
                        qq2.EasingFunction.EasingMode = EasingMode.EaseOut;
                        ll1.BeginAnimation(UIElement.OpacityProperty, qq2);

                        mm2.BeginAnimation(TranslateTransform.XProperty, oo1);
                        mm2.BeginAnimation(TranslateTransform.YProperty, oo2);
                    }
                }
            }
        }

        private void u1()
        {
            if (b2 == null)
            {
                return;
            }

            int rr1 = 0;
            int rr2 = 0;
            for (rr1 = 0; rr1 < 5; rr1 = rr1 + 1)
            {
                for (rr2 = 0; rr2 < 4; rr2 = rr2 + 1)
                {
                    if (b2[rr1, rr2] != null)
                    {
                        Button ss1 = b2[rr1, rr2];
                        if (ss1.RenderTransform is TransformGroup)
                        {
                            TransformGroup tt1 = ss1.RenderTransform as TransformGroup;
                            TranslateTransform tt2 = tt1.Children.OfType<TranslateTransform>().FirstOrDefault();
                            if (tt2 != null)
                            {
                                tt2.BeginAnimation(TranslateTransform.XProperty, null);
                                tt2.BeginAnimation(TranslateTransform.YProperty, null);
                                tt2.X = 0;
                                tt2.Y = 0;
                            }

                            RotateTransform uu1 = tt1.Children.OfType<RotateTransform>().FirstOrDefault();
                            if (uu1 != null)
                            {
                                double uu2 = uu1.Angle;
                                uu1.BeginAnimation(RotateTransform.AngleProperty, null);
                                double targetAngle = 0;
                                if (b3 == true)
                                {
                                    targetAngle = 180;
                                }
                                else
                                {
                                    targetAngle = 0;
                                }

                                DoubleAnimation vv1 = new DoubleAnimation();
                                vv1.From = uu2;
                                vv1.To = targetAngle;
                                vv1.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                                vv1.EasingFunction = new CubicEase();
                                vv1.EasingFunction.EasingMode = EasingMode.EaseInOut;
                                uu1.BeginAnimation(RotateTransform.AngleProperty, vv1);
                            }
                        }

                        ss1.Opacity = 1.0;
                        ss1.BeginAnimation(UIElement.OpacityProperty, null);
                    }
                }
            }
        }
    }
}
