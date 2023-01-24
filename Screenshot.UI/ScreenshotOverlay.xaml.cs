﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SelectArea.Helpers;
using SelectArea.Utilities;

// using Windows.Globalization;
// using Windows.Media.Ocr;

namespace SelectArea;

public partial class ScreenshotOverlay : Window
{
    private bool isShiftDown;
    private Point clickedPoint;
    private Point shiftPoint;
    private System.Drawing.Rectangle regionScaled;
    private bool IsSelecting { get; set; }

    private Border selectBorder = new Border();

    private DpiScale? dpiScale;

    private Point GetMousePos() => PointToScreen(Mouse.GetPosition(this));

    // private Language? selectedLanguage = null;
    private MenuItem cancelMenuItem;

    private System.Windows.Forms.Screen? CurrentScreen
    {
        get;
        set;
    }

    private double selectLeft;
    private double selectTop;

    private double xShiftDelta;
    private double yShiftDelta;

    private const double ActiveOpacity = 0.4;
    
    public ScreenshotOverlay()
    {
        InitializeComponent();
    }
    
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Maximized;
        FullWindow.Rect = new Rect(0, 0, Width, Height);
        // KeyDown += MainWindow_KeyDown;
        // KeyUp += MainWindow_KeyUp;

        BackgroundImage.Source = ImageMethods.GetWindowBoundsImage(this);
        BackgroundBrush.Opacity = ActiveOpacity;
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
        BackgroundImage.Source = null;
        BackgroundImage.UpdateLayout();

        CurrentScreen = null;
        dpiScale = null;

        // KeyDown -= MainWindow_KeyDown;
        // KeyUp -= MainWindow_KeyUp;

        Loaded -= Window_Loaded;
        Unloaded -= Window_Unloaded;

        RegionClickCanvas.MouseDown -= RegionClickCanvas_MouseDown;
        RegionClickCanvas.MouseUp -= RegionClickCanvas_MouseUp;
        RegionClickCanvas.MouseMove -= RegionClickCanvas_MouseMove;

        // cancelMenuItem.Click -= CancelMenuItem_Click;
    }
    
    private void RegionClickCanvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        RegionClickCanvas.CaptureMouse();

        CursorClipper.ClipCursor(this);
        clickedPoint = e.GetPosition(this);
        selectBorder.Height = 1;
        selectBorder.Width = 1;

        dpiScale = VisualTreeHelper.GetDpi(this);

        try
        {
            RegionClickCanvas.Children.Remove(selectBorder);
        }
        catch (Exception)
        {
        }

        selectBorder.BorderThickness = new Thickness(2);
        Color borderColor = Color.FromArgb(255, 40, 118, 126);
        selectBorder.BorderBrush = new SolidColorBrush(borderColor);
        _ = RegionClickCanvas.Children.Add(selectBorder);
        Canvas.SetLeft(selectBorder, clickedPoint.X);
        Canvas.SetTop(selectBorder, clickedPoint.Y);

        var screens = System.Windows.Forms.Screen.AllScreens;
        System.Drawing.Point formsPoint = new System.Drawing.Point((int)clickedPoint.X, (int)clickedPoint.Y);
        foreach (var scr in screens)
        {
            if (scr.Bounds.Contains(formsPoint))
            {
                CurrentScreen = scr;
                break;
            }
        }

        IsSelecting = true;
    }

    private void RegionClickCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (!IsSelecting)
        {
            return;
        }

        Point movingPoint = e.GetPosition(this);

        if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Shift)
        {
            if (!isShiftDown)
            {
                shiftPoint = movingPoint;
                selectLeft = Canvas.GetLeft(selectBorder);
                selectTop = Canvas.GetTop(selectBorder);
            }

            isShiftDown = true;
            xShiftDelta = movingPoint.X - shiftPoint.X;
            yShiftDelta = movingPoint.Y - shiftPoint.Y;

            double leftValue = selectLeft + xShiftDelta;
            double topValue = selectTop + yShiftDelta;

            if (CurrentScreen is not null && dpiScale is not null)
            {
                double currentScreenLeft = CurrentScreen.Bounds.Left; // Should always be 0
                double currentScreenRight = CurrentScreen.Bounds.Right / dpiScale.Value.DpiScaleX;
                double currentScreenTop = CurrentScreen.Bounds.Top; // Should always be 0
                double currentScreenBottom = CurrentScreen.Bounds.Bottom / dpiScale.Value.DpiScaleY;

                // this is giving issues on different monitors
                // leftValue = Math.Clamp(leftValue, currentScreenLeft, currentScreenRight - selectBorder.Width);
                // topValue = Math.Clamp(topValue, currentScreenTop, currentScreenBottom - selectBorder.Height);
            }

            clippingGeometry.Rect = new Rect(
                new Point(leftValue, topValue),
                new Size(selectBorder.Width, selectBorder.Height));
            Canvas.SetLeft(selectBorder, leftValue - 1);
            Canvas.SetTop(selectBorder, topValue - 1);
            return;
        }

        isShiftDown = false;

        double left = Math.Min(clickedPoint.X, movingPoint.X);
        double top = Math.Min(clickedPoint.Y, movingPoint.Y);

        selectBorder.Height = Math.Max(clickedPoint.Y, movingPoint.Y) - top;
        selectBorder.Width = Math.Max(clickedPoint.X, movingPoint.X) - left;
        selectBorder.Height += 2;
        selectBorder.Width += 2;

        clippingGeometry.Rect = new Rect(
            new Point(left, top),
            new Size(selectBorder.Width - 2, selectBorder.Height - 2));
        Canvas.SetLeft(selectBorder, left - 1);
        Canvas.SetTop(selectBorder, top - 1);

        ActionsCanvas.SetValue(Canvas.LeftProperty, clippingGeometry.Rect.Right);
        ActionsCanvas.SetValue(Canvas.TopProperty, clippingGeometry.Rect.Bottom);
    }

    private async void RegionClickCanvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (IsSelecting == false)
        {
            return;
        }

        IsSelecting = false;
        


        CurrentScreen = null;
        CursorClipper.UnClipCursor();
        RegionClickCanvas.ReleaseMouseCapture();
        Matrix m = PresentationSource.FromVisual(this)?.CompositionTarget.TransformToDevice ?? throw new Exception("Null PresentationSource");

        Point mPt = GetMousePos();
        Point movingPoint = e.GetPosition(this);
        movingPoint.X *= m.M11;
        movingPoint.Y *= m.M22;

        movingPoint.X = Math.Round(movingPoint.X);
        movingPoint.Y = Math.Round(movingPoint.Y);

        double xDimScaled = Canvas.GetLeft(selectBorder) * m.M11;
        double yDimScaled = Canvas.GetTop(selectBorder) * m.M22;

        regionScaled = new System.Drawing.Rectangle(
            (int)xDimScaled,
            (int)yDimScaled,
            (int)(selectBorder.Width * m.M11),
            (int)(selectBorder.Height * m.M22));

        // string grabbedText;

        ActionsCanvas.Visibility = Visibility.Visible;
    }

    private void SaveClipboardButton_Click(object sender, RoutedEventArgs e)
    {
        Screenshot.Lib.Screenshot.sendToClipboard(
            Screenshot.Lib.Screenshot.selectArea(
                regionScaled.X, regionScaled.Y,
                Screenshot.Lib.Screenshot.setSize(regionScaled.Width, regionScaled.Height)
            )
        );
        
        CloseOverlay();
    }

    private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {

        CloseOverlay();

        var picker = new Windows.Storage.Pickers.FolderPicker();
        picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        Windows.Storage.StorageFolder folder = await picker.PickSingleFolderAsync();
        var path = folder.Path + "\\screenshot1.jpeg";
        
        Screenshot.Lib.Screenshot.save(
            path,
            Screenshot.Lib.Screenshot.selectArea(
                regionScaled.X, regionScaled.Y,
                Screenshot.Lib.Screenshot.setSize(regionScaled.Width, regionScaled.Height)
            )
        );
        

    }

    private void CloseOverlay()
    {
        RegionClickCanvas.Children.Remove(selectBorder);
        clippingGeometry.Rect = new Rect(0, 0, 0, 0);
        WindowUtilities.CloseAllOCROverlays();
    }
}