using Microsoft.AspNetCore.Components;
using Shared.Attributes;
using Shared.Models;

namespace WillCDev.Components.Programs.DocumentationGuide
{
    [Program("Documentation Guide", "Interactive Documentation Guide for Portfolio XP.",
        iconName:"Address Book", defaultTaskbarIcon: true, defaultDesktopShortcut: true, shrinkWindowOnLoad: true)]
    public partial class DocumentationGuide : IProgram
    {
        [Parameter]
        public int AppId { get; set; }
    }
}
