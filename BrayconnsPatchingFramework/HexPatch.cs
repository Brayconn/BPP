using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace BrayconnsPatchingFramework.HexPatch
{
    public class HexPatch : IHack
    {
        readonly static Regex patchFinder = new Regex(@"(0x[A-Fa-f0-9]+)\s+((?:[A-Fa-f0-9]{2}(?:\s+|$))+)", RegexOptions.Compiled | RegexOptions.Multiline);
        public static HexPatch Parse(string filename)
        {
            var text = File.ReadAllText(filename);
            var matches = patchFinder.Matches(text);
            if (matches.Count <= 0)
                throw new FileLoadException("No patches were found in this file. It probably isn't a hex patch.", filename);
            
            IPatch[] patches = new IPatch[matches.Count];
            for(int i = 0; i < matches.Count; i++)
            {
                ulong address = Convert.ToUInt64(matches[i].Groups[1].Value, 16);
                
                byte[] data = matches[i].Groups[2].Value.Split(null)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => Convert.ToByte(x, 16))
                    .ToArray();
                
                patches[i] = new Patch(address, data);
            }
            return new HexPatch(Path.GetFileName(filename), patches);
        }

        IPatch[] patches;

        HexPatch(string name, params IPatch[] p)
        {
            Name = name;
            patches = p;
        }

        public string Name { get; private set; }

        public string EXE => null;

        public object Clone()
        {
            return new HexPatch(Name, copyOfPatches);
        }

        [System.ComponentModel.Browsable(false)]
        public bool WantsDefaultValues => false;
        public void LoadDefaultValues(string filename, ulong baseAddress) { }
        
        public void LoadSettings(HackSettings hs) { }

        HackSettings settings => new HackSettings(EXE, Name);
        IPatch[] copyOfPatches
        {
            get
            {
                Patch[] p = new Patch[patches.Length];
                patches.CopyTo(p, 0);
                return p.DeepClone();
            }
        }

        public HackSignature<IPatchFootprint> ToHackHistory()
        {
            return new HackSignature<IPatchFootprint>(settings, copyOfPatches);
        }

        public HackSignature<IPatch> ToHackInfo()
        {
            return new HackSignature<IPatch>(settings, copyOfPatches);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
