using DisableScreenshot.Views;
using Plugin.Maui.ScreenSecurity;

namespace DisableScreenshot
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Navigating += Current_Navigating;
            Navigated += AppShell_Navigated;
        }

        private static bool isSecure = false;
        private void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            var str = e.Target.Location.OriginalString;
            int cleanParams = str.LastIndexOf("?");
            if (cleanParams >= 0)
                str = str.Substring(0, cleanParams);
            if (!isSecure && isSecurityPage(str))
            {
                isSecure = true;
                ScreenSecurity.Default.ActivateScreenSecurityProtection(true, true, true);
            }
        }

        private void AppShell_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            var currentPath = GetCurrentPath();

            if (isSecure && !string.IsNullOrWhiteSpace(currentPath) &&
                !isSecurityPage(currentPath))
            {
                isSecure = false;
                ScreenSecurity.Default.DeactivateScreenSecurityProtection();
            }
        }

        public string GetCurrentPath()
        {
            var path = "";
            foreach (var page in Shell.Current.Navigation.NavigationStack)
            {
                path += GetPageName($"{page}") + "/";
            }

            foreach (var item in Shell.Current.Navigation.ModalStack)
            {
                    path += Shell.Current.CurrentPage.GetType().Name + "/";
            }

            if (path.Equals("/"))
            {
                path = GetPageName($"{Shell.Current.CurrentPage}");
                corePage = path.TrimEnd('/');
            }
            if (!string.IsNullOrEmpty(corePage) && corePage != path)
                path = $"{corePage}{path}";
            return path.TrimEnd('/');
        }
        private string previousPath;
        private string corePage;
        private string GetPageName(string page)
        {
            if (page.Contains('.'))
            {
                var idx = page.LastIndexOf('.');
                return page.Substring(idx + 1);
            }
            return page;
        }

        private bool isSecurityPage(string currentPath)
        {
            return currentPath.EndsWith(nameof(DScreenshot));
        }

    }
}
