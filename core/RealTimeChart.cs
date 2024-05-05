using System.Collections.ObjectModel;
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
    private readonly List<DateTimePoint> _velocityValues = new();
    private readonly List<DateTimePoint> _accelerationValues = new();
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

        VelocitySeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _velocityValues,
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

        AccelerationSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _accelerationValues,
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

        _customAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
        {
            CustomSeparators = GetSeparators(),
            AnimationsSpeed = TimeSpan.FromMilliseconds(0),
            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100)),
        };

        XAxes = new Axis[] { _customAxis };

        _ = ReadData();
    }

    public ObservableCollection<ISeries> Series { get; set; }

    public ObservableCollection<ISeries> VelocitySeries { get; set; }

    public ObservableCollection<ISeries> AccelerationSeries { get; set; }

    public Axis[] XAxes { get; set; }
    public Axis[] YAxesDistance { get; set; }
    public Axis[] YAxesVelocity { get; set; }
    public Axis[] YAxesAcceleration { get; set; }

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
                if (_values1.Count > 250) _values1.RemoveAt(0);

                _velocityValues.Add(new DateTimePoint(DateTime.Now, _data.ReadVelocityValue(i)));
                if (_velocityValues.Count > 250) _velocityValues.RemoveAt(0);

                _accelerationValues.Add(new DateTimePoint(DateTime.Now, _data.ReadAccelerationValue(i)));
                if (_accelerationValues.Count > 250) _accelerationValues.RemoveAt(0);

                foreach (double? value in new List<double>() { _data.MaxValue, _data.MinValue })
                {
                    _values2.Add(new DateTimePoint(DateTime.Now, value));
                    if (_values2.Count > 6) _values2.RemoveAt(0);
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