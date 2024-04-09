using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using CommunityToolkit.Mvvm.Input;

namespace ViewModelsSamples.General.RealTime;

public partial class ViewModel : ObservableObject
{
    private readonly Random _random = new(); // do zmiany na wlasne dane
    private readonly List<DateTimePoint> _values = new List<DateTimePoint>();
    private readonly DateTimeAxis _customAxis;

    public ViewModel()
    {
        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _values,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null
            }
        };

        _customAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
        {
            CustomSeparators = GetSeparators(),
            AnimationsSpeed = TimeSpan.FromMilliseconds(0),
            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100))
        };

        XAxes = new Axis[] { _customAxis };

        StartChartCommand = new RelayCommand(ChartDisplayButton_Click);
    }

    private async void ChartDisplayButton_Click()
    {
        IsReading = !IsReading; // Making the button a toggle (although it looks quite odd when the time between stop and start is even a bit lengthy)
        _ = ReadData();  // Start continuous data generation
    }

    public ObservableCollection<ISeries> Series { get; set; }

    public Axis[] XAxes { get; set; }

    public object Sync { get; } = new object();

    public ICommand StartChartCommand { get; set; }

    public bool IsReading { get; set; }


    public async Task ReadData()
    {
        while (IsReading)
        {
            await Task.Delay(100);

            // Because we are updating the chart from a different thread 
            // we need to use a lock to access the chart data. 
            // this is not necessary if your changes are made in the UI thread. 
            lock (Sync)
            {
                _values.Add(new DateTimePoint(DateTime.Now, _random.Next(0, 10))); // do zmiany tutaj
                if (_values.Count > 250) _values.RemoveAt(0);

                // we need to update the separators every time we add a new point 
                _customAxis.CustomSeparators = GetSeparators();
            }
        }
    }

    private double[] GetSeparators()
    {
        var now = DateTime.Now;

        return new double[]
        {
            now.AddSeconds(-25).Ticks,
            now.AddSeconds(-20).Ticks,
            now.AddSeconds(-15).Ticks,
            now.AddSeconds(-10).Ticks,
            now.AddSeconds(-5).Ticks,
            now.Ticks
        };
    }

    private static string Formatter(DateTime date)
    {
        var secsAgo = (DateTime.Now - date).TotalSeconds;

        return secsAgo < 1
            ? "now"
            : $"{secsAgo:N0}s ago";
    }
    
}