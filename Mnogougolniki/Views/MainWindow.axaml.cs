using Avalonia.Controls;
using Avalonia.Input;

namespace Mnogougolniki.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Figures.ItemsSource = new[] {"Circle", "Triangle", "Square"};
        Figures.SelectedIndex = 1;
        
    }
    
    private void OnPointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        Controls? Control = this.Find<Controls>("MyCustomControls");
        Control?.Release((int)e.GetPosition(Control).X, (int)e.GetPosition(Control).Y);
    }
    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        Controls? Control = this.Find<Controls>("MyCustomControls");
        Control?.Move((int)e.GetPosition(Control).X, (int)e.GetPosition(Control).Y);
    }
    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        Controls? Control = this.Find<Controls>("MyCustomControls");
        var point = e.GetCurrentPoint(sender as Control);
           

        if (point.Properties.IsRightButtonPressed)
        {
            Control?.RightClick((int)e.GetPosition(Control).X, (int)e.GetPosition(Control).Y);
        }
        if (point.Properties.IsLeftButtonPressed)
        {
            Control?.LeftClick((int)e.GetPosition(Control).X, (int)e.GetPosition(Control).Y, point);
        }
    }
    private void Figures_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Controls? Control = this.Find<Controls>("MyCustomControls");

        int type = Figures.SelectedIndex;
        Control?.ChangeType(type);
    }
}
