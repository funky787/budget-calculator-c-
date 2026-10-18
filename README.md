# Калькулятор бюджета (C#)

Простой REST API для подсчёта бюджета. Пользователь передаёт стоимость дня, количество дней и указывает, нужна ли страховка. Страховка добавляет 5% к итоговой сумме.

## Стек

- C# / .NET 8
- ASP.NET Core Web API
- xUnit для тестирования
- Coverlet для покрытия кода

## Запуск тестов

Открыть `BudgetCalculatorCSharp.sln` в Visual Studio 2022 или Rider. Нужен .NET 8 SDK.

В терминале из корня проекта:

```powershell
dotnet restore
dotnet test
```

Тестов шесть: обычный расчёт (500), расчёт со страховкой (525), отрицательное число дней (400), нулевая цена (0), дробная цена (301,50) и ноль дней (400).

## Покрытие кода

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

XML-отчёт Cobertura появится в `BudgetCalculator.Tests/TestResults/<идентификатор>/coverage.cobertura.xml`.

Чтобы сделать HTML-отчёт, можно дополнительно установить ReportGenerator:

```powershell
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:"BudgetCalculator.Tests/TestResults/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

Открыть `coverage-report/index.html`. Проверять нужно покрытие веток метода `Calculate`.

## Пример запроса

Запустить сервер:

```powershell
dotnet run --project BudgetCalculator.Api
```

POST `/api/budget/calculate`:

```json
{
  "dailyCost": 100,
  "days": 5,
  "includeInsurance": true
}
```

Ожидаемый ответ: `200 OK`, `{"totalAmount":525}`.

## Найденная ошибка

В исходном задании страховка считалась как `total *= 5`, то есть увеличивала сумму в пять раз. Правильный вариант — `total *= 1.05m`. Тест `Calculate_WithInsurance_AddsFivePercent` проверяет это. Для демонстрации поиска бага можно временно заменить `1.05m` на `5m`: тест со страховкой должен упасть. После возврата `1.05m` он проходит.
