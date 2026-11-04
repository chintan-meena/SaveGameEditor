// Starts ArkhamKnightStars.ps1 (next to this exe) without a console window.
// Build: .\build.ps1
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyTitle("Arkham Knight AR Challenge Stars")]
[assembly: AssemblyProduct("SaveGameEditor")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

static class Launcher
{
    [STAThread]
    static int Main()
    {
        string dir = AppDomain.CurrentDomain.BaseDirectory;
        string script = Path.Combine(dir, "ArkhamKnightStars.ps1");
        if (!File.Exists(script))
        {
            MessageBox.Show("ArkhamKnightStars.ps1 must be in the same folder as this program:\n\n" + dir,
                "Arkham Knight Stars", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
        var psi = new ProcessStartInfo("powershell.exe",
            "-NoProfile -STA -ExecutionPolicy Bypass -WindowStyle Hidden -File \"" + script + "\"");
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        Process.Start(psi);
        return 0;
    }
}
