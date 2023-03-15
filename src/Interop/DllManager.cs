using System.Reflection;
using System.Runtime.InteropServices;

namespace Cead.Interop;

public static class DllManager
{
    private static readonly string[] _libs = { "Cead.lib" };
    private static bool _isLoaded;

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr SetDllDirectory(string lpFileName);

    public static void LoadCead()
    {
        if (_isLoaded) {
            return;
        }

        string path = Path.Combine(Path.GetTempPath(), $"Cead-{typeof(Yaz0).Assembly.GetName().Version}");

        foreach (var lib in _libs) {
            string dll = Path.Combine(path, lib);
            // Always copy in debug mode
#if DEBUG
            Directory.CreateDirectory(path);
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Cead.Lib.{lib}")!;
            using FileStream fs = File.Create(dll);
            stream.CopyTo(fs);
#else
            if (!File.Exists(dll)) {
                Directory.CreateDirectory(path);
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Cead.Lib.{lib}")!;
                using FileStream fs = File.Create(dll);
                stream.CopyTo(fs);
            }
#endif
        }

        SetDllDirectory(path);
        _isLoaded = true;
    }
}
