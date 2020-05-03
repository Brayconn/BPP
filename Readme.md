# Brayconn's Patching Program
It's basically a stand-alone version of the hackinator. You can use it to patch exes.

# How to use
There's only two windows in BPP:

## The main window
This is where you do all the stuff.

Choose your hack folder using the box in the top left, choose the exe you want to patch in the top right.

BPP supports two kinds of hacks:
- Booster's Lab xmls
  - Download [Booster's Lab](https://github.com/taedixon/boosters-lab/releases) to get a better idea of what these look like
- "Hex Patch" txt files
  - It's just a hex address on one line, and then a bunch of space seperated bytes
  - See the regex at the top of [`BrayconnsPatchingFramework/HexPatch.cs`](BrayconnsPatchingFramework/HexPatch.cs) for more details

The loaded hacks will appear in the left list, queued hacks appear in the center list, and the currently selected hack in the queue will appear in property grid on the right.

To add a hack to the queue, either drag/drop it or double click on it. Drag/drop the other way, or double click, to remove.

Reload re-scans the selected hack folder, Apply and Undo are pretty self explanitory, Edit lets you edit some hacks using a GUI interface, and Preview opens the preview window.

## The preview window
This is where you can preview what's about to happen to your exe.
That said, I wouldn't recommend using this view to help you make hacks.

The box on the left shows the original contents, and the box on the right shows the new contents.

Three different views are available:
- Hex/Bytes
- ASCII Strings
- x86 Assembly (16, 32, and 64 bit varients)

The two sets of `<<` `>>` buttons let you increase the amount of bytes shown _other_ than the patched bytes.
The left set of arrows will increase the amount of bytes shown before the hack, while the second set will increase the amount shown after.
These bytes are all from the original exe, and will show up in _both_ views.
So no, your hacks are not magically expanding, it's just so you can better see what's going on.


# Building
It's just a regular VS2019 project, should build pretty normally.

Uses these NuGet packages:
- CommandLineParser - startup options
- ScintillaNET - Text boxes in the preview window
- SharpDisasm - x86 support in preview window

# Running
On Windows you can just click the exe.
Dragging/dropping an exe will load it as the selected exe, dragging a folder will use it as the hack folder.

Mac/Linux you'll need [mono](https://www.mono-project.com/), and probably need to open it with `mono BPP.exe`

# Credits
- Code - Brayconn<sup><sup>hi<sup/><sup/>
- Booster's Lab + its xml format - [Noxid](https://github.com/taedixon)