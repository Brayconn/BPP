using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BrayconnsPatchingFramework
{
    public static class PatchApplier
    {
        public const ulong DefaultBaseAddress = 0x400000;
                
        public static bool CheckPatchCollisions<T>(IList<T> patches1, IList<T> patches2) where T : IPatchFootprint
        {
            for(int i = 0; i < patches1.Count; i++)
            {
                for(int j = 0; j < patches2.Count; j++)
                {
                    if (patches1[i].Address < patches2[j].Address + patches2[j].Length
                     && patches2[j].Address < patches1[i].Address + patches1[i].Length)
                        return true;
                }
            }
            return false;
        }
        public static IList<Tuple<HackSignature<T>, HackSignature<T>>> CheckHackCollisions<T>(IList<HackSignature<T>> hacks) where T : IPatchFootprint
        {
            var collisions = new List<Tuple<HackSignature<T>, HackSignature<T>>>();
            for(int i = 0; i < hacks.Count - 1; i++)
            {
                for(int j = i + 1; j < hacks.Count; j++)
                {
                    if (CheckPatchCollisions(hacks[i].Patches, hacks[j].Patches))
                        collisions.Add(new Tuple<HackSignature<T>, HackSignature<T>>(hacks[i], hacks[j]));
                }
            }
            return collisions;
        }

        public static void ApplyHacks<T>(string exePath, IEnumerable<HackSignature<T>> hacks, ulong baseAddress = DefaultBaseAddress) where T : IPatch
        {
            using BinaryWriter bw = new BinaryWriter(new FileStream(exePath, FileMode.Open, FileAccess.Write));
            foreach (var hack in hacks)
            {
                foreach (var patch in hack.Patches)
                {
                    bw.BaseStream.Seek((long)(patch.Address - baseAddress), SeekOrigin.Begin);
                    bw.Write(patch.Data);
                }
            }
        }

        public static void UndoHacks<T>(string exePath, string originalExePath, IEnumerable<HackSignature<T>> hacks, ulong baseAddress = DefaultBaseAddress) where T : IPatchFootprint
        {
            using BinaryReader br = new BinaryReader(new FileStream(originalExePath, FileMode.Open, FileAccess.Read));
            using BinaryWriter bw = new BinaryWriter(new FileStream(exePath, FileMode.Open, FileAccess.Write));
            foreach (var hack in hacks)
            {
                foreach (var patch in hack.Patches)
                {
                    ulong add = patch.Address - baseAddress;
                    br.BaseStream.Seek((long)add, SeekOrigin.Begin);
                    bw.BaseStream.Seek((long)add, SeekOrigin.Begin);
                    bw.Write(br.ReadBytes((int)patch.Length));
                }
            }
        }

        private static List<HackSignature<PatchFootprint>> ToSaveable<T>(IEnumerable<HackSignature<T>> hacks) where T : IPatchFootprint
        {
            List<HackSignature<PatchFootprint>> processedHacks = new List<HackSignature<PatchFootprint>>();
            foreach (var hack in hacks)
            {
                processedHacks.Add(new HackSignature<PatchFootprint>(hack.Settings));
                foreach (var patch in hack.Patches)
                {
                    processedHacks[processedHacks.Count - 1].Patches.Add(new PatchFootprint(patch));
                }
            }
            return processedHacks;
        }

        public static void SavePatchHistory<T>(IEnumerable<HackSignature<T>> hacks, string path) where T : IPatchFootprint
        {   
            SavePatchHistory(ToSaveable(hacks), path);
        }
        public static void SavePatchHistory<T>(IEnumerable<HackSignature<T>> hacks, Stream stream) where T : IPatchFootprint
        {
            SavePatchHistory(ToSaveable(hacks), stream);
        }

        public static void SavePatchHistory(List<HackSignature<PatchFootprint>> hacks, string path)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            SavePatchHistory(hacks, fs);
        }
        public static void SavePatchHistory(List<HackSignature<PatchFootprint>> hacks, Stream stream)
        {
            var xs = new XmlSerializer(hacks.GetType());
            var sw = new StreamWriter(stream);
            xs.Serialize(sw, hacks);
        }

        public static List<HackSignature<PatchFootprint>> LoadPatchHistory(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return LoadPatchHistory(fs);
        }
        public static List<HackSignature<PatchFootprint>> LoadPatchHistory(Stream stream)
        {
            var xs = new XmlSerializer(typeof(List<HackSignature<PatchFootprint>>));
            var sr = new StreamReader(stream);
            return (List<HackSignature<PatchFootprint>>)xs.Deserialize(sr);
        }        

        public static List<HackSignature<PatchFootprint>> GeneratePatchHistory(string originalEXE, string modifiedEXE, int maxGapSize = 16)
        {
            var p = new HackSignature<PatchFootprint>(new HackSettings(Path.GetFileName(modifiedEXE), "Generated Patch History"));
            using var oribr = new BinaryReader(new FileStream(originalEXE, FileMode.Open, FileAccess.Read));
            using var modbr = new BinaryReader(new FileStream(modifiedEXE, FileMode.Open, FileAccess.Read));
            while (oribr.BaseStream.Position != oribr.BaseStream.Length)
            {
                //scan for a change
                while (oribr.BaseStream.Position < oribr.BaseStream.Length && oribr.ReadByte() == modbr.ReadByte()) ;
                //stop if reaches the end of stream
                if (oribr.BaseStream.Position == oribr.BaseStream.Length)
                    break;
                ulong address = (ulong)(oribr.BaseStream.Position - 1);
                //store the length
                ulong length = 1;
                int gapBuffer = maxGapSize;
                bool wasDifference;
                while (oribr.BaseStream.Position < oribr.BaseStream.Length && ((wasDifference = oribr.ReadByte() != modbr.ReadByte()) || gapBuffer > 0))
                {
                    length++;
                    gapBuffer = (wasDifference) ? maxGapSize : gapBuffer - 1;
                }
                length -= (ulong)(maxGapSize - gapBuffer);
                //add the patch
                p.Patches.Add(new PatchFootprint(address, length));
            }
            return new List<HackSignature<PatchFootprint>>() { p };
        }
    }
}
