using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScpLauncher
{
    internal class TransferConfig
    {
        public string Username { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string KeyPath { get; set; } = string.Empty;
        public string DownloadLocal { get; set; } = string.Empty;
        public string UploadRemote { get; set; } = string.Empty;
    }

    internal static class ConfigStore
    {
        public static string ConfigDir
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".SCP--");

        private static string LastAliasFile
            => Path.Combine(ConfigDir, "_last_alias");

        public static void EnsureConfigDir()
        {
            if (!Directory.Exists(ConfigDir))
            {
                Directory.CreateDirectory(ConfigDir);
            }
        }

        public static IEnumerable<string> ListAliases()
        {
            EnsureConfigDir();
            if (!Directory.Exists(ConfigDir)) yield break;
            var files = Directory.GetFiles(ConfigDir, "*.cfg", SearchOption.TopDirectoryOnly);
            foreach (var f in files.Select(f => Path.GetFileNameWithoutExtension(f)).OrderBy(n => n, StringComparer.OrdinalIgnoreCase))
                yield return f;
        }

        public static void SetLastAlias(string alias)
        {
            EnsureConfigDir();
            if (string.IsNullOrWhiteSpace(alias)) return;
            File.WriteAllText(LastAliasFile, alias.Trim());
        }

        public static string GetLastAlias()
        {
            EnsureConfigDir();
            if (!File.Exists(LastAliasFile)) return null;
            try
            {
                var content = File.ReadAllText(LastAliasFile).Trim();
                return string.IsNullOrWhiteSpace(content) ? null : content;
            }
            catch { return null; }
        }

        private static string PathFor(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias)) throw new ArgumentException("alias is required", nameof(alias));
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                if (alias.Contains(c)) throw new ArgumentException("alias contains invalid characters", nameof(alias));
            }
            return System.IO.Path.Combine(ConfigDir, alias + ".cfg");
        }

        public static bool Exists(string alias)
        {
            EnsureConfigDir();
            return File.Exists(PathFor(alias));
        }

        public static TransferConfig Load(string alias)
        {
            EnsureConfigDir();
            var file = PathFor(alias);
            var cfg = new TransferConfig();
            if (!File.Exists(file)) return cfg;

            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var rawLine in File.ReadAllLines(file))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith(";")) continue;
                var idx = line.IndexOf('=');
                if (idx <= 0) continue;
                var key = line.Substring(0, idx).Trim();
                var val = line.Substring(idx + 1).Trim();
                dict[key] = val;
            }

            dict.TryGetValue("username", out var username);
            dict.TryGetValue("ip", out var ip);
            dict.TryGetValue("port", out var port);
            dict.TryGetValue("keypath", out var keyPath);
            dict.TryGetValue("downloadlocal", out var downloadLocal);
            dict.TryGetValue("uploadremote", out var uploadRemote);

            cfg.Username = username ?? string.Empty;
            cfg.Ip = ip ?? string.Empty;
            cfg.Port = port ?? string.Empty;
            cfg.KeyPath = keyPath ?? string.Empty;
            cfg.DownloadLocal = downloadLocal ?? string.Empty;
            cfg.UploadRemote = uploadRemote ?? string.Empty;
            return cfg;
        }

        public static void Save(string alias, TransferConfig cfg)
        {
            EnsureConfigDir();
            var file = PathFor(alias);
            var lines = new List<string>
            {
                "# SCP-- configuration",
                "# key=value, lines starting with # or ; are comments",
                $"username={cfg.Username ?? string.Empty}",
                $"ip={cfg.Ip ?? string.Empty}",
                $"port={cfg.Port ?? string.Empty}",
                $"keypath={cfg.KeyPath ?? string.Empty}",
                $"downloadlocal={cfg.DownloadLocal ?? string.Empty}",
                $"uploadremote={cfg.UploadRemote ?? string.Empty}"
            };
            File.WriteAllLines(file, lines);
        }

        public static void Delete(string alias)
        {
            EnsureConfigDir();
            var file = PathFor(alias);
            if (File.Exists(file)) File.Delete(file);
        }

        public static void Rename(string oldAlias, string newAlias)
        {
            EnsureConfigDir();
            var oldPath = PathFor(oldAlias);
            var newPath = PathFor(newAlias);
            if (!File.Exists(oldPath)) return;
            if (File.Exists(newPath)) throw new IOException("目标已存在");
            File.Move(oldPath, newPath);
        }
    }
}
