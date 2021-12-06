using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IConfigService
    {
        bool Set(string key, string value);
        string Get(string key);
        bool Delete(string key);
        Dictionary<string, string> GetList();
    }

    public class ConfigService : IConfigService, ITransientDependency
    {
        private const string EnvironmentVariablePrefix = "AELF_CLI_";
        private readonly List<string> _supportKeys = new List<string> {"endpoint", "account", "password"};
        
        public bool Set(string key, string value)
        {
            return AddConfigToFile(key, value);
        }

        public string Get(string key)
        {
            var configs = GetConfig();
            configs.TryGetValue(key, out var value);
            return value;
        }

        public bool Delete(string key)
        {
            return DeleteConfigFromFile(key);
        }

        public Dictionary<string, string> GetList()
        {
            return GetConfig();
        }

        private string GetConfigPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "aelf","aelfcli.conf");
        }

        private Dictionary<string, string> GetConfig()
        {
            var configs = GetConfigFromFile();
            foreach (var key in _supportKeys)
            {
                if (configs.ContainsKey(key))
                {
                    continue;
                }

                var value = Environment.GetEnvironmentVariable($"{EnvironmentVariablePrefix}{key.ToUpper()}");
                if (!value.IsNullOrWhiteSpace())
                {
                    configs[key] = value;
                }
            }

            return configs;
        }

        private Dictionary<string, string> GetConfigFromFile()
        {
            var configs = new Dictionary<string, string>();
            var path = GetConfigPath();

            if (!File.Exists(path))
            {
                File.Create(path);
                return configs;
            }

            using var sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.IsNullOrWhiteSpace() || line.StartsWith("#"))
                {
                    continue;
                }

                var item = line.Split(" ");
                configs.Add(item[0], item.Last());
            }

            return configs;
        }

        private bool AddConfigToFile(string key, string value)
        {
            var config = GetConfigFromFile();
            if (config.ContainsKey(key))
            {
                return false;
            }
            
            var path = GetConfigPath();
            using var sr = new StreamWriter(path);
            sr.WriteLine($"{key} {value}");
            sr.Close();

            return true;
        }
        
        private bool DeleteConfigFromFile(string key)
        {
            var config = GetConfigFromFile();
            if (!config.ContainsKey(key))
            {
                return false;
            }
            
            var path = GetConfigPath();
            using var sr = new StreamReader(path);
            string line;
            var newContent = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                var lineTrim = line.Trim();
                if (lineTrim.IsNullOrWhiteSpace() || lineTrim.StartsWith("#"))
                {
                    newContent.Add(line);
                    continue;
                }

                var item = lineTrim.Split(" ");
                if (item[0] != key)
                {
                    newContent.Add(line);
                }
            }
            
            File.WriteAllLines(path,newContent);

            return true;
        }
    }
}