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
        return new Roadmap
        {
            Form = formData,
            Steps = RoadmapSteps.CreateSteps(formData)
        };
    }
}
