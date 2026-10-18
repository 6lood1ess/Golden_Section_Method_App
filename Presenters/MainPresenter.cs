using GoldenSectionMethodApp.Infrastructure;
using GoldenSectionMethodApp.Models;
using GoldenSectionMethodApp.Services;
using GoldenSectionMethodApp.Validators;
using GoldenSectionMethodApp.Views;

namespace GoldenSectionMethodApp.Presenters {
  public class MainPresenter {
    private readonly IMainView _view;
    private readonly IInputValidator _validator;
    private readonly IGoldenSectionSolver _solver;

    public MainPresenter(
      IMainView view,
      IInputValidator validator,
      IGoldenSectionSolver solver) {

      _view = view ?? throw new ArgumentNullException(nameof(view));
      _validator = validator ?? throw new ArgumentNullException(nameof(validator));
      _solver = solver ?? throw new ArgumentNullException(nameof(solver));

      _view.CalculateRequested += OnCalculateRequested;
      _view.ClearRequested += OnClearRequested;
      _view.BuildChartRequested += OnBuildChartRequested;
    }

    private void OnCalculateRequested(object sender, EventArgs e) {
      var validation = _validator.Validate(
        _view.FunctionExpression,
        _view.AText,
        _view.BText,
        _view.EpsilonText);

      if (!validation.IsValid) {
        _view.ShowError(validation.ErrorMessage);
        return;
      }

      NumberParser.TryParse(_view.AText, out double a);
      NumberParser.TryParse(_view.BText, out double b);
      NumberParser.TryParse(_view.EpsilonText, out double eps);

      try {
        var parameters = new GoldenSectionParameters(
          new FunctionDefinition(_view.FunctionExpression), a, b, eps);

        var result = _solver.Solve(parameters);

        _view.ClearSteps();
        _view.ShowSteps(result.Steps);

        string minimumText =
          $"x* = {result.Minimum:F6}    f(x*) = {result.FunctionValueAtMinimum:F6}{Environment.NewLine}" +
          $"Итераций: {result.IterationsCount}";

        _view.ShowMinimum(minimumText);
        _view.BuildChart(_view.FunctionExpression, a, b, result.Minimum);

      } catch (Exception ex) {
        _view.ShowError(ex.Message);
      }
    }

    private void OnBuildChartRequested(object sender, EventArgs e) {
      var validation = _validator.Validate(
        _view.FunctionExpression,
        _view.AText,
        _view.BText,
        _view.EpsilonText);

      if (!validation.IsValid) {
        _view.ShowError(validation.ErrorMessage);
        return;
      }

      NumberParser.TryParse(_view.AText, out double a);
      NumberParser.TryParse(_view.BText, out double b);

      try {
        _view.BuildChart(_view.FunctionExpression, a, b, null);

      } catch (Exception ex) {
        _view.ShowError($"Ошибка построения графика: {ex.Message}");
      }
    }

    private void OnClearRequested(object sender, EventArgs e) {
      _view.ClearSteps();
      _view.ClearMinimum();
      _view.BuildChart(_view.FunctionExpression, 0, 1, null);
    }
  }
}