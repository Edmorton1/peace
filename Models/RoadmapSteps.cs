using Form = App.Models.Form;

namespace App.Models;

public static class RoadmapSteps
{
    // Offset задаёт срок от даты выдачи патента; null — шаг без срока.
    private static readonly (Func<DateTime, DateTime>? Offset, string Description, Func<Form, bool> Filter)[] Steps =
    [
        (
            null,
            "Получить патент (заглушка).",
            form => !form.Patent.HasPatent
        ),
        (
            date => date.AddDays(7),
            "Проверить данные полученного патента (заглушка).",
            form => form.Patent.HasPatent
        ),
        (
            date => date.AddMonths(2),
            "Заключить трудовой договор или договор ГПХ (заглушка).",
            form => form.Patent.HasPatent && !form.Contract.HasContract
        ),
        (
            date => date.AddMonths(2),
            "Подготовить сведения о трудовом договоре (заглушка).",
            form => form.Patent.HasPatent && form.Contract.Type == ContractType.Employment
        ),
        (
            date => date.AddMonths(2),
            "Подготовить сведения о договоре ГПХ (заглушка).",
            form => form.Patent.HasPatent && form.Contract.Type == ContractType.CivilLaw
        ),
        (
            date => date.AddMonths(1),
            "Проверить следующий платёж по патенту (заглушка).",
            form => form.Patent.HasPatent
        )
    ];

    public static List<RoadmapStep> CreateSteps(Form formData)
    {
        var steps = new List<RoadmapStep>();
        foreach (var step in Steps)
        {
            if (!step.Filter(formData)) continue;

            steps.Add(new RoadmapStep
            {
                Description = step.Description,
                Deadline = step.Offset is null ? null : step.Offset(formData.Patent.IssueDate!.Value.Date)
            });
        }

        return [.. steps.OrderBy(step => step.Deadline)];
    }
}
