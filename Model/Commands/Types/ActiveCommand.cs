using System;
using UnityEngine;

namespace Model.Commands.Types
{
    public class ActiveCommand : ICommand
    {
        private Action _action;
        private Action _undo;
        private bool _done;
        private string _name;

        public ActiveCommand(Action action, Action undo, string name, float duration = float.MaxValue)
        {
            _name = name;
            _action = action;
            _undo = undo;
            Duration = duration;
        }

        public float Duration { get; }

        public void Perform()
        {
            Debug.Log($"Perform command {_name}");
            _action.Invoke();
        }

        public void Undo()
        {
            if(_done)
                return;
            
            Debug.Log($"Undo active command {_name}");
            _undo.Invoke();
            _done = true;
        }
    }
}