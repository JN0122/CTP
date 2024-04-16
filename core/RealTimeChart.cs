﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CTP;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace CTP.core;

public partial class ViewModel : ObservableObject
{
    private readonly List<DateTimePoint> _values1 = new();
    private readonly List<DateTimePoint> _values2 = new();
    private readonly DateTimeAxis _customAxis;
    private readonly Measurements _data = Measurements.GetInstance();

    public int _visibleElements { get; set; } = 250;

    public ViewModel()
    {
        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _values1,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,

            },
            new LineSeries<DateTimePoint>
            {
                Values = _values2,
                Fill = null,
                GeometryFill = null,
                Stroke = null,
                GeometryStroke = null,
                IsHoverable = false,
                IsVisibleAtLegend = false,
            }
        };

        _customAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
        {
            CustomSeparators = GetSeparators(),
            AnimationsSpeed = TimeSpan.FromMilliseconds(0),
            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100))
        };

        XAxes = new Axis[] { _customAxis };

        _ = ReadData();
    }

    public ObservableCollection<ISeries> Series { get; set; }

    public Axis[] XAxes { get; set; }

    public object Sync { get; } = new object();

    public bool IsReading { get; set; } = true;

    private async Task ReadData()
    {
        // to keep this sample simple, we run the next infinite loop 
        // in a real application you should stop the loop/task when the view is disposed 

        for (int i = 0; IsReading; i++)
        {
            await Task.Delay(100);

            if (i > _data.ValueCount()) i = 0;

            // Because we are updating the chart from a different thread 
            // we need to use a lock to access the chart data. 
            // this is not necessary if your changes are made in the UI thread. 
            lock (Sync)
            {
                _values1.Add(new DateTimePoint(DateTime.Now, _data.ReadValue(i)));
                if (_values1.Count > _visibleElements) _values1.RemoveAt(0);

                if (i % 4 == 0)
                {
                    foreach (double? value in new List<double>() { _data.MaxValue, _data.MinValue })
                    {
                        _values2.Add(new DateTimePoint(DateTime.Now, value));
                        if (_values2.Count > _visibleElements / 4) _values2.RemoveAt(0);
                    }
                }

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