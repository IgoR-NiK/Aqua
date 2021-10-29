using System;

namespace Aqua.Core.Services
{
    public class ViewClosingArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}