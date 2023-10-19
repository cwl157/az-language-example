using Azure.AI.TextAnalytics;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzLangExample.CLI
{
    internal class EntityExtractor
    {
        public async Task<List<Entity>> GetEntities(string content)
        {
            var credentials = new AzureKeyCredential(Settings.Instance.AzLanguageServicesKey);
            var endpoint = new Uri(Settings.Instance.AzLanguageEndpoint);

            var client = new TextAnalyticsClient(endpoint, credentials);

            var contentParts = SplitStringByLength(content, 5000);
            var response = await client.RecognizeEntitiesBatchAsync(contentParts);
            var collection = response.Value;
            var result = new List<Entity>();
            foreach (var docResponse in response.Value)
            {
                foreach (var entity in docResponse.Entities)
                {
                    result.Add(new Entity()
                    {
                        Text = entity.Text,
                        Type = entity.Category.ToString(),
                        Score = entity.ConfidenceScore,
                        Start = entity.Offset,
                        Length = entity.Length,
                    });
                }
            }

            return result;
        }

        private List<string> SplitStringByLength(string input, int chunkLength)
        {
            if (string.IsNullOrEmpty(input) || chunkLength <= 0)
            {
                throw new ArgumentException("Invalid input or chunk length.");
            }

            List<string> chunks = new List<string>();

            for (int i = 0; i < input.Length; i += chunkLength)
            {
                int length = Math.Min(chunkLength, input.Length - i);
                chunks.Add(input.Substring(i, length));
            }

            return chunks;
        }
    }
}
