namespace GoldenSectionMethodApp.Models {
  // Итог работы метода золотого сечения
  public class GoldenSectionResult {
    public double Minimum { get; set; }
    public double FunctionValueAtMinimum { get; set; }
    public int IterationsCount { get; set; }
    public List<GoldenSectionStep> Steps { get; set; } = new List<GoldenSectionStep>();
  }
}