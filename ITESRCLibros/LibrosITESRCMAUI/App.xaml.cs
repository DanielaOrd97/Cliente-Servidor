using LibrosITESRCMAUI.Services;

namespace LibrosITESRCMAUI
{
    public partial class App : Application
    {
        public static LibroService LibroService { get; set; } = new();

        public App()
        {
            InitializeComponent();

            Thread thread = new Thread(Sincronizador) { IsBackground = true };
            thread.Start();

            MainPage = new AppShell();
        }

        async void Sincronizador()
        {
            while (true)
            {
                await LibroService.GetLibros();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
    }
}
