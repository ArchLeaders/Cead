global using static Cead.Interop.DllManager;

using System.Reflection;
using System.Runtime.InteropServices;

namespace Cead.Interop;

public static class DllManager
{
#if LINUX
    public const string CeadLib = "Cead.so";
#else
    public const string CeadLib = "Cead.lib";
#endif


    private static readonly string[] _libs = { CeadLib };
    private static bool _isLoaded;

    public static void LoadCead()
    {
        if (_isLoaded) {
            return;
        }

        string path = Path.Combine(Path.GetTempPath(), $"Cead-{typeof(Yaz0).Assembly.GetName().Version}");

        foreach (var lib in _libs) {
            string dll = Path.Combine(path, lib);

#if DEBUG
            // Always copy in debug mode
            Directory.CreateDirectory(path);
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Cead.Lib.{lib}")!;
            using (FileStream fs = File.Create(dll)) {
                stream.CopyTo(fs);
            }
#else
            if (!File.Exists(dll)) {
                Directory.CreateDirectory(path);
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Cead.Lib.{lib}")!;
                using (FileStream fs = File.Create(dll)) {
                    stream.CopyTo(fs);
                }
            }
#endif

            NativeLibrary.Load(dll);
        }

        _isLoaded = true;
    }
}
