using DxLibDLL;
using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Infrastructures
{
    internal class RealtimeInput
    {
        private byte _prevDirection = 5;
        private bool _prevShot;
        private bool _prevPause;

        public Input GetInput()
        {
            int flag = DX.GetJoypadInputState(DX.DX_INPUT_KEY_PAD1);
            byte nextDirection = GetDirection(flag);
            bool? nextShot = (flag & DX.PAD_INPUT_1) > 0;
            bool? nextPause = (flag & DX.PAD_INPUT_2) > 0;
            if (_prevDirection != nextDirection)
                _prevDirection = nextDirection;
            else
                nextDirection = 0;
            if (_prevShot != nextShot)
                _prevShot = nextShot.Value;
            else
                nextShot = null;
            if (_prevPause != nextPause)
                _prevPause = nextPause.Value;
            else
                nextPause = null;
            return new Input(nextDirection, nextShot, nextPause);
        }

        private static byte GetDirection(int dxPadInputState)
        {
            return IsOn(dxPadInputState, DX.PAD_INPUT_UP | DX.PAD_INPUT_RIGHT) ? (byte)9
                : IsOn(dxPadInputState, DX.PAD_INPUT_RIGHT | DX.PAD_INPUT_DOWN) ? (byte)3
                : IsOn(dxPadInputState, DX.PAD_INPUT_DOWN | DX.PAD_INPUT_LEFT) ? (byte)1
                : IsOn(dxPadInputState, DX.PAD_INPUT_LEFT | DX.PAD_INPUT_UP) ? (byte)7
                : IsOn(dxPadInputState, DX.PAD_INPUT_UP) ? (byte)8
                : IsOn(dxPadInputState, DX.PAD_INPUT_RIGHT) ? (byte)6
                : IsOn(dxPadInputState, DX.PAD_INPUT_DOWN) ? (byte)2
                : IsOn(dxPadInputState, DX.PAD_INPUT_LEFT) ? (byte)4
                : (byte)5;
        }

        private static bool IsOn(int flag, int test)
        {
            return (flag & test) == test;
        }
    }
}