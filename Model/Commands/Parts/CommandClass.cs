using System;

namespace Model.Commands.Parts
{
    public struct CommandClass
    {
        public readonly string Action;
        public readonly float Duration;

        public CommandClass(string action, float duration)
        {
            Action = action;
            Duration = duration;
        }

        public override bool Equals(object obj)
        {
            if (obj is CommandClass anotherClass)
                return Action == anotherClass.Action &&
                       Duration == anotherClass.Duration;

            return false;
        }
    }
}