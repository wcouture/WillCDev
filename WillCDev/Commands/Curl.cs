using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("curl")]
    public class Curl(IHttpClientFactory httpClientFactory) : CommandHandlerBase
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public override async Task HandleCommand(List<string> consoleLines, params string[] args)
        {
            // Implement curl command logic here
            using (var client = _httpClientFactory.CreateClient())
            {
                var url = args.Length > 0 ? args[0] : string.Empty;
                if (!string.IsNullOrEmpty(url))
                {
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    consoleLines.Add(content);
                }
            }

            return;
        }
    }
}