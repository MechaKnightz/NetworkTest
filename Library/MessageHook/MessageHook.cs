using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class MessageHook : WindowsHook
    {
        public MessageHook(IntPtr hWnd) : base(hWnd)
        {
        }

        protected override void WndProc(ref Win32.Message message)
        {
            throw new NotImplementedException();
        }
    }
}
