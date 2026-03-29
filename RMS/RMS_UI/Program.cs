using RMS_UI.Companies;
using RMS_UI.Forms;
using RMS_UI.Peoples;

namespace RMS_UI
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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5fcnVRQ2BZUEx0WkBWYEs=");
            ApplicationConfiguration.Initialize();

            using frmLogin loginForm = new frmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm
                {
                    WindowState = FormWindowState.Maximized
                });
            }
        }
    }
}