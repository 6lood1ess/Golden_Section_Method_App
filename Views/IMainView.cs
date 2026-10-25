using GoldenSectionMethodApp.Models;

namespace GoldenSectionMethodApp.Views {
  // Абстракция главного окна
  public interface IMainView {
    string FunctionExpression { get; }
    string AText { get; }
    string BText { get; }
    string EpsilonText { get; }

    ExtremumType SelectedExtremumType { get; }

    void ShowSteps(IEnumerable<GoldenSectionStep> steps);
    void ShowExtremum(string extremumText, ExtremumType extremumType);
    void ClearSteps();
    void ClearExtremum();

    void BuildChart(string expression, double a, double b, double? extremum, ExtremumType extremumType);

    void ShowError(string message);
    void ShowInfo(string message);

    event EventHandler CalculateRequested;
    event EventHandler ClearRequested;
    event EventHandler BuildChartRequested;
  }
}