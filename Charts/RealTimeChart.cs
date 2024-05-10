﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.RightsManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CTP;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace CTP.Charts;

public partial class RealTimeChart : ObservableObject
{
    private readonly List<DateTimePoint> _values = new();
    /*private readonly List<DateTimePoint> _velocityValues = new();
    private readonly List<DateTimePoint> _accelerationValues = new();*/
    private readonly DateTimeAxis _customAxis;

    public int VisibleElements { get; set; } = 350;

    public RealTimeChart()
    {
        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _values,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
            }

        };

        /*VelocitySeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _velocityValues,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
            }
        };*/

        /*AccelerationSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _accelerationValues,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
            }
        };*/

        _customAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
        {
            CustomSeparators = GetSeparators(),
            AnimationsSpeed = TimeSpan.FromMilliseconds(0),
            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100)),
        };

        YAxesDistance = new Axis[]
        {
            new Axis
            {
                Name = "x [mm]",
                TextSize = 16
            }
        };

        YAxesVelocity = new Axis[]
        {
            new Axis
            {
                Name = "v [mm/s]",
                TextSize = 16
            }
        };

        YAxesAcceleration = new Axis[]
        {
            new Axis
            {
                Name = "a [mm/s^2]",
                TextSize = 16
            }
        };

        XAxes = new Axis[] { _customAxis };

        _ = ReadData();
    }

    public ObservableCollection<ISeries> Series { get; set; }

    public Axis[] XAxes { get; set; }

    public Axis[] YAxesDistance { get; set; }

    public Axis[] YAxesVelocity { get; set; }

    public Axis[] YAxesAcceleration { get; set; }

    public object Sync { get; } = new object();

    public bool IsReading { get; set; } = true;

    public List<float> AllValues { get; set; } = new();
    private int i = 0;

    private async Task ReadData()
    {
        while (IsReading)
        {
            await Task.Delay(100);

            if (AllValues.Count == 0) continue;

            if (i >= AllValues.Count) i = 0;

            lock (Sync)
            {
                _values.Add(new DateTimePoint(DateTime.Now, AllValues[i]));
                if (_values.Count > VisibleElements) _values.RemoveAt(0);

                _customAxis.CustomSeparators = GetSeparators();
            }
            i++;
        }
    }

    private static double[] GetSeparators()
    {
        var now = DateTime.Now;

        return new double[]
        {
            now.AddSeconds(-30).Ticks,
            now.AddSeconds(-20).Ticks,
            now.AddSeconds(-10).Ticks,
            now.Ticks
        };
    }

    private static string Formatter(DateTime date)
    {
        var secsAgo = (DateTime.Now - date).TotalSeconds;

        return secsAgo < 1
            ? "teraz"
            : $"{secsAgo:N0} sekund temu";
    }
}