﻿using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace RectangleArrangeApp2.Android
{
    [Activity(
        Label = "RectangleArrangeApp2.Android",
        Theme = "@style/MyTheme.NoActionBar",
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.Landscape,
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : AvaloniaMainActivity<App>
    {
        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder)
                .WithInterFont();
        }
    }
}
