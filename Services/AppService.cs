using App.Models;
using Form = App.Models.Form;

namespace App.Services;

public class AppService
{
    public static void Validate(Form formData)
    {
        if (string.IsNullOrWhiteSpace(formData.FullName))
            throw new ArgumentException("Введите ФИО.");
        if (!Enum.IsDefined(formData.Contract.Type))
            throw new ArgumentException("Укажите статус договора.");
        if (!formData.Patent.HasPatent) return;

        var date = formData.Patent.IssueDate?.Date;
        var today = DateTime.Today;
        if (date == null)
            throw new ArgumentException("Укажите дату выдачи патента.");
        if (date > today || date < today.AddMonths(-2))
            throw new ArgumentException("Дата патента не должна быть в будущем или старше двух месяцев.");
    }

    public static Roadmap CreateRoadmap(Form formData)
    {
        var roadmap = new Roadmap { Form = formData };
        if (!formData.Patent.HasPatent)
        {
            roadmap.Steps.Add(new RoadmapStep { Description = "Получить патент (заглушка)." });
            return roadmap;
        }

        // В задании нет точных действий и сроков. Все значения ниже — учебные заглушки.
        var date = formData.Patent.IssueDate!.Value.Date;
        var contractName = formData.Contract.Type == ContractType.Employment
            ? "трудовом договоре" : "договоре ГПХ";
        roadmap.Steps.Add(new RoadmapStep
        {
            Description = "Проверить данные полученного патента (заглушка).",
            Deadline = date.AddDays(7)
        });
        roadmap.Steps.Add(new RoadmapStep
        {
            Description = formData.Contract.HasContract
                ? $"Подготовить сведения о {contractName} (заглушка)."
                : "Заключить трудовой договор или договор ГПХ (заглушка).",
            Deadline = date.AddMonths(2)
        });
        roadmap.Steps.Add(new RoadmapStep
        {
            Description = "Проверить следующий платёж по патенту (заглушка).",
            Deadline = date.AddMonths(1)
        });
        roadmap.Steps = [.. roadmap.Steps.OrderBy(step => step.Deadline)];
        return roadmap;
    }
}
