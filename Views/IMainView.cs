using GoldenSectionMethodApp.Models;

namespace GoldenSectionMethodApp.Views {
  // Абстракция главного окна
  public interface IMainView {
    string FunctionExpression { get; }
    string AText { get; }
    string BText { get; }
    string EpsilonText { get; }

    void ShowSteps(IEnumerable<GoldenSectionStep> steps);
    void ShowMinimum(string minimumText);
    void ClearSteps();
    void ClearMinimum();

    void BuildChart(string expression, double a, double b, double? minimum);

    void ShowError(string message);
    void ShowInfo(string message);

    event EventHandler CalculateRequested;
    event EventHandler ClearRequested;
    event EventHandler BuildChartRequested;
  }
}