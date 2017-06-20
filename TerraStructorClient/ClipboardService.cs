using System.Threading;
using System.Windows.Forms;
using EmptyKeys.UserInterface.Mvvm;

namespace TerraStructorClient
{
    public class ClipboardService : IClipboardService
    {
        public string GetText()
        {
            string text = string.Empty;
            Thread thread = new Thread(() => text = Clipboard.GetText());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return text;
        }

        public void SetText(string text)
        {
            Thread thread = new Thread(() => Clipboard.SetText(text));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
