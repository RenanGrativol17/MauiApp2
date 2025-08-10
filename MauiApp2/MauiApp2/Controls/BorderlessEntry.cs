namespace MauiApp2.Controls;

public class BorderlessEntry : Entry
{
    public BorderlessEntry()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            BackgroundColor = Colors.Transparent;
        }
    }
}