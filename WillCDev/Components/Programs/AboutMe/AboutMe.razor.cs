using Microsoft.AspNetCore.Components;
using Shared.Attributes;
using Shared.Models;

namespace WillCDev.Components.Programs.AboutMe
{
    [Program("About Me", iconName: "Fax Sender Information", shrinkWindowOnLoad: true)]
    public partial class AboutMe : IProgram
    {
        [Parameter]
        public int AppId { get; set; }
    }
}