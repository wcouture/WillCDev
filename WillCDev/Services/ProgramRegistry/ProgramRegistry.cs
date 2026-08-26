using Microsoft.AspNetCore.Components;
using WillCDev.Components.Programs;

namespace WillCDev.Services.Program
{
    public class ProgramRegistry : IProgramRegistry
    {
        private IDictionary<int, ProgramAttribute> _programAttributes = new Dictionary<int, ProgramAttribute>();

        public ProgramAttribute? GetProgram(int programId)
        {
            _programAttributes.TryGetValue(programId, out var attribute);
            return attribute;
        }

        public int GetProgramIdByComponentType(Type componentType)
        {
            foreach (var kvp in _programAttributes)
            {
                if (kvp.Value.ComponentType == componentType)
                {
                    return kvp.Key;
                }
            }
            return -1;
        }

        public int GetProgramIdByProgramName(string programName)
        {
            foreach (var kvp in _programAttributes)
            {
                if (kvp.Value.Name == programName)
                {
                    return kvp.Key;
                }
            }
            return -1;
        }

        public IDictionary<int, ProgramAttribute> GetProgramAttributes()
        {
            return _programAttributes;
        }

        public void Register(ProgramAttribute attribute)
        {
            _programAttributes[attribute.ID] = attribute;
        }
    }
}
