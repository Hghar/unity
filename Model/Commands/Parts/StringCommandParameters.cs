using Model.Commands.Parts;
using Units;

namespace Model.Commands
{
    public class StringCommandParameters
    {
        public CommandType Type;
        public string Action;
        public object[] ActionParameters;
        public TargetType Target;
        public object[] TargetParameters;
        public float Duration;
        public CommandClass CommandClass;

        public bool UsingRadius;
        public int Radius;
        public IMinion Caster;

        public StringCommandParameters(CommandType type, string action, object[] actionParameters, TargetType target,
            object[] targetParameters, float duration, IMinion caster)
        {
            Duration = duration;
            TargetParameters = targetParameters;
            Target = target;
            ActionParameters = actionParameters;
            Action = action;   
            Type = type;
            CommandClass = new CommandClass(action, duration);
            Caster = caster;
        }

        public void AddRadius(int radius)
        {
            UsingRadius = true;
            Radius = radius;
        }
    }
}