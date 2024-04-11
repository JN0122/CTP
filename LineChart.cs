using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System;
using CTP;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Runtime.ExceptionServices;

namespace LineChart;

public partial class ViewModel : ObservableObject
{
    public List<double> _timeValues = new List<double>();
    public List<double> _voltageValues = new List<double>();
    public static ObservableCollection<ObservablePoint> _valuesCollection = new ObservableCollection<ObservablePoint>();
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public ViewModel()
    {
        StartChartCommand = new RelayCommand(ChartDisplayButton_Click);
    }
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<ObservablePoint>
        {
            Values = _valuesCollection,
            Fill = null,
            Stroke = null,
            GeometryFill = null
        }
    };
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Name = "t [s]",
            MinLimit = 0,
            MaxLimit = 50,
        }
    };
    public Axis[] YAxes { get; set; } =
    {
        new Axis
        {
            Name = "X [mm]",
            MinLimit = -100,
            MaxLimit = 650,
            MinStep = 100,
        }
    };

    public ICommand StartChartCommand { get; set; }
    public bool IsReading { get; set; }
    public bool IsStopped { get; set; }

    /// <summary>
    /// Rysuje lub wymazuje wykres poprzez wciśnięcie przycisku w zależności od aktualnie wykonywanej czynności.
    /// </summary>
    private void ChartDisplayButton_Click()
    {
        IsReading = !IsReading;
        if (IsReading)
        {
            IsStopped = false;
            _ = ReadData(cancellationTokenSource.Token);
        }
        else IsStopped = true;
    }

    public async Task ReadData(CancellationToken cancellationToken)
    {
        while (IsReading)
        {
            for (int i = 0; i < _voltageValues.Count; i++)
            {
                await Task.Delay(5); // Odstęp czasowy pomiędzy dodawaniem kolejnych punktów [ms].
                _valuesCollection.Add(new ObservablePoint(_timeValues[i], _voltageValues[i]));
                if (i >= _voltageValues.Count-1) _valuesCollection.Clear();
                if (IsStopped) { _valuesCollection.Clear(); break; }
            }
            await Task.Delay(1);
        }
    }

}
