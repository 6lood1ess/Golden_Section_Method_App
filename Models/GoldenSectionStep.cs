namespace GoldenSectionMethodApp.Models {
  // Данные одного шага метода золотого сечения
  public class GoldenSectionStep {
    public int Number { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double F1 { get; set; }
    public double F2 { get; set; }
    public double IntervalLength => B - A;
  }
}