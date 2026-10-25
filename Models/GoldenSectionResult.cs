namespace GoldenSectionMethodApp.Models {
  // Итог работы метода золотого сечения
  public class GoldenSectionResult {
    public double Extremum { get; set; }
    public double FunctionValueAtExtremum { get; set; }
    public ExtremumType ExtremumType { get; set; }
    public int IterationsCount { get; set; }
  }
}