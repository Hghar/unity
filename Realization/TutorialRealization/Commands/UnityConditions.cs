using System;
using System.Collections.Generic;
using Model.Economy;
using Plugins.Ship.Sheets.StepSheet.Commands.Conditions;
using Plugins.Ship.Sheets.StepSheet.Commands.DefaultCommands;
using Plugins.Ship.Sheets.StepSheet.Steps;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.TutorialRealization.Commands
{
    public class UnityConditions : ConditionDictionary, IDisposable
    {
        private const char Separator = ':';
        private List<IDisposable> _disposables = new();
        private IStorage _storage;

        public UnityConditions(IStorage storage)
        {
            _storage = storage;
        }

        public override ICondition Get(string name, string parameter, string argument, IParameterHolder parameterHolder)
        {
            DelayedObject unit;
            string[] stringCoordinates;
            int x;
            int y;
            switch (name)
            {
                case "button_pressed":
                    DelayedObject<Button> button = new DelayedObject<Button>(parameter);
                    _disposables.Add(button);
                    return new ButtonPressedCondition(button);
                case "destroyed":
                    DelayedObject destroyed = new DelayedObject(parameter);
                    _disposables.Add(destroyed);
                    return new DestroyedCondition(destroyed);
                case "object_enabled":
                    DelayedObject obj = new DelayedObject(parameter);
                    _disposables.Add(obj);
                    bool acted = argument is "true" or "";
                    return new ObjectEnabledCondition(obj, acted);
                case "unit_on_cell":
                    unit = new DelayedObject(parameter);
                    stringCoordinates = argument.Split(Separator);
                    x = int.Parse(stringCoordinates[0])-1;
                    y = int.Parse(stringCoordinates[1])-1;
                    return new UnitOnCellCondition(unit, new Vector2Int(x, y));
                case "begin_meta":
                    return new BeginMeta();
                case "get_key":
                    var key = int.Parse(parameter);
                    return new GetKeyCondition(_storage, key);
                case "unit_moved":
                    unit = new DelayedObject(parameter); 
                    stringCoordinates = argument.Split(Separator);
                    x = int.Parse(stringCoordinates[0])-1;
                    y = int.Parse(stringCoordinates[1])-1;
                    return new UnitMovedCondition(unit, new Vector2Int(x, y));
                default:
                    throw new ArgumentOutOfRangeException(name);
            }
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}