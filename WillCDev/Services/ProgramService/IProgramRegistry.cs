using Microsoft.AspNetCore.Components;
using WillCDev.Components.Programs;

namespace WillCDev.Services.ProgramService
{
    public interface IProgramRegistry
    {
        void Register(ProgramAttribute attribute);
        ProgramAttribute? GetProgram(int programId);
        int GetProgramIdByComponentType(Type componentType);
        int GetProgramIdByProgramName(string programName);
        IDictionary<int, ProgramAttribute> GetProgramAttributes();
    }
}
