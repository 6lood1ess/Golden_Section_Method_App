using GoldenSectionMethodApp.Models;

namespace GoldenSectionMethodApp.Services {
  // Интерфейс решателя методом золотого сечения
  public interface IGoldenSectionSolver {
    GoldenSectionResult Solve(GoldenSectionParameters parameters);
  }
}