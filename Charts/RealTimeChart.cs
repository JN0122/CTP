using System.Collections.ObjectModel;
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
    private readonly List<ObservableCollection<DateTimePoint>> _values = new() { new(), new(), new(), new() };
    private readonly DateTimeAxis _customAxis;

    public int VisibleElements { get; set; } = 350;

    public RealTimeChart()
    {
        for (int i = 0; i < _values.Count; i++)
        {
            Series.Add(CreateSeries(_values[i]));
        }

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

    static ISeries CreateSeries(ObservableCollection<DateTimePoint> _seriesValues)
    {
        return new LineSeries<DateTimePoint>()
        {
            Values = _seriesValues,
            Fill = null,
            GeometryFill = null,
            GeometryStroke = null,
        };
    }

    public ObservableCollection<ISeries> Series { get; set; } = new();

    public Axis[] XAxes { get; set; }

    public Axis[] YAxesDistance { get; set; }

    public Axis[] YAxesVelocity { get; set; }

    public Axis[] YAxesAcceleration { get; set; }

    public object Sync { get; } = new object();

    public bool IsReading { get; set; } = true;

    public List<List<float>> AllValues { get; set; } = new() { new(), new(), new(), new() };

    private int i = 0;

    public void ClearChart()
    {
        for(int j = 0; j < _values.Count; j++)
        {
            _values[j].Clear();
            AllValues[j].Clear();
        }
    }
 
    public void SetSensorValues(int index, List<float> Values, string SensorName)
    {
        AllValues[index] = Values;
        Series[index].Name = SensorName;
    }

    private async Task ReadData()
    {
        while (IsReading)
        {
            await Task.Delay(100);

            if (AllValues[0].Count == 0) continue;

            if (i >= AllValues[0].Count) i = 0;

            lock (Sync)
            {
                for(int j = 0; j < _values.Count && AllValues[0].Count == AllValues[j].Count; j++)
                {
                    _values[j].Add(new DateTimePoint(DateTime.Now, AllValues[j][i]));
                    if (_values[j].Count > VisibleElements) _values[j].RemoveAt(0);
                }
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

    public void ShowSensor(string SensorName)
    {
        ChangeSensorVisibility(SensorName, true);
    }

    public void HideSensor(string SensorName)
    {
        ChangeSensorVisibility(SensorName, false);
    }

    private void ChangeSensorVisibility(string SensorName, bool Show)
    {
        for (int i = 0; i < _values.Count; i++)
        {
            if (Series[i].Name != SensorName) continue;
            Series[i].IsVisible = Show;
            return;
        }
        throw new Exception("Sensor does not exists on chart!");
    }
}