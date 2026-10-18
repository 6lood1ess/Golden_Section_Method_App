using System.ComponentModel.DataAnnotations;

namespace GoldenSectionMethodApp.Validators {
  // Проверка входных данных пользователя
  public interface IInputValidator {
    ValidationResult Validate(string functionExpr, string aText, string bText, string eText);
  }
}