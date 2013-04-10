using System;
using System.Collections.Generic;
using DxLibDLL;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.Infrastructures
{
    internal class Renderer : DxLibraryBase, IDisposable
    {
        private readonly Dictionary<string, int> _handles
            = new Dictionary<string, int>();

        public Renderer()
        {
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetGraphMode(800, 500, 32);
            if (DX.DxLib_Init() < 0)
                throw new Exception();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.SetTransColor(0, 255, 0);
            DX.SetFontSize(28);
            DX.SetFontSpace(5);
            DX.SetFontThickness(9);
        }

        #region IDisposable Members

        /// <summary>
        ///     Internal variable which checks if Dispose has already been called
        /// </summary>
        private Boolean _disposed;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Call the private Dispose(bool) helper and indicate 
            // that we are explicitly disposing
            Dispose(true);

            // Tell the garbage collector that the object doesn't require any
            // cleanup when collected since Dispose was called explicitly.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        private void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                //Managed cleanup code here, while managed refs still valid
            }
            DX.DxLib_End();

            _disposed = true;
        }

        #endregion

        public void Flip()
        {
            DX.ScreenFlip();
            DX.ClearDrawScreen();
        }

        public void Load(string fileName)
        {
            int handle = DX.LoadGraph(GetPath(fileName));
            if (handle == -1)
                throw new Exception();
            _handles.Add(fileName, handle);
        }

        public void Release(string fileName)
        {
            if (DX.DeleteGraph(_handles[fileName]) < 0)
                throw new Exception();
        }

        public void Draw(string fileName, Point point)
        {
            if (!_handles.ContainsKey(fileName))
                Load(fileName);

            DX.DrawGraph(point.X, point.Y, _handles[fileName], DX.TRUE);
        }

        public void DrawRotate(string fileName, Point point, double angle)
        {
            if (!_handles.ContainsKey(fileName))
                Load(fileName);

            DX.DrawRotaGraph(point.X, point.Y,
                1.0, angle, _handles[fileName], DX.TRUE);
        }

        public void Draw(string fileName, Point point, byte alpha)
        {
            DX.SetDrawBright(alpha, alpha, alpha);
            Draw(fileName, point);
            DX.SetDrawBright(255, 255, 255);
        }

        public void DrawClip(string fileName, Point src, Size size, Point point)
        {
            if (!_handles.ContainsKey(fileName))
                Load(fileName);

            DX.DrawRectGraph(point.X, point.Y, src.X, src.Y, size.Width, size.Height,
                _handles[fileName], DX.TRUE, DX.FALSE);
        }

        public void DrawText(string text, Point point, Color color)
        {
            DX.DrawString(point.X, point.Y, text, ToDxColor(color));
        }

        public void DrawPixel(Point point, Color color)
        {
            DX.DrawPixel(point.X, point.Y, ToDxColor(color));
        }

        public void DrawLine(Point from, Point to, Color color)
        {
            DX.DrawLine(from.X, from.Y, to.X, to.Y, ToDxColor(color));
        }

        private static int ToDxColor(Color color)
        {
            return DX.GetColor(color.Red, color.Green, color.Blue);
        }
    }
}