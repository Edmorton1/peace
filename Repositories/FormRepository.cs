using Form = App.Models.Form;

namespace App.Repositories;

public class FormRepository
{
    private Form? form;

    public Form? Get() => form;

    public void Save(Form formData)
    {
        form = formData;
    }
}
