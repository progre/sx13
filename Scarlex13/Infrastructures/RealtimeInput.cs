using DxLibDLL;
using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Infrastructures
{
    internal class RealtimeInput
    {
        private byte _prevDirection = 5;
        private bool _prevShot;
        private bool _prevPause;
        private bool _prevTweet;

        public Input GetInput()
        {
            int flag = DX.GetJoypadInputState(DX.DX_INPUT_KEY_PAD1);

            byte nextDirection = GetDirection(flag);
            var directionChanged = _prevDirection != nextDirection;

            var nextShot = (flag & DX.PAD_INPUT_1) > 0;
            var shotToggled = _prevShot != nextShot;

            var nextPause = (flag & DX.PAD_INPUT_2) > 0;
            var pauseToggled = _prevPause != nextPause;

            var nextTweet = DX.CheckHitKey(DX.KEY_INPUT_T) == DX.TRUE;
            var tweetToggled = _prevTweet != nextTweet;

            var nextInput = new Input(
                nextDirection, directionChanged,
                nextShot, shotToggled,
                nextPause, pauseToggled,
                nextTweet, tweetToggled);

            _prevDirection = nextDirection;
            _prevShot = nextShot;
            _prevPause = nextPause;
            _prevTweet = nextTweet;
            return nextInput;
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