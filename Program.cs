namespace App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "App", "form.json");
            var repository = new Repositories.FormRepository(path);
            var service = new Services.AppService();
            Application.Run(new Form1(new Controllers.AppController(repository, service)));
        }
    }
}
