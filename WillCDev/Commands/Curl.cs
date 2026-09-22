using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("curl", Description = new string[]
    {
        "- Format:",
        "   curl [url]",
        "- Description:",
        "   Fetches the content of the specified URL.",
        "- Parameters/Flags:",
        "   [url] => The URL to fetch",
    })]
    public class Curl(IHttpClientFactory httpClientFactory) : CommandHandlerBase
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public override async Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            // Implement curl command logic here
            using (var client = _httpClientFactory.CreateClient())
            {
                var url = args.Length > 0 ? args[0] : string.Empty;
                if (!string.IsNullOrEmpty(url))
                {
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    feedController.AddLine(content);
                }
            }

            return;
        }
    }
}