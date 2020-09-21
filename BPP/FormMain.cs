using BPP.Properties;
using BrayconnsPatchingFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BPP
{
    public partial class FormMain : Form
    {
        public FormMain(string hackFolder = null, string executable = null, ulong baseaddress = PatchApplier.DefaultBaseAddress, string loadFile = null)
        {
            InitializeComponent();
            UpdateDisplayModeButtons();

            HackFolder = !string.IsNullOrWhiteSpace(hackFolder) ? hackFolder : Settings.Default.HackFolder;
            LoadHacks();

            baseAddress = baseaddress;
            if (!string.IsNullOrWhiteSpace(executable))
                selectedEXE = executable;
            //an attempt to validate file paths was made
            //but it turns out that's a deep rabbit hole that I don't want to go down
            if (!string.IsNullOrWhiteSpace(loadFile))
            {
                //setting openedPathHistory no matter what
                //so if you use -l to open a file that doesn't exist
                //it will offer to save it there for you when you apply
                openedPatchHistory = loadFile;
                if (File.Exists(openedPatchHistory))
                    LoadPatchHistory(openedPatchHistory);
            }
            UpdateActionButtons();
            UpdateEditButtons();
        }

        static readonly string EXEFilter = Dialog.EXEFilter + " (*.exe)|*.exe";
        static readonly string AllFilesFilter = Dialog.AllFilesFilter + " (*.*)|*.*";
        static readonly string PatchHistoryFilter = Dialog.PatchHistoryFilter + " (*.ph)|*.ph";

        ulong baseAddress
        {
            get => (ulong)baseAddressNumericUpDown.Value;
            set => baseAddressNumericUpDown.Value = value;
        }

        string HackFolder
        {
            get
            {
                return Directory.Exists(hackDirectoryTextBox.Text) ? hackDirectoryTextBox.Text: null;
            }
            set
            {
                if(hackDirectoryTextBox.Text != value && Directory.Exists(value))
                {
                     Settings.Default.HackFolder = hackDirectoryTextBox.Text = value;
                }
            }
        }

        bool allowMismatchedBaseAddress = false;
        string selectedEXE
        {
            get
            {
                return File.Exists(selectedEXETextBox.Text) ? selectedEXETextBox.Text : null;
            }
            set
            {
                if (value != selectedEXETextBox.Text && File.Exists(value))
                {
                    allowMismatchedBaseAddress = false;
                    selectedEXETextBox.Text = value;
                    foreach (var hack in queuedHacks)
                        LoadDefaultValues(hack);
                    UpdateActionButtons();
                    UpdateEditButtons();
                }
            }
        }

        IEnumerable<IHack> queuedHacks
        {
            get
            {
                foreach (var hack in queuedHacksListBox.Items)
                    yield return hack as IHack;
            }
        }

        IHack highlightedHack { get => queuedHacksListBox.SelectedItem as IHack; set => queuedHacksListBox.SelectedItem = value; }

        private IEnumerable<HackSignature<IPatchFootprint>> QueuedHackFootprints()
        {
            foreach (var item in queuedHacks)
            {
                yield return item.ToHackHistory();
            }
        }

        private IEnumerable<HackSignature<IPatch>> QueuedHackInfos()
        {
            foreach (var item in queuedHacks)
            {
                HackSignature<IPatch> info;
                try
                {
                    info = item.ToHackInfo();
                }
                catch (NotSupportedException)
                {
                    continue;
                }
                yield return info;
            }
        }

        #region Hack loading/init

        readonly List<IHack> loadedHacks = new List<IHack>();
        readonly List<string[]> hackPaths = new List<string[]>();
        string GetHackFullPath(int index)
        {
            string path = HackFolder + Path.DirectorySeparatorChar + string.Join(Path.DirectorySeparatorChar.ToString(), hackPaths[index]);
            
            //slightly hacky way of dealing with hacks that are inside other files.
            //Currently only used for dll hacks, but maybe could be useful for hacks in archives?
            while (!File.Exists(path))
                path = Path.GetDirectoryName(path);

            return path;
        }
        string GetHackFullPath(IHack hack)
        {
            var found = loadedHacks.Find(x => hack.Name == x.Name && hack.EXE == x.EXE);
            return GetHackFullPath(loadedHacks.IndexOf(found));
        }
        string GetHackFullPath(TreeNode node)
        {
            if (node.Nodes.Count == 0)
            {
                return GetHackFullPath(int.Parse(node.Name));
            }
            else
            {
                var nodes = new List<string>();
                do
                {
                    nodes.Add(node.Name);
                    node = node.Parent;
                } while (node != null) ;

                var output = HackFolder;
                for (int i = nodes.Count - 1; 0 <= i; i--)
                    output += Path.DirectorySeparatorChar + nodes[i];
                
                return output;
            }
        }

        /// <summary>
        /// Load all hacks from the hack folder into loadedHacks
        /// </summary>
        private void LoadHacks()
        {
            loadedHacks.Clear();
            hackPaths.Clear();
            if (HackFolder == null)
                return;
            foreach (var file in Directory.EnumerateFiles(HackFolder, "*.*", SearchOption.AllDirectories))
            {
                IHack hack = null;
                switch (Path.GetExtension(file).ToLowerInvariant())
                {
                    case ".xml":
                        using (StreamReader sr = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read)))
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(BrayconnsPatchingFramework.BoostersLab.BoostersLabHack));
                            try
                            {
                                hack = (BrayconnsPatchingFramework.BoostersLab.BoostersLabHack)xs.Deserialize(sr);
                            }
                            catch
                            {

                            }                            
                        }
                        break;
                    case ".txt":
                        try
                        {
                            hack = BrayconnsPatchingFramework.HexPatch.HexPatch.Parse(file);
                        }
                        catch (FileLoadException)
                        {

                        }
                        break;
                    case ".dll":
                        var dll = Assembly.LoadFile(file);
                        foreach(var type in dll.GetExportedTypes())
                        {
                            if (typeof(IHack).IsAssignableFrom(type))
                            {
                                IHack instance;
                                try
                                {
                                    instance = (IHack)Activator.CreateInstance(type);
                                }
                                catch (TypeInitializationException)
                                {
                                    //Unable to create instance, so I guess we just try the next one?
                                    continue;
                                }
                                loadedHacks.Add(instance);
                                hackPaths.Add(file.Replace(HackFolder + Path.DirectorySeparatorChar, null)
                                      .Split(Path.DirectorySeparatorChar)
                                      .Concat(new[] { type.Name })
                                      .ToArray());
                            }
                        }
                        break;
                }
                if (hack != null)
                {
                    loadedHacks.Add(hack);
                    //this Replace() seems a little hacky, but it should be safe given the circumstances
                    hackPaths.Add(file.Replace(HackFolder + Path.DirectorySeparatorChar,null)
                                      .Split(Path.DirectorySeparatorChar));
                }
            }
        }

        void UpdateDisplayModeButtons()
        {
            hackNamesToolStripMenuItem.Checked = FileDisplayMode == FileDisplayModes.Name;
            hackFilenamesToolStripMenuItem.Checked = FileDisplayMode == FileDisplayModes.Filename;

            byEXEToolStripMenuItem.Checked = TreeDisplayMode == TreeDisplayModes.EXE;
            byDirectoryToolStripMenuItem.Checked = TreeDisplayMode == TreeDisplayModes.Directory;
        }

        enum FileDisplayModes
        {
            Name,
            Filename
        }
        FileDisplayModes fdm = FileDisplayModes.Filename;
        FileDisplayModes FileDisplayMode
        {
            get => fdm;
            set
            {
                if(fdm != value)
                {
                    fdm = value;
                    UpdateDisplayModeButtons();
                    DisplayHacks();
                }
            }
        }
        
        enum TreeDisplayModes
        {
            EXE,
            Directory
        }
        TreeDisplayModes tdm = TreeDisplayModes.Directory;
        TreeDisplayModes TreeDisplayMode
        {
            get => tdm;
            set
            {
                if (tdm != value)
                {
                    tdm = value;
                    UpdateDisplayModeButtons();
                    DisplayHacks();
                }
            }
        }

        private void DisplayHacks()
        {
            availableHacksTreeView.Nodes.Clear();

            for (int i = 0; i < loadedHacks.Count; i++)
            {
                var node = availableHacksTreeView.Nodes;
                switch (TreeDisplayMode)
                {
                    case TreeDisplayModes.EXE:
                        string exe = loadedHacks[i].EXE ?? Dialog.DefaultEXEDisplayName;
                        if (!node.ContainsKey(exe))
                            node = node.Add(exe, exe).Nodes;
                        else
                            node = node[exe].Nodes;
                        break;
                    case TreeDisplayModes.Directory:
                        for (int j = 0; j < hackPaths[i].Length - 1; j++)
                        {
                            var dir = hackPaths[i][j];
                            if (!node.ContainsKey(dir))
                                node.Add(dir, dir);
                            node = node[dir].Nodes;
                        }
                        break;
                }
                //TODO switch to C#8 so I can clean up this mess
                string name = "";
                switch (FileDisplayMode)
                {
                    case FileDisplayModes.Name:
                        name = loadedHacks[i].Name;
                        break;
                    case FileDisplayModes.Filename:
                        name = hackPaths[i][hackPaths[i].Length - 1];
                        break;
                }
                node.Add(i.ToString(), name);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrWhiteSpace(HackFolder))
            {
                MessageBox.Show("No hack folder has been set! Forcing you to choose one now! >:3");
                SelectHackFolder();
            }
            //*/
            DisplayHacks();
        }

        void ReloadAvailableHacks()
        {
            //The ToArray/ToList is very important here
            //without it, it will try to iterate through the queuedHacks list down...
            var prevHacks = queuedHacks.Select(x => x.ToHackHistory()).ToArray();
            queuedHacksListBox.Items.Clear();

            LoadHacks();

            //...here, which by this point is empty
            RestoreQueuedHacks(prevHacks);
            DisplayHacks();
            UpdateActionButtons();
            UpdateEditButtons();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ReloadAvailableHacks();
        }

        #endregion

        private void ShowContextMenu(Point location, string path, bool enableOpenWith = true)
        {
            var cms = new ContextMenuStrip();
            cms.Items.Add(new ToolStripMenuItem("Open", null, delegate { ShellOpener.Open(path); }));
            
            //TODO haven't figured out a way for Open With to work on non-windows platforms
            if (enableOpenWith && Environment.OSVersion.Platform == PlatformID.Win32NT)
                cms.Items.Add(new ToolStripMenuItem("Open with", null, delegate { ShellOpener.OpenFileWith(path, this.Handle); }));

            cms.Items.Add(new ToolStripMenuItem("Open path", null, delegate { ShellOpener.OpenPath(path); }));
            cms.Show(location);
        }

        /// <summary>
        /// Updates the Save, Save As, Generate Patch History File, Apply, and Undo buttons
        /// </summary>
        private void UpdateActionButtons()
        {
            var anyHacks = queuedHacks.Any();
            var exeSelected = !string.IsNullOrWhiteSpace(selectedEXE);

            saveToolStripMenuItem.Enabled = saveAsToolStripMenuItem.Enabled = anyHacks;
            generatePatchHistoryFileToolStripMenuItem.Enabled = exeSelected;
            if (applyButton.Enabled = undoButton.Enabled = anyHacks && exeSelected)
            { 
                //checking that the queued hacks list can actually be applied
                foreach (var hack in queuedHacks)
                {
                    if (hack.GetType() == typeof(HackFootprint))
                    {
                        applyButton.Enabled = false;
                        return;
                    }
                }
            }
        }

        IHack lastCheckedHack = null;
        private void UpdateEditButtons()
        {
            editHackButton.Enabled = highlightedHack is IHasEditor;
            bool canPreview = highlightedHack != null && !string.IsNullOrWhiteSpace(selectedEXE);
            if(canPreview && highlightedHack != lastCheckedHack)
            {
                lastCheckedHack = highlightedHack;
                try
                {
                    canPreview = highlightedHack.ToHackInfo().Patches.Count > 0;
                }
                catch (NotSupportedException)
                {
                    canPreview = false;
                }
            }
            previewButton.Enabled = canPreview;
        }

        private void availableHacksTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                //don't want to allow opening the context menu on exes
                if (e.Node.Nodes.Count > 0 && TreeDisplayMode == TreeDisplayModes.EXE)
                    return;
                ShowContextMenu(availableHacksTreeView.PointToScreen(e.Location),GetHackFullPath(e.Node), e.Node.Nodes.Count == 0);
            }
        }

        private void availableHacksTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Don't add exes/folders to the list...
            if (e.Node.Nodes.Count > 0)
                return;
            AddHack(e.Node);
        }

        void RemoveHack(object hack)
        {
            var index = queuedHacksListBox.Items.IndexOf(hack);
            queuedHacksListBox.Items.Remove(hack);
            queuedHacksListBox.SelectedIndex = Math.Min(index, queuedHacksListBox.Items.Count - 1);

            UpdateActionButtons();
            UpdateEditButtons();
        }

        void AddHack(TreeNode tn, int index = -1)
        {
            var hackToAdd = (IHack)loadedHacks[int.Parse(tn.Name)].Clone();
            LoadDefaultValues(hackToAdd);
            if (index < 0)
            {
                queuedHacksListBox.Items.Add(hackToAdd);
            }
            else
            {
                queuedHacksListBox.Items.Insert(Math.Min(index, queuedHacksListBox.Items.Count), hackToAdd);
            }
            highlightedHack = hackToAdd;
            UpdateActionButtons();
            UpdateEditButtons();
        }

        private void queuedHacksListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedHackPropertyGrid.SelectedObject = queuedHacksListBox.SelectedItem;
            UpdateEditButtons();
        }

        private void queuedHacksListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RemoveHack(queuedHacksListBox.SelectedItem);
        }

        #region Drag & Drop

        bool isHolding = false;
        int startIndex = -1;
        int endIndex = -1;

        bool IsIHack(IDataObject data)
        {
            return data.GetFormats().Any(x => data.GetData(x) is IHack);
        }
        IHack GetAsIHack(IDataObject data)
        {
            return data.GetFormats().Select(x => data.GetData(x) as IHack).FirstOrDefault();
        }

        private void queuedHacksListBox_MouseDown(object sender, MouseEventArgs e)
        {
            var clickedIndex = queuedHacksListBox.IndexFromPoint(e.Location);
            if (clickedIndex == -1)
                return;
            switch (e.Button)
            {
                case MouseButtons.Left when !isHolding:
                    startIndex = clickedIndex;
                    isHolding = true;
                    break;
                case MouseButtons.Right:
                    ShowContextMenu(queuedHacksListBox.PointToScreen(e.Location), GetHackFullPath(highlightedHack));
                    break;
            }
        }

        private void queuedHacksListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isHolding)
            {
                queuedHacksListBox.SelectedIndex = startIndex;
                endIndex = queuedHacksListBox.IndexFromPoint(e.Location);
                if (startIndex != endIndex)
                    DoDragDrop(queuedHacksListBox.Items[startIndex], DragDropEffects.Move);
            }
        }

        private void queuedHacksListBox_MouseUp(object sender, MouseEventArgs e)
        {
            isHolding = false;
        }

        private void queuedHacksListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (IsIHack(e.Data))
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void queuedHacksListBox_DragOver(object sender, DragEventArgs e)
        {
            //TODO put visual indicator of where you're going to insert
        }

        private void queuedHacksListBox_DragDrop(object sender, DragEventArgs e)
        {
            int GetIndex()
            {
                var p = queuedHacksListBox.PointToClient(new Point(e.X, e.Y));
                int index = queuedHacksListBox.IndexFromPoint(p);
                //default to adding to the end of the list
                if (index < 0)
                    index = queuedHacksListBox.Items.Count;
                return index;
            }
            if (e.Data.GetDataPresent(typeof(TreeNode)) && (e.AllowedEffect | DragDropEffects.Copy) != 0)
            {
                var hackToAdd = (TreeNode)e.Data.GetData(typeof(TreeNode));
                int index = GetIndex();
                AddHack(hackToAdd, index);
                queuedHacksListBox.SelectedIndex = index;
            }
            else if (IsIHack(e.Data) && (e.AllowedEffect | DragDropEffects.Move) != 0 && queuedHacksListBox.Items.Count > 1)
            {
                int index = Math.Min(queuedHacksListBox.Items.Count - 1, GetIndex());
                var hackToMove = GetAsIHack(e.Data);
                queuedHacksListBox.Items.Remove(hackToMove);
                queuedHacksListBox.Items.Insert(index, hackToMove);

                isHolding = false;
                queuedHacksListBox.SelectedIndex = index;
            }
        }

        private void availableHacksTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var tn = (TreeNode)e.Item;
            availableHacksTreeView.SelectedNode = tn;
            DoDragDrop(tn, tn.Nodes.Count <= 0 ? DragDropEffects.Copy : DragDropEffects.None);
        }

        private void availableHacksTreeView_DragEnter(object sender, DragEventArgs e)
        {
            if (IsIHack(e.Data))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        private void availableHacksTreeView_DragDrop(object sender, DragEventArgs e)
        {
            if (IsIHack(e.Data) && (e.AllowedEffect | DragDropEffects.Move) != 0)
            {
                RemoveHack(GetAsIHack(e.Data));
                isHolding = false;
            }
        }

        #endregion

        private void selectEXEButton_Click(object sender, EventArgs e)
        {
            using(var ofd = new OpenFileDialog()
            {
                Title = Dialog.SelectEXETitle,
                Filter = string.Join("|", EXEFilter, AllFilesFilter)
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    selectedEXE = ofd.FileName;
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if(!allowMismatchedBaseAddress && PETools.PEFile.TryGetBaseAddress(selectedEXE, out ulong actualBase) && actualBase != baseAddress)
            {
                var res = MessageBox.Show(string.Format(Dialog.GetBaseAddressDecision, "0x" + baseAddress.ToString("X"), "0x" + actualBase.ToString("X")), Dialog.WarningTitle, MessageBoxButtons.YesNoCancel);
                switch(res)
                {
                    case DialogResult.Yes:
                        baseAddress = actualBase;
                        break;
                    case DialogResult.No:
                        allowMismatchedBaseAddress = true;
                        break;
                    default:
                        return;
                }
            }

            var infos = QueuedHackInfos();
            var collisions = PatchApplier.CheckHackCollisions(infos.ToList());
            string collisionWarningMessage = "";
            if (collisions.Count > 0)
            {
                //TODO collisions shouldn't just be displayed here
                collisionWarningMessage += Dialog.HackCollisionWarning + "\n" +
                    string.Join("\n", collisions.Select(x => $"\"{x.Item1.Settings.Name}\" - \"{x.Item2.Settings.Name}\"")) + "\n";
            }
            if(MessageBox.Show(collisionWarningMessage + Dialog.GetApplyConfirmation, Dialog.WarningTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PatchApplier.ApplyHacks(selectedEXE, infos, baseAddress);
                string confirmationText = Dialog.ApplySuccess + "\n" +
                    (!string.IsNullOrWhiteSpace(openedPatchHistory)
                    ? string.Format(Dialog.SaveOffer, openedPatchHistory)
                    : Dialog.CreateSaveOffer);
                if (MessageBox.Show(confirmationText, this.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
            }
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            new FormDiff(selectedEXE, highlightedHack.ToHackInfo().Patches, baseAddress).ShowDialog();
        }

        private string GetOtherEXE()
        {
            string originalFile = "";
            do
            {
                using (OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = Dialog.SelectOriginalEXETitle,
                    Filter = string.Join("|", EXEFilter, AllFilesFilter)
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        originalFile = ofd.FileName;
                    else
                        return null;
                }
                if (originalFile == selectedEXE)
                    MessageBox.Show(Dialog.InvalidOriginalEXE, Dialog.ErrorTitle);
            }
            while (originalFile == selectedEXE);
            return originalFile;
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            string originalFile = GetOtherEXE();
            if (string.IsNullOrWhiteSpace(originalFile))
                return;
            if (MessageBox.Show("Are you sure you want to undo these hacks?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PatchApplier.UndoHacks(selectedEXE, originalFile, QueuedHackFootprints(), baseAddress);
                MessageBox.Show("Hacks undone successfully!", this.Text);
            }
        }

        #region Save/Save As
        string openedPatchHistory = "";
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = Dialog.SaveTitle,
                Filter = string.Join("|", PatchHistoryFilter, AllFilesFilter)
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                    openedPatchHistory = sfd.FileName;
                else
                    return;
            }
            saveToolStripMenuItem_Click(sender, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(openedPatchHistory))
                saveAsToolStripMenuItem_Click(sender, e);
            else if (queuedHacks.Any())
            {
                try
                {
                    PatchApplier.SavePatchHistory(QueuedHackFootprints(), openedPatchHistory);
                }
                catch (ArgumentException ae)
                {
                    MessageBox.Show(ae.Message, this.Text);
                }
            }
        }
        #endregion

        void LoadDefaultValues(IHack hack)
        {
            if (hack.WantsDefaultValues && selectedEXE != null)
            {
                try
                {
                    hack.LoadDefaultValues(selectedEXE, baseAddress);
                }
                catch (IOException)
                {
                    //ignore any IO errors just in case...
                }
            }
        }

        void RestoreQueuedHacks<T>(IEnumerable<HackSignature<T>> hacks) where T : IPatchFootprint
        {
            foreach (var appliedPatch in hacks)
            {
                var foundHack = loadedHacks.Where(x => x.EXE == appliedPatch.Settings.EXE
                                                    && x.Name == appliedPatch.Settings.Name);

                IHack hackToAdd = foundHack.Any()
                    ? (IHack)foundHack.First().Clone()
#if !CRASH_ON_UNKNOWN_HACK
                    : HackFootprint.Create(appliedPatch);
#else              
                    : throw new FileNotFoundException(Dialog.UnableToLocateHack, appliedPatch.Settings.Name);
#endif
                hackToAdd.LoadSettings(appliedPatch.Settings);
                LoadDefaultValues(hackToAdd);

                queuedHacksListBox.Items.Add(hackToAdd);
            }
        }

        void LoadPatchHistory(string file, bool clearQueue = true)
        {
            if(clearQueue)
                queuedHacksListBox.Items.Clear();
            var loadedHistory = PatchApplier.LoadPatchHistory(file);
            RestoreQueuedHacks(loadedHistory);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = Dialog.OpenTitle,
                Filter = string.Join("|", PatchHistoryFilter, AllFilesFilter)
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    openedPatchHistory = ofd.FileName;
                else
                    return;
            }
            LoadPatchHistory(openedPatchHistory);
        }

        private void editHackButton_Click(object sender, EventArgs e)
        {
            if (highlightedHack is IHasEditor hh)
            {
                Form editor;
#if !DEBUG
                try
                {
#endif
                    editor = hh.GetEditor();
                    editor.Icon = this.Icon;
#if !DEBUG
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Dialog.GetEditorError, ex.Message), Dialog.ErrorTitle, MessageBoxButtons.OK);
                    return;
                }
#endif
                editor.ShowDialog();
            }
        }

        private void changeHackFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (queuedHacks.Any() &&
                MessageBox.Show(Dialog.ChangeHackFolderConfirmation, Dialog.WarningTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            using (var fbd = new FolderBrowserDialog()
            {
                Description = Dialog.SelectHackFolderTitle,
                SelectedPath = HackFolder
            })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    HackFolder = fbd.SelectedPath;
                    Settings.Default.Save();
                    ReloadAvailableHacks();
                }
            }            
        }

        private void generatePatchHistoryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Dialog.GeneratePatchHistoryWarning, Dialog.WarningTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            string originalFile = GetOtherEXE();
            if (originalFile == null)
                return;
            using(var sfd = new SaveFileDialog()
            {
                Title = Dialog.GeneratePatchHistoryTitle,
                Filter = string.Join("|", PatchHistoryFilter, AllFilesFilter)
            })
            {
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    PatchApplier.SavePatchHistory(PatchApplier.GeneratePatchHistory(originalFile, selectedEXE), sfd.FileName);
                    MessageBox.Show(Dialog.GeneratePatchHistorySuccess, this.Text);
                }
            }
        }

        private void byDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeDisplayMode = TreeDisplayModes.Directory;
        }

        private void byEXEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeDisplayMode = TreeDisplayModes.EXE;
        }

        private void hackNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDisplayMode = FileDisplayModes.Name;
        }

        private void hackFilenamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDisplayMode = FileDisplayModes.Filename;
        }
    }
}
