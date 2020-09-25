using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using LocalizeableComponentModel;

namespace BrayconnsPatchingFramework.BoostersLab
{

    public interface IHasChildren
    {
        object[] Nodes { get; set; }
    }

    public class BoostersLabPanel : IHasChildren, ICloneable
    {
        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_TITLE)]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_COL)]
        public string Column { get; set; }

        [XmlElement(ElementName = BoostersLabHack.NODE_PANEL, Type = typeof(BoostersLabPanel)),
         XmlElement(ElementName = BoostersLabHack.NODE_FIELD, Type = typeof(BoostersLabField))]
        public object[] Nodes { get; set; }

        public object Clone()
        {
            var p = new BoostersLabPanel()
            {
                Title = Title,
                Column = Column,
                Nodes = Nodes?.DeepClone()
            };
            return p;
        }
    }

    #region Checkboxes
    public abstract class BoostersLabCheckContainer : IHasChildren
    {
        [XmlElement(ElementName = BoostersLabHack.NODE_FIELD, Type = typeof(BoostersLabField))]
        public object[] Nodes { get; set; }

        protected object Clone<T>() where T : BoostersLabCheckContainer, new()
        {
            var c = new T()
            {
                Nodes = Nodes?.DeepClone()
            };            
            return c;
        }
    }
    public class BoostersLabChecked : BoostersLabCheckContainer, ICloneable
    {
        public object Clone() => base.Clone<BoostersLabChecked>();
    }
    public class BoostersLabUnchecked : BoostersLabCheckContainer, ICloneable
    {
        public object Clone() => base.Clone<BoostersLabUnchecked>();
    }
    #endregion

    public class BoostersLabCheckbox : ICloneable
    {
        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_NAME)]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_VALUE)]
        public string Value { get; set; }

        [XmlIgnore]
        public bool Checked { get; set; }
        public object Clone()
        {
            var c = new BoostersLabCheckbox()
            {
                Name = Name,
                Value = Value,
            };
            return c;
        }
    }

    public class BoostersLabField : IHasChildren, ICloneable
    {
        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_TYPE)]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_NAME)]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_SIZE)]
        public int Size { get; set; } = 4;

        [XmlIgnore]
        internal bool recognizeLongs = true;
        [XmlIgnore]
        public bool IsNumber => Size == 1 || Size == 2 || Size == 4 || (recognizeLongs && Size == 8);

        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_OFFSET)]
        public string Offset { get; set; }

        [XmlAttribute(AttributeName = BoostersLabHack.ATTR_COL)]
        public string Column { get; set; }

        [XmlElement(ElementName = BoostersLabHack.NODE_CHECKBOX, Type = typeof(BoostersLabCheckbox)),
         XmlElement(ElementName = BoostersLabHack.NODE_CHECKED, Type = typeof(BoostersLabChecked)),
         XmlElement(ElementName = BoostersLabHack.NODE_UNCHECKED, Type = typeof(BoostersLabUnchecked))]
        public object[] Nodes { get; set; }

        [XmlIgnore]
        public string DefaultData { get; internal set; }

        [XmlText]
        public string Data { get; set; }

        public object Clone()
        {
            var f = new BoostersLabField()
            {
                Type = Type,
                Name = Name,
                Size = Size,
                Offset = Offset,
                Column = Column,
                Nodes = Nodes?.DeepClone(),
                Data = Data,
            };
            return f;
        }
    }

    [XmlRoot(ElementName = NODE_HACK)]
    public class BoostersLabHack : IHack, IHasEditor, IHasChildren
    {
        [XmlIgnore]
        bool recognizeLongs = true;
        [XmlIgnore]
        [DefaultValue(true), LocalizeableDescription(nameof(Dialog.RecognizeLongsDescription), typeof(Dialog))]
        public bool RecognizeLongs
        {
            get => recognizeLongs;
            set
            {
                recognizeLongs = value;
                void SetLongValue(object[] nodes)
                {
                    foreach(var node in Nodes)
                    {
                        if (node is BoostersLabPanel blp)
                            SetLongValue(blp.Nodes);
                        else if (node is BoostersLabField blf)
                            blf.recognizeLongs = recognizeLongs;
                    }
                }
                SetLongValue(Nodes);
            }
        }

        #region Constants/Enums
        public const string ATTR_POP_FROM_EXE = "populatefromexe"; //unused?
        public const string ATTR_SIZE = "size";
        public const string ATTR_LENGTH = "length";
        public const string ATTR_OFFSET = "offset";
        public const string ATTR_TYPE = "type";
            public const string FIELDTYPE_TEXT = "text";
            public const string FIELDTYPE_LABEL = "label";
            public const string FIELDTYPE_DATA = "data";
            public const string FIELDTYPE_FLAG = "flags";
            public const string FIELDTYPE_INFO = "info";
            public const string FIELDTYPE_CHECK = "check";
            public const string FIELDTYPE_IMAGE = "image";
        public const string ATTR_NAME = "name";
        public const string ATTR_TITLE = "title";
        public const string ATTR_COL = "col";
        public const string ATTR_VALUE = "value";
        public const string ATTR_DEFVALUE = "defvalue"; //unused?
        public const string ATTR_SRC = "src";

        public const string NODE_PANEL = "panel";
        public const string NODE_FIELD = "field";
        public const string NODE_HACK = "hack";
        public const string NODE_TEXT = "text";
        public const string NODE_CHECKBOX = "checkbox";
        public const string NODE_CHECKED = "checked";
        public const string NODE_UNCHECKED = "unchecked";
        #endregion

        [ReadOnly(true)]
        [XmlAttribute(AttributeName = ATTR_NAME)]
        public string Name { get; set; }

        [ReadOnly(true)]
        [XmlAttribute(AttributeName = "author")] //This field was introduced/popularized by Enlight, and is not part of BL
        public string Author { get; set; }

        [XmlIgnore]
        public string EXE => !string.IsNullOrWhiteSpace(Game) ? Game : Dialog.CaveStory;

        [Browsable(false)]
        [XmlAttribute(AttributeName = "game")] //This field is part of this program, not BL
        public string Game { get; set; }
        
        [Browsable(false)]
        [XmlElement(ElementName = NODE_PANEL, Type = typeof(BoostersLabPanel))]
        public object[] Nodes { get; set; }
        
        public object Clone()
        {
            var h = new BoostersLabHack()
            {
                Name = Name,
                Author = Author,
                Game = Game,
                Nodes = Nodes.DeepClone()
            };
            return h;
        }

        public const string WantsDefaultValuesSetting = "acceptdefaults";
        public const string RecognizeLongsSetting = "recognizelongs";
        public const string FixStreakingSetting = "fixstreaking";
        public void LoadSettings(HackSettings hs)
        {
            if(hs.EXE != EXE)
                throw new ArgumentException(Dialog.InvalidGame, nameof(hs.EXE));
            if (hs.Name != Name)
                throw new ArgumentException(Dialog.InvalidName, nameof(hs.Name));
            foreach(var setting in hs.Settings)
            {
                switch(setting.Key)
                {
                    case RecognizeLongsSetting:
                        RecognizeLongs = (bool)setting.Value;
                        break;
                    case WantsDefaultValuesSetting:
                        WantsDefaultValues = (bool)setting.Value;
                        break;
                    case FixStreakingSetting:
                        FixStreaking = (bool)setting.Value;
                        break;
                    default:
                        object node = this;
                        var indexes = setting.Key.Split(',')
                                                 .Where(x => int.TryParse(x, out _))
                                                 .Select(x => int.Parse(x));
                        foreach (var i in indexes)
                            node = (node as IHasChildren)?.Nodes?[i];

                        if (node is BoostersLabField blf)
                            blf.Data = (string)setting.Value;
                        else if (node is BoostersLabCheckbox blc)
                            blc.Checked = (bool)setting.Value;
                        break;
                }
            }
        }

        public void LoadDefaultValues(string filename, ulong baseAddress)
        {
            using var br = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read));
            void GetDefaults(object[] nodes)
            {
                if (nodes == null)
                    return;
                foreach (var node in nodes)
                {
                    if (node is BoostersLabPanel blp)
                        GetDefaults(blp.Nodes);
                    else if (node is BoostersLabField blf)
                    {
                        if (!string.IsNullOrWhiteSpace(blf.Offset))
                            br.BaseStream.Seek((long)(Convert.ToUInt64(blf.Offset, 16) - baseAddress), SeekOrigin.Begin);
                        switch (blf.Type)
                        {
                            case FIELDTYPE_TEXT:
                                if (blf.IsNumber)
                                {
                                    switch (blf.Size)
                                    {
                                        case 1:
                                            blf.DefaultData = br.ReadByte().ToString();
                                            break;
                                        case 2:
                                            blf.DefaultData = br.ReadInt16().ToString();
                                            break;
                                        case 4:
                                            blf.DefaultData = br.ReadInt32().ToString();
                                            break;
                                        //If we've already passed the IsNumber check above, we'll pass it here too
                                        case 8:
                                            blf.DefaultData = br.ReadInt64().ToString();
                                            break;
                                    }
                                }
                                else
                                {
                                    blf.DefaultData = Encoding.ASCII.GetString(br.ReadBytes(blf.Size)).Replace("\0",null);
                                }
                                break;
                            case FIELDTYPE_FLAG:
                                //TODO make this work with arbitrary sizes?
                                var defaultFlags = br.ReadUInt64();
                                //there's no check before overwriting blc.Checked
                                //because the only way for blc.checked to be True, is for it to have been opened and set that way
                                //this is impossible, because this function would've already exited with the hasOpenedEditor check
                                foreach (BoostersLabCheckbox blc in blf.Nodes)
                                    blc.Checked = (Convert.ToUInt64(blc.Value, 16) & defaultFlags) > 0;
                                break;
                        }
                    }
                }
            }
            GetDefaults(Nodes);
        }
        [XmlIgnore]
        [Browsable(false)]
        public bool WantsDefaultValues { get; private set; } = true;
        
        [XmlIgnore]
        [DefaultValue(false), LocalizeableDescription(nameof(Dialog.FixStreakingDescription), typeof(Dialog))]
        public bool FixStreaking { get; set; }
        private class ExStyleForm : Form
        {
            readonly int ExStyle;
            public ExStyleForm(int exStyle)
            {
                ExStyle = exStyle;
            }
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= ExStyle;
                    return cp;
                }
            }
        }
        public Form GetEditor()
        {
            WantsDefaultValues = false;
            ExStyleForm f = new ExStyleForm(FixStreaking ? 0x02000000 : 0)
            {
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                AutoScroll = true,
                Text = Name,
            };
            f.Controls.Add(new TableLayoutPanel()
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 1,
            });
            void AddControls(TableLayoutPanel parent, object[] nodes)
            {
                if (nodes == null)
                    return;
                int column = 0;
                int row = 0;
                void UpdateColumn(string value)
                {
                    if (int.TryParse(value, out int newcol))
                    {
                        column = newcol;
                        if (parent.ColumnCount <= column)
                            parent.ColumnCount = column + 1;
                        //TODO may need to modify this if a hack is found that reuses a previous column
                        row = 0;
                    }
                }                
                int GetRow()
                {
                    parent.RowCount++;
                    return row++;
                }
                foreach (var node in nodes)
                {
                    if(node is BoostersLabPanel blp)
                    {
                        UpdateColumn(blp.Column);
                        var gb = new GroupBox()
                        {
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowOnly,
                            Margin = new Padding(0,0,0,0),
                            Padding = new Padding(2,0,2,2),
                            //this "removes" the extra space at the top of the group box for cases where there isn't any text
                            Font = (string.IsNullOrWhiteSpace(blp.Title)
                                    ? new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 1, GraphicsUnit.Pixel)
                                    : Control.DefaultFont),
                            Text = blp.Title
                        };
                        if ((blp.Nodes?.Length ?? 0) > 0)
                        {
                            var flp = new TableLayoutPanel()
                            {
                                AutoSize = true,
                                Dock = DockStyle.Fill,
                                RowCount = 1,
                                ColumnCount = 1,
                            };
                            AddControls(flp, blp.Nodes);
                            gb.Controls.Add(flp);
                        }
                        parent.Controls.Add(gb);
                    }
                    else if(node is BoostersLabField blf)
                    {
                        UpdateColumn(blf.Column);
                        switch(blf.Type)
                        {
                            case FIELDTYPE_LABEL:
                                var l = new Label()
                                {
                                    AutoSize = true,
                                    Dock = DockStyle.Fill,
                                    Margin = new Padding(3,2,3,3), //Top & Bottom margin would be 0 without this
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    Font = Control.DefaultFont,
                                    Text = blf.Data?.Replace("\n", null).Replace("\t", null) ?? "",
                                };
                                parent.Controls.Add(l, column, GetRow());
                                break;
                            //Data and info are both just displayed as text boxes
                            case FIELDTYPE_DATA:
                            case FIELDTYPE_INFO:
                                var tb = new RichTextBox()
                                {
                                    WordWrap = true,
                                    Multiline = true,
                                    Dock = DockStyle.Fill,
                                    ScrollBars = RichTextBoxScrollBars.Vertical,
                                    MinimumSize = new Size(200, 80),
                                    //MaximumSize = new Size(200, int.MaxValue),
                                    ReadOnly = true,
                                    Font = Control.DefaultFont,
                                    Text = blf.Data?.Trim('\n').Replace("\n", Environment.NewLine).Replace("\t", null) ?? ""
                                };
                                parent.Controls.Add(tb, column, GetRow());
                                break;
                            case FIELDTYPE_TEXT:
                                if (blf.Data == null)
                                    blf.Data = blf.DefaultData;
                                if (blf.IsNumber)
                                {
                                    var nud = new NumericUpDown()
                                    {
                                        AutoSize = true,
                                        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                                        Minimum = long.MinValue,
                                        Maximum = long.MaxValue,
                                        //use the Data if not null (which can only happen on loading a hack with no value there)
                                        //if it is null, just use whatevers in the default
                                        Value = int.TryParse(blf.Data, out int i) ? i : 0,
                                        DecimalPlaces = 0,
                                        Font = Control.DefaultFont,
                                    };
                                    nud.ValueChanged += (o, e) =>
                                    {
                                        blf.Data = ((NumericUpDown)o).Value.ToString();
                                    };
                                    parent.Controls.Add(nud, column, GetRow());
                                }
                                else
                                {
                                    var txt = new RichTextBox()
                                    {
                                        Dock = DockStyle.Fill,
                                        Multiline = false,
                                        Font = Control.DefaultFont,
                                        Text = blf.Data
                                    };
                                    txt.TextChanged += (o, e) =>
                                    {
                                        blf.Data = ((TextBox)o).Text;
                                    };
                                    parent.Controls.Add(txt, column, GetRow());
                                }
                                break;
                            case FIELDTYPE_FLAG:
                                foreach(BoostersLabCheckbox blc in blf.Nodes)
                                {
                                    var cb = new CheckBox()
                                    {
                                        AutoSize = true,
                                        Anchor = AnchorStyles.None,
                                        Checked = blc.Checked,
                                        Font = Control.DefaultFont,
                                        Text = blc.Name
                                    };
                                    cb.CheckedChanged += (o, e) => { blc.Checked = ((CheckBox)o).Checked; };
                                    parent.Controls.Add(cb, column, GetRow());
                                }
                                break;
                            case FIELDTYPE_CHECK:
                                var c = new CheckBox()
                                {
                                    AutoSize = true,
                                    Anchor = AnchorStyles.None,
                                    Checked = bool.TryParse(blf.Data, out bool b) ? b : false,
                                    Font = Control.DefaultFont,
                                    Text = blf.Name
                                };
                                c.CheckedChanged += (o, e) => { blf.Data = ((CheckBox)o).Checked.ToString(); };
                                parent.Controls.Add(c, column, GetRow());
                                break;
                        }
                    }
                }
            }
            AddControls((TableLayoutPanel)f.Controls[0], Nodes);
            f.MaximumSize = new Size(f.Controls[0].PreferredSize.Width + SystemInformation.VerticalScrollBarWidth * 3,
                                     f.Controls[0].PreferredSize.Height + SystemInformation.HorizontalScrollBarHeight * 3);
            Screen monitor = Screen.FromControl(f);
            f.Height = Math.Min(f.MaximumSize.Height, monitor.Bounds.Height - f.Top);
            f.Width = Math.Min(f.MaximumSize.Width, monitor.Bounds.Width - f.Left);
            return f;
        }

        private HackSettings GetHackSettings()
        {
            var theSettings = new HackSettings(EXE, Name);
            if (!RecognizeLongs)
                theSettings.Settings.Add(new Setting<string, object>(RecognizeLongsSetting, RecognizeLongs));
            if(!WantsDefaultValues)
                theSettings.Settings.Add(new Setting<string, object>(WantsDefaultValuesSetting, WantsDefaultValues));
            if (FixStreaking)
                theSettings.Settings.Add(new Setting<string, object>(FixStreakingSetting, FixStreaking));
            void GetSetting(IList<object> nodes, string chain = "")
            {
                for(int i = 0; i < nodes?.Count; i++)
                {
                    var newChain = chain + $"{i},";
                    if (nodes[i] is BoostersLabField blf)
                        switch(blf.Type)
                        {
                            case FIELDTYPE_TEXT:
                                //if it's a number, it needs to be a number, otherwise just check for empty
                                if (blf.IsNumber ? long.TryParse(blf.Data, out _) : !string.IsNullOrWhiteSpace(blf.Data))
                                    theSettings.Settings.Add(new Setting<string, object>(newChain, blf.Data));
                                break;
                            case FIELDTYPE_FLAG:
                                GetSetting(blf.Nodes, newChain);                                    
                                break;
                            case FIELDTYPE_CHECK:
                                if(bool.TryParse(blf.Data, out bool b) && b)
                                    theSettings.Settings.Add(new Setting<string, object>(newChain, blf.Data));
                                break;
                        }
                    if(nodes[i] is BoostersLabCheckbox blc && blc.Checked)//bools default to false, so don't need to store those
                        theSettings.Settings.Add(new Setting<string, object>(newChain, blc.Checked));
                    //important to check this last, since BoostersLabField inherits from IHasChildren
                    else if (nodes[i] is IHasChildren hc)
                        GetSetting(hc.Nodes, newChain);
                }
            }
            //don't need to save any settings if all that's there is default junk that's going to be overwriteen
            if(!WantsDefaultValues)
                GetSetting(Nodes);
            return theSettings;
        }

        public IPatch[] ToListOfPatches()
        {
            List<Patch> GetPatches(IEnumerable<object> nodes)
            {
                var p = new List<Patch>();
                if (nodes == null)
                    return p;
                foreach (var node in nodes)
                {
                    if (node is BoostersLabPanel blp)
                        p.AddRange(GetPatches(blp.Nodes));
                    else if (node is BoostersLabField blf)
                    {
                        ulong add = 0;
                        //This check is just here for the FIELDTYPE_CHECK type
                        if (!string.IsNullOrWhiteSpace(blf.Offset))
                            add = Convert.ToUInt64(blf.Offset, 16);

                        byte[] data;
                        switch (blf.Type)
                        {
                            case FIELDTYPE_DATA:
                                data = blf.Data.Split(null)
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .Select(x => Convert.ToByte(x, 16))
                                    .ToArray();
                                break;

                            case FIELDTYPE_TEXT:
                                data = new byte[blf.Size];
                                if (!string.IsNullOrWhiteSpace(blf.Data))
                                {
                                    if (blf.IsNumber)
                                         Array.ConstrainedCopy(BitConverter.GetBytes(long.Parse(blf.Data)), 0, data, 0, blf.Size);
                                    else
                                    {
                                        var s = Encoding.ASCII.GetBytes(blf.Data);
                                        //leaving the last byte alone, so the end has a null terminator
                                        Array.ConstrainedCopy(s, 0, data, 0, Math.Min(s.Length, blf.Size - 1));
                                    }
                                }
                                break;

                            case FIELDTYPE_FLAG:
                                ulong bits = 0;
                                foreach (BoostersLabCheckbox cb in blf.Nodes)
                                    if (cb.Checked)
                                        bits |= Convert.ToUInt64(cb.Value, 16);
                                data = BitConverter.GetBytes(bits).Take(blf.Size).ToArray();
                                break;

                            case FIELDTYPE_CHECK:
                                bool check = false;
                                bool.TryParse(blf.Data, out check);
                                //Get the correct node category (either checked or unchecked)
                                if (blf.Nodes.Where(x => check ? x is BoostersLabChecked : x is BoostersLabUnchecked)
                                    //treat as CheckContainer either way
                                    .Single() is BoostersLabCheckContainer checkContainer)
                                    //if it isn't null then add those nodes
                                    p.AddRange(GetPatches(checkContainer.Nodes));
                                //either way, continue
                                continue;

                            //skip everything else
                            default:
                                continue;
                        }
                        p.Add(new Patch(add, data));
                    }
                }
                return p;
            }
            return GetPatches(Nodes).ToArray();
        }

        public HackSignature<IPatch> ToHackInfo()
        {
            return new HackSignature<IPatch>(GetHackSettings(), ToListOfPatches());
        }

        public HackSignature<IPatchFootprint> ToHackHistory()
        {
            return new HackSignature<IPatchFootprint>(GetHackSettings(), ToListOfPatches());
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
