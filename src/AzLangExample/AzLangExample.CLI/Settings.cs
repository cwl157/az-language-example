using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzLangExample.CLI
{
	internal sealed class Settings
	{
		public string AzLanguageServicesKey { get; set; }

		public string AzLanguageEndpoint { get; set; }


        private Settings()
		{
		}

		private static Settings instance = null;

		public static Settings Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Settings();

					LoadSettings();
				}
				return instance;
			}
		}

		private static void LoadSettings()
		{
			var developerConfigString = ReadDeveloperConfig();
			var config = JsonSerializer.Deserialize<DevConfig>(developerConfigString);

			Settings.Instance.AzLanguageServicesKey = config.AzLanguageServicesKey;
			Settings.Instance.AzLanguageEndpoint = config.AzLanguageEndpoint;
		}

        private static string ReadDeveloperConfig()
        {
            var executablePath = AppDomain.CurrentDomain.BaseDirectory;
            var configFile = Path.Combine(executablePath, "config.json");
            if (!File.Exists(configFile))
            {
                throw new InvalidOperationException(@"
Unable to local debug custom page when developerconfig file has not been created.
Create your developer config file under deployment/build/developerconfig.json with your relativity server information");
            }

            using (var fileStream = File.OpenText(configFile))
            {
                return fileStream.ReadToEnd();
            }
        }
    }

    internal class DevConfig
    {
        public string AzLanguageServicesKey { get; set; }

        public string AzLanguageEndpoint { get; set; }

    }
}
