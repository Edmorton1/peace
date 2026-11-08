using App.Models;
using Form = App.Models.Form;
using App.Repositories;
using App.Services;

namespace App.Controllers;

public class AppController(FormRepository repository, AppService service)
{
    public Form? GetForm() => repository.Get();

    public void SaveForm(Form formData)
    {
        service.Validate(formData);
        formData.FullName = formData.FullName.Trim();
        if (!formData.Patent.HasPatent) formData.Patent.IssueDate = null;
        repository.Save(formData);
    }

    public Roadmap OpenRoadmap()
    {
        var formData = repository.Get()
            ?? throw new InvalidOperationException("Сначала заполните и сохраните анкету.");
        return service.CreateRoadmap(formData);
    }
}
