using System;
using DxLibDLL;

namespace Progressive.Scarlex13.Infrastructures
{
    internal class Messaging
    {
        public void MessageLoop(Func<bool> action)
        {
            while (DX.ProcessMessage() >= 0)
                if (!action())
                    break;
        }
    }
}