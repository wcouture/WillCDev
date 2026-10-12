using Microsoft.AspNetCore.Components;
using Shared.Attributes;
using Shared.Models;

namespace WillCDev.Components.Programs.Salt
{
    [Program("Salt Game", iconName: "Registry Editor", shrinkWindowOnLoad: true)]
    public partial class SaltGame : IProgram
    {
        [Parameter]
        public int AppId { get; set; }

        [Parameter]
        public int WindowId { get; set; }
    }
}