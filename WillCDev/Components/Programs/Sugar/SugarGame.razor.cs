using Microsoft.AspNetCore.Components;
using Shared.Attributes;
using Shared.Models;

namespace WillCDev.Components.Programs.Sugar
{
    [Program("Sugar Game", iconName: "Registry Editor", shrinkWindowOnLoad: true)]
    public partial class SugarGame : IProgram
    {
        [Parameter]
        public int AppId { get; set; }
    }
}