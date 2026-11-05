# SaveGameEditor

Small save-game editors for single-player PC games.

## Batman: Arkham Knight – AR Challenge Stars

Pick AR challenges from a list and give them 3 stars, instead of replaying them over and over.

![Steam](https://img.shields.io/badge/platform-Steam%20(PC)-blue)

### Download and run

1. Download the [`ArkhamKnight`](ArkhamKnight) folder. You need both `ArkhamKnightStars.exe` and `ArkhamKnightStars.ps1` in the same folder.
2. **Close the game.**
3. Run `ArkhamKnightStars.exe`.
4. Pick your save slot. The newest slot is selected by default.
5. Tick the challenges you want, then click **Give 3 stars to ticked**.
6. Start the game. If Steam reports a **Cloud sync conflict**, choose the **local** files.

You need Windows with PowerShell 5.1, which comes with Windows 10 and 11. There's nothing to install.

### What it does

| Challenge status | What happens |
|---|---|
| **Played** | Your own results are kept. Each character's entry is set to 3 stars, and the best score or time is raised to the 3-star goal if it's below it. |
| **Unlocked** (not played) | Keeps the record the game wrote and adds one 3-star result for the challenge's default character, taken from a real 100% save. |
| **Locked** (grey) | Experimental. Adds a record built the same way, and you're asked to confirm first. |

### WayneTech upgrade points

The **WayneTech points** box shows your unspent upgrade points for the selected save. Change the number and click **Set points**. Like every other change, your saves are backed up first.

In the save, the points come after the text `- Total` (part of the combat score help text):

```
"- Total\0"  u32 N, N x u32       N is usually 16, but 14 early in a new game
             u32 points           <- WayneTech points
             u32, u32
             u32 K, K x u32       completed training challenge ids (503, 201, ...)
```

Because `N` varies, the position of the points varies too. The tool only writes when the `K` values after the points are real challenge IDs. Older saves without this block show "not found".

### Backups

All backups go in `Documents\ArkhamKnight_SaveBackup`.

- **Original:** the first time the tool runs, it copies your untouched saves to a folder ending in `_ORIGINAL_<steam id>`. It never overwrites this copy.
- **Before every change:** all your save slots are copied to a folder named by date and time.
- **Back up now:** makes an extra copy whenever you like.
- **Restore a backup...:** pick any backup folder to put it back. Your current saves are backed up first.

Steam achievements are **not** unlocked, and leaderboards keep your real scores.

> **Tip:** do a few challenges first, start the game, and only then do the rest. If the game won't load the save, close it and use **Restore a backup...**.

### How it works

Saves are in `Steam\userdata\<id>\208650\remote\BAK1Save<slot>x<n>.sgd`. The game rotates 3 files per slot, and the newest one is the live save. There's no checksum. However, the header `u32` at offset `0x0C` is the size of the section that holds the challenge list. When records are added or grow, it has to change by the same number of bytes, or the game crashes with a "Fatal error" on load.

The AR challenge results are stored as a list of records:

```
u32 count, then per record:
  u8     0x03
  u32    challenge id        (matches Challenge_<id>_SF.upk / ARChallenges.int)
  u8[2]  flags
  u8     N                   (result entries: one per character/vehicle; 0 = not played)
  N x 24 bytes:
    u32 0
    u32 packed               stars << 29 | value   (times: value = 0x20000000 - ms)
    u32 best                 points, or centiseconds for timed challenges
    u32 0, u32 0
    u8  stars, u8 ?, u8 character/vehicle (0 = default), u8 rivals
  u8[7] + u32 tail           last character/vehicle used, or a stamp
```

Each entry belongs to one character or Batmobile. A save loaded with entries for characters or vehicles it hasn't unlocked crashed the game, so the tool only ever adds an entry for the challenge's default character.

Check the save parsing without changing anything:

```powershell
powershell -ExecutionPolicy Bypass -File ArkhamKnight\ArkhamKnightStars.ps1 -SelfTest
```

### Building the exe

`ArkhamKnightStars.exe` is a tiny launcher ([`Launcher.cs`](ArkhamKnight/Launcher.cs)) that runs the script without showing a console window. To rebuild it with the C# compiler that ships with Windows:

```powershell
.\ArkhamKnight\build.ps1
```

### Credits

The reference 3-star records for unlocked and locked challenges come from **"Darny's 240 Save"** on Nexus Mods.

### Disclaimer

Use at your own risk. Back up your saves (the tool does this automatically). This project is not affiliated with Warner Bros. Games or Rocksteady Studios.
