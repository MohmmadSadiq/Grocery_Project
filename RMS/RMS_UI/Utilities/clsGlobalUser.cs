using RMS_Business;

namespace RMS_UI
{
    public static class clsGlobalUser
    {
        private static clsUser? _currentUser;

        public static clsUser? CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        public static bool IsUserLoggedIn()
        {
            return CurrentUser != null;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}