using System.Windows.Forms.DataVisualization.Charting;
using GoldenSectionMethodApp.Services;
using GoldenSectionMethodApp.Theme;

namespace GoldenSectionMethodApp.Charting {
  public class FunctionChartBuilder : IChartBuilder {
    private const int PointCount = 600;
    private const double YRangeFactor = 1.4;

    private readonly IFunctionEvaluator _evaluator;

    public FunctionChartBuilder(IFunctionEvaluator evaluator) {
      _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
    }

    public void Build(Chart chart, string expression, double a, double b, double? minimum) {
      if (chart == null) {
        throw new ArgumentNullException(nameof(chart));
      }

      if (a >= b) {
        throw new ArgumentException("a должно быть меньше b");
      }

      chart.Series.Clear();
      chart.Titles.Clear();

      if (chart.ChartAreas.Count == 0) {
        chart.ChartAreas.Add(new ChartArea("MainArea"));
      }

      var area = chart.ChartAreas[0];

      area.AxisX.Minimum = a;
      area.AxisX.Maximum = b;
      area.AxisY.Minimum = double.NaN;
      area.AxisY.Maximum = double.NaN;
      area.RecalculateAxesScale();

      double step = (b - a) / PointCount;
      var points = new List<PointF>(PointCount + 1);

      double yMin = double.PositiveInfinity;
      double yMax = double.NegativeInfinity;

      for (int pointIndex = 0; pointIndex <= PointCount; ++pointIndex) {
        double x = a + pointIndex * step;

        try {
          double y = _evaluator.Evaluate(expression, x);

          if (double.IsNaN(y) || double.IsInfinity(y)) {
            continue;
          }

          points.Add(new PointF((float)x, (float)y));

          if (y < yMin) yMin = y;
          if (y > yMax) yMax = y;

        } catch {
          // Пропуск точек, где функция не определена
        }
      }

      if (points.Count == 0) {
        return;
      }

      double span = yMax - yMin;
      if (span < 1e-9) {
        span = 1.0;
      }

      double padding = span * 0.15;
      double yLow = yMin - padding;
      double yHigh = yMax + padding;

      area.BackColor = AppTheme.Card;
      area.AxisX.Title = "x";
      area.AxisY.Title = "f(x)";
      area.AxisX.LineColor = AppTheme.Border;
      area.AxisY.LineColor = AppTheme.Border;
      area.AxisX.LabelStyle.ForeColor = AppTheme.TextSecondary;
      area.AxisY.LabelStyle.ForeColor = AppTheme.TextSecondary;
      area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8f);
      area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8f);
      area.AxisX.TitleForeColor = AppTheme.TextSecondary;
      area.AxisY.TitleForeColor = AppTheme.TextSecondary;
      area.AxisX.TitleFont = new Font("Segoe UI", 9f, FontStyle.Bold);
      area.AxisY.TitleFont = new Font("Segoe UI", 9f, FontStyle.Bold);
      area.AxisX.MajorGrid.LineColor = AppTheme.Border;
      area.AxisY.MajorGrid.LineColor = AppTheme.Border;
      area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
      area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
      area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
      area.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
      area.AxisX.Minimum = a;
      area.AxisX.Maximum = b;
      area.AxisY.Minimum = yLow;
      area.AxisY.Maximum = yHigh;

      area.Position.Auto = false;
      area.Position = new ElementPosition(8, 8, 88, 84);
      area.InnerPlotPosition.Auto = false;
      area.InnerPlotPosition = new ElementPosition(14, 8, 84, 84);

      var functionSeries = new Series("Function") {
        ChartType = SeriesChartType.Line,
        BorderWidth = 2,
        Color = AppTheme.Accent,
        ChartArea = "MainArea",
        LegendText = "f(x)"
      };

      foreach (var p in points) {
        functionSeries.Points.AddXY(p.X, p.Y);
      }

      chart.Series.Add(functionSeries);

      if (minimum.HasValue && minimum.Value >= a && minimum.Value <= b) {
        double mx = minimum.Value;
        double my = _evaluator.Evaluate(expression, mx);

        var guideSeries = new Series("MinimumGuide") {
          ChartType = SeriesChartType.Line,
          BorderWidth = 1,
          Color = AppTheme.Success,
          BorderDashStyle = ChartDashStyle.Dash,
          ChartArea = "MainArea",
          LegendText = "x*"
        };

        guideSeries.Points.AddXY(mx, yLow);
        guideSeries.Points.AddXY(mx, yHigh);
        chart.Series.Add(guideSeries);

        var minimumSeries = new Series("Minimum") {
          ChartType = SeriesChartType.Point,
          MarkerStyle = MarkerStyle.Circle,
          MarkerSize = 12,
          MarkerColor = AppTheme.Success,
          MarkerBorderColor = Color.White,
          MarkerBorderWidth = 2,
          ChartArea = "MainArea",
          LegendText = $"x* = {mx:F4}"
        };

        minimumSeries.Points.AddXY(mx, my);
        chart.Series.Add(minimumSeries);

        minimumSeries.Points[0].Label = $"x* = {mx:F4}";
        minimumSeries.Points[0].Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        minimumSeries.Points[0].LabelForeColor = AppTheme.Success;
        minimumSeries.Points[0].LabelBackColor = Color.White;
        minimumSeries.Points[0].LabelBorderColor = AppTheme.Success;
        minimumSeries.Points[0].LabelBorderDashStyle = ChartDashStyle.Solid;
        minimumSeries.Points[0].LabelBorderWidth = 1;
      }
    }
  }
}