using System;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Diagnostics;
namespace Mnogougolniki;

public class ChartControl : UserControl
{
    private Tuple<int, double>[]? _jarvisChart;
    private Tuple<int, double>[]? _byDefChart;
    private bool _isChart;
    private int _chartToDraw = 3;
    private int _scale;
    public void SetArrays(Tuple<int, double>[]? jarv, Tuple<int, double>[]? def, int type)
    {
        _jarvisChart = jarv;
        _byDefChart = def;
        _isChart = true;
        _chartToDraw = type;
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        Brush lineBrush = new SolidColorBrush(Colors.Blue);
        Pen pen = new(lineBrush, lineCap: PenLineCap.Square);
        int oy = 550;
        int ox = 20;
        context.DrawLine(pen, new Point(ox, oy), new Point(oy, 550));
        context.DrawLine(pen, new Point(ox, oy), new Point(ox, 30));

        if (_isChart)
        {
            switch (_chartToDraw)
            {
                case 1:
                    _scale = 1;
                    DrawChart(context, _byDefChart, Colors.Chartreuse);
                    break;
                case 2:
                    _scale = 1000;
                    DrawChart(context, _jarvisChart, Colors.Fuchsia);
                    break;
                case 3:
                    _scale = 1;
                    DrawChart(context, _jarvisChart, Colors.Fuchsia);
                    DrawChart(context, _byDefChart, Colors.Chartreuse);
                    break;
            }
        }
    }

    private void DrawChart(DrawingContext context, Tuple<int, double>[]? chart, Color color)
    {
        Brush lineBrush = new SolidColorBrush(color);
        Pen pen = new(lineBrush, lineCap: PenLineCap.Square);
        for (int i = 1; i < chart?.Length; ++i)
        {
            var p1 = new Point(chart[i - 1].Item1 + 20, 550 - chart[i - 1].Item2 * _scale);
            var p2 = new Point(chart[i].Item1 + 20, 550 - chart[i].Item2 * _scale);
            context.DrawLine(pen, p1, p2);
        }
    }
}