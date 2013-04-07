using System.Collections.Generic;
using DxLibDLL;

namespace Progressive.Scarlex13.Infrastructures
{
    internal class SoundManager : DxLibraryBase
    {
        private readonly Dictionary<string, int> _handles
            = new Dictionary<string, int>();

        public void Play(string fileName)
        {
            if (!_handles.ContainsKey(fileName))
                _handles[fileName] = DX.LoadSoundMem(GetPath(fileName));
            DX.PlaySoundMem(_handles[fileName], DX.DX_PLAYTYPE_BACK);
        }

        public void PlayMusic(string fileName)
        {
            DX.PlayMusic(GetPath(fileName), DX.DX_PLAYTYPE_LOOP);
        }

        public void StopMusic()
        {
            DX.StopMusic();
        }
    }
}