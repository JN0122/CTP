using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Factorization;
using SkiaSharp;

namespace CTP.Charts;

public partial class AllSensorsChart: ObservableObject
{
    private readonly List<ObservableCollection<ObservablePoint>> _movementValues = new() { new(), new(), new(), new() };
    private readonly List<ObservableCollection<ObservablePoint>> _velocityValues = new() { new(), new(), new(), new() };
    private readonly List<ObservableCollection<ObservablePoint>> _accelerationValues = new() { new(), new(), new(), new() };

    public AllSensorsChart()
    {
        for (int i = 0; i < _movementValues.Count; i++)
        {
            MovementSeries.Add(CreateSeries(_movementValues[i]));
            VelocitySeries.Add(CreateSeries(_velocityValues[i]));
            AccelerationSeries.Add(CreateSeries(_accelerationValues[i]));
        }

        XAxes = new[] { new Axis() };
        YAxes = new[] { new Axis() };
    }

    static ISeries CreateSeries(ObservableCollection<ObservablePoint>  _values)
    {
        return new LineSeries<ObservablePoint>()
        {
            Values = _values,
            GeometryStroke = null,
            GeometryFill = null,
            DataPadding = new(0, 1),
        };
    }

    public List<ISeries> MovementSeries { get; set; } = new();
    public List<ISeries> VelocitySeries { get; set; } = new();
    public List<ISeries> AccelerationSeries { get; set; } = new();
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }
    public List<float> Labels { get; set; } = new();


    public void SetSensorValues(int index, List<float> Values, string SensorName) {
        SetValues(_movementValues[index], Values);
        MovementSeries[index].Name = SensorName;
    }

    private void SetValues(ObservableCollection<ObservablePoint> _seriesValues, List<float> Values)
    {
        if(Labels.Count != Values.Count) throw new Exception("Labels does not match Values");

        _seriesValues.Clear();
        for (int i = 0; i < Values.Count; i++)
        {
            _seriesValues.Add(new ObservablePoint(Labels[i], Values[i]));
        }
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
        for (int i = 0; i < _movementValues.Count; i++)
        {
            if (MovementSeries[i].Name != SensorName) continue;
            MovementSeries[i].IsVisible = Show;
            return;
        }
        throw new Exception("Sensor does not exists on chart!");
    }
}