using RMS_Business;

namespace RMS_UI
{
    public static class clsGlobalUser
    {
        public static clsUser? _currentUser = clsUser.Find(1);
        public static clsUser? CurrentUser { 
            get {
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