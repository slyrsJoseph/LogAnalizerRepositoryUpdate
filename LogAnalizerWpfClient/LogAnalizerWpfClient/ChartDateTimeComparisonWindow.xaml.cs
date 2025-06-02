using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


using LogAnalizerShared;

using LogAnalizerWpfClient.CustomSeries;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Legends;

namespace LogAnalizerWpfClient
{
    public partial class ChartDateTimeComparisonWindow  :MaterialDesignWindow
    {
        private readonly List<ComparisonResult> _results;
        private readonly string _week1Label;
        private readonly string _week2Label;
        private System.Timers.Timer _filterDelayTimer;
        public PlotModel PlotModel { get; private set; }

        public ChartDateTimeComparisonWindow(List<ComparisonResult> results, string week1Label, string week2Label)
        {
            InitializeComponent();

            _results = results;
            _week1Label = week1Label;
            _week2Label = week2Label;

            this.Title = $"Comparison: {_week1Label} vs {_week2Label}";

            comboReportType.ItemsSource = new List<string> { "Equipment Alarms" };
            comboReportType.SelectedIndex = 0;

            comboCategory.ItemsSource = new List<string>
                { "VPH", "BRC", "LGA", "HRN", "DDM", "DW", "DW ADS" ,"DW VFD", "DW ZPS", "DW ECS" ,"TFM", "ELT", "PDPH","TD" };
            comboCategory.SelectedIndex = -1;
        }

        public ChartDateTimeComparisonWindow(List<ComparisonResult> results, DateTimeRangeComparisonRequest request, LogApiClient apiClient)
            : this(results,
                  $"{request.Range1Start:dd.MM.yyyy} - {request.Range1End:dd.MM.yyyy}",
                  $"{request.Range2Start:dd.MM.yyyy} - {request.Range2End:dd.MM.yyyy}")
        {
        }

        private void comboReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboCategory.IsEnabled = comboReportType.SelectedItem?.ToString() == "Equipment Alarms";
        }

        private void comboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshChart();
        }

        private void comboMinCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshChart();
        }

        private void RefreshChart()
        {
            try
            {
                if (comboCategory.SelectedItem == null || _results == null)
                    return;

                string selectedCategory = comboCategory.SelectedItem.ToString();
                string subFilter = txtSubFilter.Text?.Trim() ?? "";

                if (!int.TryParse(((ComboBoxItem)comboMinCount.SelectedItem)?.Content.ToString(), out int minCount))
                    minCount = 0;

                var filteredResults = _results
                    .Where(r => !string.IsNullOrEmpty(r.AlarmMessage))
                    // .Where(r => r.AlarmMessage.Contains(selectedCategory, StringComparison.OrdinalIgnoreCase))
                    .Where(r =>
                    {
                        var msg = r.AlarmMessage;
                        if (string.IsNullOrWhiteSpace(msg))
                            return false;

                        // Строгая проверка только для DDM и DW
                        if (selectedCategory == "DDM")
                            return msg.Contains("DDM", StringComparison.OrdinalIgnoreCase)
                                   && !Regex.IsMatch(msg, @"^\s*DW", RegexOptions.IgnoreCase);
                    

                        // Для всех остальных — обычное Contains
                        return msg.Contains(selectedCategory, StringComparison.OrdinalIgnoreCase);
                    })
                    .Where(r =>
                        selectedCategory != "DW" || 
                        string.IsNullOrWhiteSpace(subFilter) ||
                        r.AlarmMessage.Contains(subFilter, StringComparison.OrdinalIgnoreCase))
                    .Where(r => r.CountWeek1 > minCount || r.CountWeek2 > minCount)
                    .OrderByDescending(r => r.CountWeek1 + r.CountWeek2)
                    .ToList();

                if (!filteredResults.Any())
                {
                    MessageBox.Show("No data for selected category or filter.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    PlotModel = new PlotModel
                    {
                        Title = "Empty Chart",
                        TextColor = OxyColors.Cyan,
                        PlotAreaBorderColor = OxyColors.Transparent
                    };
                    oxyPlotView.Model = PlotModel;

                    return;
                }

                BuildChart(filteredResults); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating chart: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      private void BuildChart(List<ComparisonResult> filteredResults)
{
    var model = new PlotModel
    {
        Title = $"Comparison: {_week1Label} vs {_week2Label}",
        TextColor = OxyColors.Cyan,
        PlotAreaBorderColor = OxyColors.Transparent,
        Background = OxyColors.Black
    };

    var legend = new Legend
    {
        LegendPosition = LegendPosition.TopRight,
        LegendOrientation = LegendOrientation.Horizontal,
        LegendPlacement = LegendPlacement.Outside,
        TextColor = OxyColors.Cyan
    };
    model.Legends.Add(legend);

    var categoryAxis = new CategoryAxis
    {
        Position = AxisPosition.Bottom,
        TextColor = OxyColors.Cyan,
        TicklineColor = OxyColors.Cyan,
        FontSize = 10,
        GapWidth = 1
    };

    foreach (var label in filteredResults.Select(r => WrapText(r.AlarmMessage, 25)))
        categoryAxis.Labels.Add(label);

    var valueAxis = new LinearAxis
    {
        Position = AxisPosition.Left,
        Title = "Count",
        TitleColor = OxyColors.Cyan,
        TextColor = OxyColors.Cyan,
        Minimum = 0,
        MajorGridlineStyle = LineStyle.Solid,
        MajorGridlineColor = OxyColors.DarkSlateGray
    };

    model.Axes.Add(categoryAxis);
    model.Axes.Add(valueAxis);

    var seriesWeek1 = new LogAnalizerWpfClient.CustomSeries.RectangleBarSeries
    {
        Title = _week1Label,
        FillColor = OxyColors.LightGray
    };

    var seriesWeek2 = new LogAnalizerWpfClient.CustomSeries.RectangleBarSeries
    {
        Title = _week2Label,
        FillColor = OxyColors.SteelBlue
    };

    const double barWidth = 0.4;

    for (int i = 0; i < filteredResults.Count; i++)
    {
        var week1 = filteredResults[i].CountWeek1;
        var week2 = filteredResults[i].CountWeek2;

        seriesWeek1.Items.Add(new RectangleBarItem
        {
            X0 = i - barWidth,
            X1 = i,
            Y0 = 0,
            Y1 = week1
        });

        seriesWeek2.Items.Add(new RectangleBarItem
        {
            X0 = i,
            X1 = i + barWidth,
            Y0 = 0,
            Y1 = week2
        });

        model.Annotations.Add(new TextAnnotation
        {
            Text = week1.ToString(),
            TextColor = OxyColors.LightGray,
            FontSize = 20,
            TextPosition = new DataPoint(i - barWidth / 2, week1 + 0.3),
            TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
            TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom
        });

        model.Annotations.Add(new TextAnnotation
        {
            Text = week2.ToString(),
            TextColor = OxyColors.SteelBlue,
            FontSize = 20,
            TextPosition = new DataPoint(i + barWidth / 2, week2 + 0.3),
            TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
            TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom
        });
    }

    model.Series.Add(seriesWeek1);
    model.Series.Add(seriesWeek2);

    PlotModel = model;
    oxyPlotView.Model = PlotModel;
}
      
      
      
      
private string WrapText(string text, int maxLength)
{
    var words = text.Split(' ');
    var lines = new List<string>();
    var currentLine = "";

    foreach (var word in words)
    {
        if ((currentLine + " " + word).Trim().Length > maxLength)
        {
            lines.Add(currentLine.Trim());
            currentLine = word;
        }
        else
        {
            currentLine += " " + word;
        }
    }

    if (!string.IsNullOrWhiteSpace(currentLine))
        lines.Add(currentLine.Trim());

    return string.Join("\n", lines);
}
      
private void txtSubFilter_TextChanged(object sender, TextChangedEventArgs e)
{
          
    if (_filterDelayTimer != null)
    {
        _filterDelayTimer.Stop();
        _filterDelayTimer.Start();
    }
}
      

private void btnSave_Click(object sender, RoutedEventArgs e)
{
    var dialog = new Microsoft.Win32.SaveFileDialog
    {
        FileName = "Chart",
        DefaultExt = ".png",
        Filter = "PNG Image (*.png)|*.png"
    };

    if (dialog.ShowDialog() != true)
        return;

    var filePath = dialog.FileName;

    var exporter = new OxyPlot.Wpf.PngExporter
    {
        Width = 1800,           
        Height = 900,
        // Background = OxyColors.Black,
        Resolution = 96
    };

    using var stream = File.Create(filePath);
    exporter.Export(PlotModel, stream);

    MessageBox.Show("Chart saved!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
}

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        
        
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            SelectedDbText.Text = $"Selected DB: {SelectedDatabaseState.CurrentDatabaseName}";
        }
        
        
    }
}