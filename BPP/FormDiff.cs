using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrayconnsPatchingFramework;
using SharpDisasm;
using ScintillaNET;

namespace BPP
{
    public partial class FormDiff : Form
    {
        readonly ulong BaseAddress;
        readonly string exePath;
        readonly List<IPatch> Patches;

        IPatch SelectedPatch => Patches[CompareIndex];

        int compareIndex = 0;
        int CompareIndex
        {
            get => compareIndex;
            set
            {
                if (compareIndex != value && 0 <= value && value < Patches.Count)
                {
                    compareIndex = value;
                    InitPatchBuffer();
                    UpdateTitle();
                    UpdatePatchSelectionButtons();
                    DisplayComparison();
                }
            }
        }

        #region buffers
        readonly LinkedList<byte> buffer = new LinkedList<byte>();
        LinkedListNode<byte> preBufferNode, dataNode, postBufferNode;
        IEnumerable<byte> originalData => buffer;
        IEnumerable<byte> newData
        {
            get
            {
                for (var n = preBufferNode; n != null && n != dataNode; n = n.Next)
                    yield return n.Value;
                foreach (var b in SelectedPatch.Data)
                    yield return b;
                for (var n = postBufferNode; n != null; n = n.Next)
                    yield return n.Value;
            }
        }
        #endregion

        #region buffer management

        ulong preBuffer = 0;
        void IncreasePreBuffer()
        {
            preBuffer++;
            using (var br = new BinaryReader(new FileStream(exePath, FileMode.Open, FileAccess.Read)))
            {
                br.BaseStream.Seek((long)(SelectedPatch.Address - BaseAddress - preBuffer), SeekOrigin.Begin);
                buffer.AddFirst(br.ReadByte());
            }
            preBufferNode = buffer.First;
        }
        void DecreasePreBuffer()
        {
            if (preBuffer <= 0)
                return;
            preBuffer--;
            buffer.RemoveFirst();
            preBufferNode = preBuffer > 0 ? buffer.First : null;
        }

        ulong postBuffer = 0;
        void IncreasePostBuffer()
        {
            postBuffer++;
            using (var br = new BinaryReader(new FileStream(exePath, FileMode.Open, FileAccess.Read)))
            {
                br.BaseStream.Seek((long)(SelectedPatch.Address - BaseAddress + SelectedPatch.Length + postBuffer - 1), SeekOrigin.Begin);
                buffer.AddLast(br.ReadByte());
            }
            if (postBuffer == 1)
                postBufferNode = buffer.Last;
        }
        void DecreasePostBuffer()
        {
            if (postBuffer <= 0)
                return;
            postBuffer--;
            buffer.RemoveLast();
            if (postBuffer == 0)
                postBufferNode = null;
        }

        #endregion

        #region View modes
        enum ViewModes
        {
            Hex,
            Text,
            x86,
        }
        ViewModes viewMode = ViewModes.x86;
        ViewModes ViewMode
        {
            get => viewMode;
            set
            {
                if(viewMode != value)
                {
                    viewMode = value;
                    UpdateViewModeButtons();
                    DisplayComparison();
                }
            }
        }

        ArchitectureMode architecture = ArchitectureMode.x86_32;
        ArchitectureMode x86Architecture
        {
            get => architecture;
            set
            {
                if(architecture != value)
                {
                    architecture = value;
                    UpdateArchitectureButtons();
                    DisplayComparison();
                }
            }
        }

        #endregion

        public FormDiff(string exepath, IEnumerable<IPatch> patches, ulong baseAddress)
        {
            exePath = exepath;
            BaseAddress = baseAddress;
            Patches = new List<IPatch>(patches);
            InitPatchBuffer();
            
            InitializeComponent();
            UpdateTitle();

            UpdateViewModeButtons();
            UpdateArchitectureButtons();
            UpdatePatchSelectionButtons();
            UpdateBufferButtons();

            DisplayComparison();
        }

        void UpdateTitle()
        {
            this.Text = string.Format(Dialog.FormDiffTitle, CompareIndex+1, Patches.Count);
        }

        private void InitPatchBuffer()
        {
            buffer.Clear();
            preBufferNode = null;
            dataNode = null;
            postBufferNode = null;
            using (var br = new BinaryReader(new FileStream(exePath, FileMode.Open, FileAccess.Read)))
            {
                ulong start = SelectedPatch.Address - BaseAddress - preBuffer;
                ulong end = start + preBuffer + SelectedPatch.Length + postBuffer;

                br.BaseStream.Seek((long)start, SeekOrigin.Begin);
                while (br.BaseStream.Position != (long)end)
                {
                    buffer.AddLast(br.ReadByte());
                    if (preBuffer > 0 && buffer.Count == 1)
                        preBufferNode = buffer.Last;
                    else if ((ulong)buffer.Count == preBuffer + 1)
                        dataNode = buffer.Last;
                    else if (postBuffer > 0 && (ulong)buffer.Count == preBuffer + SelectedPatch.Length)
                        postBufferNode = buffer.Last;

                }
            }
        }

        private void DisplayComparison()
        {
            DisplayData(originalRichTextBox, originalData);
            DisplayData(newRichTextBox, newData);
        }
        private void DisplayData(Scintilla textbox, IEnumerable<byte> data)
        {
            textbox.ReadOnly = false;
            textbox.ClearAll();
            switch (ViewMode)
            {
                case ViewModes.Hex:
                    textbox.Text = $"0x{(SelectedPatch.Address - preBuffer).ToString("X")}\n";
                    foreach (var b in data)
                        textbox.Text += b.ToString("X2") + ", ";
                    break;
                case ViewModes.Text:
                    textbox.Text = $"0x{(SelectedPatch.Address - preBuffer).ToString("X")}\n";
                    textbox.Text += Encoding.ASCII.GetString(data.ToArray());
                    break;
                case ViewModes.x86:
                    Disassembler.Translator.IncludeAddress = true;
                    Disassembler.Translator.IncludeBinary = true;
                    using (var d = new Disassembler(data.ToArray(), x86Architecture, SelectedPatch.Address - preBuffer))
                    {
                        foreach (var instruction in d.Disassemble())
                        {
                            textbox.Text += instruction + "\n";
                        }
                    }
                    break;
            }
            textbox.ReadOnly = true;
        }

        #region the buttons on the top
        void UpdateViewModeButtons()
        {
            hexToolStripMenuItem.Checked = ViewMode == ViewModes.Hex;
            textToolStripMenuItem.Checked = ViewMode == ViewModes.Text;
            x86ToolStripMenuItem.Checked = ViewMode == ViewModes.x86;
        }
        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMode = ViewModes.Hex;
        }
        
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMode = ViewModes.Text;
        }

        private void x86ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMode = ViewModes.x86;
            //need to do this to close a menuitem with children
            viewToolStripMenuItem.HideDropDown();
        }

        void UpdateArchitectureButtons()
        {
            sixteenbitToolStripMenuItem.Checked = x86Architecture == ArchitectureMode.x86_16;
            thirtytwobitToolStripMenuItem.Checked = x86Architecture == ArchitectureMode.x86_32;
            sixtyfourbitToolStripMenuItem.Checked = x86Architecture == ArchitectureMode.x86_64;
        }

        private void sixteenbitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x86Architecture = ArchitectureMode.x86_16;
        }

        private void thirtytwobitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x86Architecture = ArchitectureMode.x86_32;
        }

        private void sixtyfourbitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x86Architecture = ArchitectureMode.x86_64;
        }
        #endregion

        #region the buttons on the bottom
        
        void UpdatePatchSelectionButtons()
        {
            previousButton.Enabled = CompareIndex > 0;
            nextButton.Enabled = CompareIndex < Patches.Count - 1;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            CompareIndex++;
        }
        private void previousButton_Click(object sender, EventArgs e)
        {
            CompareIndex--;
        }

        private void UpdateBufferButtons()
        {
            preShiftRightButton.Enabled = preBuffer > 0;
            preBufferLabel.Text = preBuffer.ToString();

            postShiftLeftButton.Enabled = postBuffer > 0;
            postBufferLabel.Text = postBuffer.ToString();
        }
        private void preShiftLeftButton_Click(object sender, EventArgs e)
        {
            IncreasePreBuffer();
            UpdateBufferButtons();
            DisplayComparison();
        }
        private void preShiftRightButton_Click(object sender, EventArgs e)
        {
            DecreasePreBuffer();
            UpdateBufferButtons();
            DisplayComparison();
        }

        private void postShiftLeftButton_Click(object sender, EventArgs e)
        {
            DecreasePostBuffer();
            UpdateBufferButtons();
            DisplayComparison();
        }
        private void postShiftRightButton_Click(object sender, EventArgs e)
        {
            IncreasePostBuffer();
            UpdateBufferButtons();
            DisplayComparison();
        }

        #endregion
    }
}
