using System;
using System.Drawing;

namespace RMS_UI.Utilities
{
    public enum ThemeMode
    {
        Light,
        Dark
    }

    public static class ThemeManager
    {
        private static ThemeMode _currentTheme = ThemeMode.Light;

        public static event EventHandler? ThemeChanged;

        public static ThemeMode CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    ThemeChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static void ToggleTheme()
        {
            CurrentTheme = CurrentTheme == ThemeMode.Light ? ThemeMode.Dark : ThemeMode.Light;
        }

        public static ColorPalette Colors => CurrentTheme == ThemeMode.Light 
            ? ColorPalette.Light 
            : ColorPalette.Dark;
    }

    public class ColorPalette
    {
        // === LIGHT THEME ===
        public static readonly ColorPalette Light = new ColorPalette
        {
            // Backgrounds
            FormBackground = Color.FromArgb(245, 247, 250),
            TitleBarBackground = Color.FromArgb(255, 255, 255),
            ContentBackground = Color.FromArgb(255, 255, 255),
            
            // Text
            TitleText = Color.FromArgb(30, 41, 59),
            PrimaryText = Color.FromArgb(51, 65, 85),
            SecondaryText = Color.FromArgb(100, 116, 139),
            
            // Borders
            BorderColor = Color.FromArgb(226, 232, 240),
            BorderAccent = Color.FromArgb(59, 130, 246),
            
            // Title Bar Buttons
            ButtonNormal = Color.FromArgb(100, 116, 139),
            ButtonHover = Color.FromArgb(241, 245, 249),
            ButtonHoverText = Color.FromArgb(51, 65, 85),
            CloseButtonHover = Color.FromArgb(239, 68, 68),
            CloseButtonHoverText = Color.FromArgb(255, 255, 255),
            
            // Primary Accent (Blue)
            Primary = Color.FromArgb(59, 130, 246),
            PrimaryHover = Color.FromArgb(37, 99, 235),
            PrimaryLight = Color.FromArgb(219, 234, 254),
            
            // Shadow
            ShadowColor = Color.FromArgb(30, 0, 0, 0)
        };

        // === DARK THEME ===
        public static readonly ColorPalette Dark = new ColorPalette
        {
            // Backgrounds
            FormBackground = Color.FromArgb(17, 24, 39),
            TitleBarBackground = Color.FromArgb(31, 41, 55),
            ContentBackground = Color.FromArgb(31, 41, 55),
            
            // Text
            TitleText = Color.FromArgb(248, 250, 252),
            PrimaryText = Color.FromArgb(226, 232, 240),
            SecondaryText = Color.FromArgb(148, 163, 184),
            
            // Borders
            BorderColor = Color.FromArgb(55, 65, 81),
            BorderAccent = Color.FromArgb(96, 165, 250),
            
            // Title Bar Buttons
            ButtonNormal = Color.FromArgb(156, 163, 175),
            ButtonHover = Color.FromArgb(55, 65, 81),
            ButtonHoverText = Color.FromArgb(248, 250, 252),
            CloseButtonHover = Color.FromArgb(239, 68, 68),
            CloseButtonHoverText = Color.FromArgb(255, 255, 255),
            
            // Primary Accent (Blue)
            Primary = Color.FromArgb(96, 165, 250),
            PrimaryHover = Color.FromArgb(59, 130, 246),
            PrimaryLight = Color.FromArgb(30, 58, 138),
            
            // Shadow
            ShadowColor = Color.FromArgb(50, 0, 0, 0)
        };

        // Instance Properties
        public Color FormBackground { get; init; }
        public Color TitleBarBackground { get; init; }
        public Color ContentBackground { get; init; }
        
        public Color TitleText { get; init; }
        public Color PrimaryText { get; init; }
        public Color SecondaryText { get; init; }
        
        public Color BorderColor { get; init; }
        public Color BorderAccent { get; init; }
        
        public Color ButtonNormal { get; init; }
        public Color ButtonHover { get; init; }
        public Color ButtonHoverText { get; init; }
        public Color CloseButtonHover { get; init; }
        public Color CloseButtonHoverText { get; init; }
        
        public Color Primary { get; init; }
        public Color PrimaryHover { get; init; }
        public Color PrimaryLight { get; init; }
        
        public Color ShadowColor { get; init; }
    }
}
