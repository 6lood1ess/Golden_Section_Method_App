namespace GoldenSectionMethodApp.Models {
  // Входные параметры задачи поиска минимума
  public class GoldenSectionParameters {
    public FunctionDefinition Function { get; }
    public double A { get; }
    public double B { get; }
    public double Epsilon { get; }

    public GoldenSectionParameters(FunctionDefinition function, double a, double b, double epsilon) {
      Function = function;
      A = a;
      B = b;
      Epsilon = epsilon;
    }
  }
}