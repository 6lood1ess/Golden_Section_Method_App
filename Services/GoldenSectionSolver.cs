using GoldenSectionMethodApp.Models;

namespace GoldenSectionMethodApp.Services {
  // Реализация метода золотого сечения для поиска мин. и макс. унимодальной функции
  public class GoldenSectionSolver : IGoldenSectionSolver {
    private const int MaxIterations = 10_000;
    private const double Tau = 0.6180339887498949; // (sqrt(5) - 1) / 2

    private readonly IFunctionEvaluator _evaluator;

    public GoldenSectionSolver(IFunctionEvaluator evaluator) {
      _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
    }

    public GoldenSectionResult Solve(GoldenSectionParameters parameters) {
      if (parameters == null) {
        throw new ArgumentNullException(nameof(parameters));
      }

      if (parameters.A >= parameters.B) {
        throw new ArgumentException("Левая граница должна быть меньше правой");
      }

      if (parameters.Epsilon <= 0) {
        throw new ArgumentException("Точность должна быть положительной");
      }

      CheckContinuity(parameters.Function.Expression, parameters.A, parameters.B);

      string expr = parameters.Function.Expression;

      double sign = parameters.ExtremumType == ExtremumType.Minimum ? 1.0 : -1.0;

      double left = parameters.A;
      double right = parameters.B;

      double x1 = right - Tau * (right - left);
      double x2 = left + Tau * (right - left);

      double f1 = sign * _evaluator.Evaluate(expr, x1);
      double f2 = sign * _evaluator.Evaluate(expr, x2);

      var result = new GoldenSectionResult { ExtremumType = parameters.ExtremumType};
      int n = 1;

      while ((right - left) > parameters.Epsilon && n <= MaxIterations) {
        if (f1 < f2) {
          // Экстремум в левой части - отбрасывает правую
          right = x2;

          x2 = x1;
          f2 = f1;

          x1 = right - Tau * (right - left);
          f1 = sign * _evaluator.Evaluate(expr, x1);

        } else {
          // Экстремум в правой части - отбрасывает левую
          left = x1;

          x1 = x2;
          f1 = f2;

          x2 = left + Tau * (right - left);
          f2 = sign * _evaluator.Evaluate(expr, x2);
        }

        n++;
      }

      if (n > MaxIterations) {
        throw new InvalidOperationException("Превышено максимальное число итераций");
      }

      result.Extremum = (left + right) / 2.0;
      result.FunctionValueAtExtremum = _evaluator.Evaluate(expr, result.Extremum);
      result.IterationsCount = n - 1;

      return result;
    }

    private void CheckContinuity(string expression, double a, double b) {
      const int SampleCount = 200;
      double step = (b - a) / SampleCount;

      for (int sampleIndex = 0; sampleIndex <= SampleCount; ++sampleIndex) {
        double x = a + sampleIndex * step;
        double y;

        try {
          y = _evaluator.Evaluate(expression, x);
        } catch (Exception ex) {
          throw new InvalidOperationException(
            $"Функция не определена в точке x = {x:F6}: {ex.Message}", ex);
        }

        if (double.IsNaN(y) || double.IsInfinity(y)) {
          throw new InvalidOperationException(
            $"Функция не определена в точке x = {x:F6} (значение = {y}). " +
            "На отрезке [a, b], скорее всего, есть точка разрыва");
        }
      }
    }
  }
}