using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace RMS_UI.Utilities
{
    public static class LoginCredentialStore
    {
        private const string RegistryPath = @"Software\RMS\Auth";
        private const string RememberMeKey = "RememberMe";
        private const string UserNameKey = "UserName";
        private const string PasswordKey = "Password";

        public static void Save(string userName, string password, bool rememberMe)
        {
            if (!rememberMe)
            {
                Clear();
                return;
            }

            try
            {
                using RegistryKey? key = Registry.CurrentUser.CreateSubKey(RegistryPath, true);
                if (key == null)
                {
                    return;
                }

                string encryptedPassword = Encrypt(password);

                key.SetValue(RememberMeKey, 1, RegistryValueKind.DWord);
                key.SetValue(UserNameKey, userName ?? string.Empty, RegistryValueKind.String);
                key.SetValue(PasswordKey, encryptedPassword, RegistryValueKind.String);
            }
            catch
            {
                // Intentionally ignore persistence failures to avoid blocking login.
            }
        }

        public static bool TryLoad(out string userName, out string password, out bool rememberMe)
        {
            userName = string.Empty;
            password = string.Empty;
            rememberMe = false;

            try
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, false);
                if (key == null)
                {
                    return false;
                }

                object? rememberValue = key.GetValue(RememberMeKey);
                rememberMe = rememberValue is int intValue && intValue == 1;

                if (!rememberMe)
                {
                    return true;
                }

                userName = key.GetValue(UserNameKey)?.ToString() ?? string.Empty;
                string encryptedPassword = key.GetValue(PasswordKey)?.ToString() ?? string.Empty;
                password = Decrypt(encryptedPassword);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Clear()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(RegistryPath, false);
            }
            catch
            {
                // No-op by design.
            }
        }

        private static string Encrypt(string value)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            byte[] protectedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        private static string Decrypt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            try
            {
                byte[] protectedBytes = Convert.FromBase64String(value);
                byte[] plainBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
