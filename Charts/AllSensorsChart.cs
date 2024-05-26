using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
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
    private readonly List<ObservableCollection<ObservablePoint>> _values = new() { new(), new(), new(), new() };

    public AllSensorsChart()
    {
        for (int i = 0; i < _values.Count; i++)
        {
            Series.Add(CreateSeries(_values[i]));
        }

        XAxes = new[] { new Axis() { Name = "Czas [s]"} };
        YAxes = new[] { new Axis() { Name = "Prędkość obrotowa [obr/min]" } };
    }

    static ISeries CreateSeries(ObservableCollection<ObservablePoint>  _seriesValues)
    {
        return new LineSeries<ObservablePoint>()
        {
            Values = _seriesValues,
            GeometryStroke = null,
            GeometryFill = null,
            DataPadding = new(0, 1),
        };
    }

    public List<ISeries> Series { get; set; } = new();
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }
    private List<float> _labels { get; set; } = new();

    public void SetSensorValues(int index, List<float> Values, string SensorName) {
        SetValues(_values[index], Values);
        Series[index].Name = SensorName;
    }

    public void ClearAllSeries()
    {
        for(int i=0; i < _values.Count; i++)
        {
            _values[i].Clear();
        }
    }

    public void SetLabels(List<float> Labels)
    {
        _labels = Labels;
        XAxes[0].MaxLimit = _labels.Last();
    }

    private void SetValues(ObservableCollection<ObservablePoint> _seriesValues, List<float> Values)
    {
        if(_labels.Count != Values.Count) throw new Exception("Labels does not match Values");

        for (int i = 0; i < Values.Count; i++)
        {
            _seriesValues.Add(new ObservablePoint(_labels[i], Values[i]));
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
        for (int i = 0; i < _values.Count; i++)
        {
            if (Series[i].Name != SensorName) continue;
            Series[i].IsVisible = Show;
            return;
        }
        throw new Exception("Sensor does not exists on chart!");
    }
}