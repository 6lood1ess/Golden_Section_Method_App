using GoldenSectionMethodApp.Models;
using System.Windows.Forms.DataVisualization.Charting;

namespace GoldenSectionMethodApp.Charting {
  public interface IChartBuilder {
    void Build(Chart chart, string expression, double a, double b, double? extremum, ExtremumType extremumType);
  }
}