using System.Windows;

namespace Banks.Client.Actions
{
    public class CloseWindows
    {
        public static void All(string windowName)
        {
            WindowCollection windows = Application.Current?.Windows;
            if (windows is null)
                return;

            foreach (Window currentWindow in Application.Current?.Windows)
            {
                if (currentWindow.Title == windowName)
                {
                    currentWindow.Close();
                }
            }
        }
    }
}