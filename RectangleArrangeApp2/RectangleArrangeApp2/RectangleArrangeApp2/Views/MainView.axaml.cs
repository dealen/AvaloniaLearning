using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RectangleArrangeApp2.Views
{
    public partial class MainView : UserControl
    {
        private List<Rectangle> rectangles = new List<Rectangle>();
        private Rectangle selectedRectangle;
        private Point lastPosition;



        public MainView()
        {
            InitializeComponent();
        }

        public Canvas? Canvas
        {
            get
            {
                var canvas = this.FindControl<Canvas>("WorkingBoard");

                if (canvas == null)
                {
                    return null;
                }

                return canvas;
            }
        }

        private void AddRectangle(double left, double top, double width, double height, IBrush fill)
        {
            var rect = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = fill,
            };

            rect.Name = Guid.NewGuid().ToString();

            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);

            rect.PointerPressed += Rectangle_PointerPressed;
            rect.PointerReleased += Rectangle_PointerReleased;
            rect.PointerMoved += Rectangle_PointerMoved;

            // Add context menu for resizing
            rect.ContextMenu = new ContextMenu
            {
                ItemsSource = new[]
                {
                    new MenuItem
                    {
                        Header = "Resize",
                        Command = ReactiveCommand.Create(() => ShowResizeDialog(rect))
                    },
                    new MenuItem
                    {
                        Header = "Remove",
                        Command = ReactiveCommand.Create(() => RemoveRectangle(rect))
                    },
                    new MenuItem
                    {
                        Header = "Rotate 90 degrees",
                        Command = ReactiveCommand.Create(() =>
                        {
                            rect.RenderTransform = new RotateTransform(90);
                            rect.RenderTransformOrigin = RelativePoint.Center;
                        })
                    }
                }
            };

            rectangles.Add(rect);
            Canvas?.Children.Add(rect);
        }

        private void Rectangle_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                selectedRectangle = rect;
                lastPosition = e.GetPosition(Canvas);
                rect.Opacity = 0.7; // Change opacity for visual effect
                rect.RenderTransform = new ScaleTransform(1.05, 1.05); // Slightly enlarge
                rect.RenderTransformOrigin = RelativePoint.Center;
                e.Pointer.Capture(rect); // Corrected line
                e.Handled = true;
            }
        }

        private void Rectangle_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (selectedRectangle != null)
            {
                //selectedRectangle.ReleasePointerCapture(e.Pointer);
                e.Pointer.Capture(null);
                selectedRectangle.Opacity = 1.0; // Reset opacity
                selectedRectangle.RenderTransform = null; // Reset transformation
                selectedRectangle = null;
                e.Handled = true;
            }
        }

        private void Rectangle_PointerMoved(object sender, PointerEventArgs e)
        {
            if (selectedRectangle != null && e.GetCurrentPoint(Canvas).Properties.IsLeftButtonPressed)
            {
                var position = e.GetPosition(Canvas);
                var offsetX = position.X - lastPosition.X;
                var offsetY = position.Y - lastPosition.Y;

                double left = Canvas.GetLeft(selectedRectangle) + offsetX;
                double top = Canvas.GetTop(selectedRectangle) + offsetY;

                // Ensure the rectangle stays within the bounds
                left = Math.Max(0, Math.Min(left, Canvas.Bounds.Width - selectedRectangle.Bounds.Width));
                top = Math.Max(0, Math.Min(top, Canvas.Bounds.Height - selectedRectangle.Bounds.Height));

                Canvas.SetLeft(selectedRectangle, left);
                Canvas.SetTop(selectedRectangle, top);

                lastPosition = position;
                e.Handled = true;
            }
        }

        private async void ShowResizeDialog(Rectangle rect)
        {
            MessageBus.Current.SendMessage("Not implemented yet.");
            //var dialog = new Window
            //{
            //    Title = "Resize Rectangle",
            //    Width = 300,
            //    Height = 200,
            //    Content = new StackPanel
            //    {
            //        Margin = new Thickness(10),
            //        Children =
            //        {
            //            new TextBlock { Text = "Width:" },
            //            new TextBox { Name = "WidthBox", Text = rect.Width.ToString() },
            //            new TextBlock { Text = "Height:" },
            //            new TextBox { Name = "HeightBox", Text = rect.Height.ToString() },
            //            new Button
            //            {
            //                Content = "OK",
            //                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            //                Command = ReactiveCommand.Create(() =>
            //                {
            //                    if (double.TryParse(((TextBox)this.FindControl<TextBox>("WidthBox")).Text, out double newWidth))
            //                    {
            //                        rect.Width = newWidth;
            //                    }
            //                    if (double.TryParse(((TextBox)this.FindControl<TextBox>("HeightBox")).Text, out double newHeight))
            //                    {
            //                        rect.Height = newHeight;
            //                    }
            //                    this.Close();
            //                })
            //            }
            //        }
            //    }
            //};

            //await Dispatcher.UIThread.InvokeAsync(async () =>
            //{
            //    await dialog.ShowDialog(this);
            //});
        }

        private async void RemoveRectangle(Rectangle rect)
        {
            //Rectangle? element = this.FindControl<Rectangle>(rect.Name);
            //if (element is null)
            //    return;

            var foundELememnt = rectangles.Find(x => x.Name == rect.Name);
            if (!(foundELememnt is null))
            {
                rectangles.Remove(foundELememnt);
                Canvas?.Children.Remove(foundELememnt as Control);
            }

            await Task.CompletedTask;
        }

        public void AddRectangleClick(object sender, RoutedEventArgs args)
        {
            if (string.IsNullOrEmpty(txtHeight.Text) || string.IsNullOrEmpty(txtWidth.Text))
            {
                MessageBus.Current.SendMessage("Please enter a valid height and width.");
                return;
            }

            if (!double.TryParse(txtHeight.Text, out double height) || !double.TryParse(txtWidth.Text, out double width))
            {
                MessageBus.Current.SendMessage("Please enter a valid height and width.");
                return;
            }

            var color = Brushes.Blue;

            var listOfColors = new List<IImmutableSolidColorBrush>
            {
                Brushes.Green,
                Brushes.Yellow,
                Brushes.Orange,
                Brushes.Red,
                Brushes.Purple,
                Brushes.BlueViolet,
                Brushes.Blue,
                Brushes.Navy,
                Brushes.Brown,
                Brushes.Gray,
                Brushes.Black
            };

            var random = new Random();

            AddRectangle(50, 50, width, height, listOfColors[random.Next(listOfColors.Count)]);
        }

    }
}