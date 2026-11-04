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
| **Unlocked** (not played) | Gets a 3-star record copied from a real 100% save. |
| **Locked** (grey) | Gets the same copied record, and you're asked to confirm first. |

- Before every change, all your saves are backed up to `Documents\ArkhamKnight_SaveBackup\<date_time>`.
- **Restore a backup...** puts any of those backups back.
- Steam achievements are **not** unlocked, and leaderboards keep your real scores.

> **Tip:** try a few challenges first, start the game, and then do the rest. If the game won't load a save, use **Restore a backup...**.

### How it works

Saves are in `Steam\userdata\<id>\208650\remote\BAK1Save<slot>x<n>.sgd`. The game rotates 3 files per slot, and the newest one is the live save. There's no checksum.

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
    u8  stars, u8[3]
  u8[7] + u32 tail           last character/vehicle used, or a stamp
```

The tail is specific to each challenge and character, so building records by hand for challenges you haven't played can crash the game on load. That's why unlocked and locked challenges get records the game itself wrote, taken from a complete save.

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
