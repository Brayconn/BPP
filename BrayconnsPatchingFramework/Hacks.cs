using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BrayconnsPatchingFramework
{
    #region Interfaces
    public interface IHasEditor
    {
        Form GetEditor();
    }
    public interface IHack : ICloneable
    {
        string Name { get; }

        string EXE { get; }

        HackSignature<IPatchFootprint> ToHackHistory();
        HackSignature<IPatch> ToHackInfo();

        void LoadSettings(HackSettings hs);

        bool WantsDefaultValues { get; }
        void LoadDefaultValues(string filename, ulong baseAddress);
    }
    #endregion

    public struct Setting<K,V>
    {
        [XmlElement]
        public K Key;
        [XmlElement]
        public V Value;

        public Setting(K k, V v)
        {
            Key = k;
            Value = v;
        }
    }

    /// <summary>
    /// How a hack was set up before applying
    /// </summary>
    public sealed class HackSettings
    {
        [XmlAttribute]
        public string EXE { get; set; } = null;

        [XmlAttribute]
        public string Name { get; set; } = null;

        [XmlElement(ElementName = "Setting", Type = typeof(Setting<string, object>))]
        public List<Setting<string, object>> Settings { get; set; } = new List<Setting<string, object>>();

        public HackSettings(string game, string name, params Setting<string, object>[] settings)
        {
            EXE = game;
            Name = name;
            Settings.AddRange(settings);
        }

        //Pleasing the XML serializer...
        private HackSettings() { }
    }

    /// <summary>
    /// The signature of one hack, containing either the patches or the patch history
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HackSignature<T> where T : IPatchFootprint
    {
        [XmlElement(Type = typeof(HackSettings))]
        public HackSettings Settings { get; set; } = null;

        [XmlArray]
        [XmlArrayItem(Type = typeof(PatchFootprint))]
        public List<T> Patches { get; set; } = new List<T>();

        public HackSignature(HackSettings settings, params T[] patches)
        {
            Settings = settings;
            foreach (var patch in patches)
                Patches.Add(patch);
        }

        //Pleasing the XML serializer...
        private HackSignature() { }
    }
    public sealed class HackFootprint : IHack
    {
        public string Name => settings.Name;

        public string EXE => settings.EXE;

        public object Clone() => throw new NotSupportedException();

        public void LoadSettings(HackSettings hi) { }

        [System.ComponentModel.Browsable(false)]
        public bool WantsDefaultValues => false;
        public void LoadDefaultValues(string filename, ulong baseAddress) { }

        private HackSettings settings;
        private IPatchFootprint[] patches;

        public HackSignature<IPatch> ToHackInfo() => throw new NotSupportedException();

        public HackSignature<IPatchFootprint> ToHackHistory()
        {
            return new HackSignature<IPatchFootprint>(settings, patches);
        }

        public HackFootprint(HackSettings hs, params IPatchFootprint[] p)
        {
            settings = hs;
            patches = p;
        }
        public static HackFootprint Create<T>(HackSignature<T> missingHack) where T : IPatchFootprint
        {
            IPatchFootprint[] p = new IPatchFootprint[missingHack.Patches.Count];
            missingHack.Patches.ToArray().CopyTo(p,0);
            return new HackFootprint(missingHack.Settings, p);
        }

        public override string ToString()
        {
            return string.Format(Dialog.UnavailableHack, this.Name);
        }
    }
}
