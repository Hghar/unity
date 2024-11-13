using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.WindowService.MVVM;
using Model.Economy;
using Plugins.Ship;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Plugins.Ship.Sheets.StepSheet.Commands.DefaultCommands;
using Realization.General;
using Realization.Shops;
using Realization.States.CharacterSheet;
using Realization.TutorialRealization.Helpers;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Realization.TutorialRealization.Commands
{
    public class UnityActions : ActionDictionary
    {
        private const string CharacterConfigPath = "Characters Config";
        private const string Separator = "_";
        private const string MoveHandSeparator = ";";
        private readonly ObjectFinder _objectFinder;
        private readonly HardTutorial _hardTutorial;
        private readonly CharacterConfig _characterConfig;
        private INotificationService _notificationService;
        private DiContainer _container;
        private IStorage _storage;
        private ISaveLoadService _saveLoadService;

        public UnityActions(ObjectFinder objectFinder, HardTutorial hardTutorial, DiContainer container, 
            INotificationService notificationService, IStorage storage, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _storage = storage;
            _container = container;
            _notificationService = notificationService;
            _hardTutorial = hardTutorial;
            _objectFinder = objectFinder;
            _characterConfig = Resources.Load<CharacterConfig>(CharacterConfigPath);
        }

        public override IAction Get(string name, string parameter, string argument)
        {
            bool activate;

            TutorialHand hand;
            Character character;
            DelayedObject minion;
            name = name.Trim();
            string[] stringCoordinates;
            int value;
            switch (name)
            {
                case "fade":
                    GameObject fade = _objectFinder.Fade;
                    activate = parameter.ConvertToBool();
                    return new FadeAction(fade, activate, _hardTutorial);
                case "hard":
                    activate = parameter.ConvertToBool();
                    return new HardAction(_hardTutorial, activate);
                case "hard_exclude":
                    DelayedObject excluded = new DelayedObject(parameter);
                    return new HardExcludeAction(_hardTutorial, excluded);
                case "hard_include":
                    DelayedObject included = new DelayedObject(parameter);
                    return new HardIncludeAction(_hardTutorial, included);
                case "hard_clear":
                    return new HardClearAction(_hardTutorial);
                case "hand":
                    DelayedObject target = new DelayedObject(parameter);
                    hand = _objectFinder.Hand;
                    bool flip = false;
                    float rotation = 0;
                    float x = 0;
                    float y = 0;
                    if (argument != "")
                    {
                        var splitted = argument.Split(Separator);
                        flip = splitted[0].ConvertToBool();
                        rotation = float.Parse(splitted[1], CultureInfo.InvariantCulture.NumberFormat);
                        stringCoordinates = splitted[2].Split(':');
                        x = float.Parse(stringCoordinates[0], CultureInfo.InvariantCulture.NumberFormat);
                        y = float.Parse(stringCoordinates[1], CultureInfo.InvariantCulture.NumberFormat);
                    }
                    return new HandAction(hand, target, flip, rotation, new Vector2(x, y));
                case "hand_disable":
                    TutorialHand handToDisable = _objectFinder.Hand;
                    return new HandDisableAction(handToDisable);
                case "highlight_unit":
                    DelayedObject highlightUnit = new DelayedObject(parameter);
                    bool highlight = argument.ConvertToBool();
                    return new HighLightUnitAction(highlightUnit, highlight); 
                case "delay":
                    float.TryParse(parameter, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat,
                        out var delay);
                    return new DelayAction(delay);
                case "move_hand":
                    hand = _objectFinder.Hand;
                    string[] names = parameter.Split(MoveHandSeparator);
                    List<IObjectProvider<GameObject>> targets = new List<IObjectProvider<GameObject>>();
                    foreach (var targetName in names)
                    {
                        targets.Add(new DelayedObject(targetName));
                    }
                    return new MoveHandAction(targets.ToArray(), hand);
                case "set_shop":
                    int.TryParse(argument, out var index);
                    character = GetCharacter(parameter);
                    return new SetShopAction(_container, character, index);
                case "hint_show":
                    var text = parameter;
                    var arguments = argument.Split(Separator);
                    var id = Enum.Parse<PopUpId>(arguments[0]);
                    var alignment = Enum.Parse<Alignment>(arguments[1]);
                    var closeButton = arguments[2].ConvertToBool();
                    return new HintShowAction(text, id, alignment, closeButton, _notificationService, _hardTutorial);
                case "hint_close":
                    var idToHide = Enum.Parse<PopUpId>(parameter);
                    return new HintCloseAction(idToHide, _notificationService);
                case "spawn_unit":
                    character = GetCharacter(parameter);
                    stringCoordinates = argument.Split(':');
                    x = int.Parse(stringCoordinates[0])-1;
                    y = int.Parse(stringCoordinates[1])-1;
                    return new SpawnUnitAction(character, _container, new Vector2Int((int)x, (int)y));
                case "update_coins":
                    var coins = int.Parse(parameter);
                    return new UpdateCoinsAction(_container, coins);
                case "press_button":
                    DelayedObject<Button> buttonToPress = new DelayedObject<Button>(parameter);
                    return new PressButtonAction(buttonToPress);
                case "fade_exclude":
                    DelayedObject fadeExcluded = new DelayedObject(parameter);
                    return new FadeExcludeAction(_hardTutorial, fadeExcluded);
                case "fade_include":
                    DelayedObject fadeIncluded = new DelayedObject(parameter);
                    return new FadeIncludeAction(_hardTutorial, fadeIncluded);
                case "set_key":
                    var key = int.Parse(parameter);
                    return new SetKeyAction(_storage, key, _saveLoadService);
                case "set_shop_on_reset":
                    var shopIndex = int.Parse(argument);
                    character = GetCharacter(parameter);
                    return new SetShopOnResetAction(_container, character, shopIndex);
                case "hard_unit_tap":
                    minion = new DelayedObject(parameter);
                    activate = argument.ConvertToBool();
                    return new HardUnitTapAction(minion, activate);
                case "hard_unit_hold":
                    minion = new DelayedObject(parameter);
                    activate = argument.ConvertToBool();
                    return new HardUnitHoldAction(minion, activate);
                case "feature":
                    var featureId = parameter;
                    activate = argument.ConvertToBool();
                    return new FeatureAction(featureId, activate);
                case "enable_object":
                    target = new DelayedObject(parameter);
                    activate = argument.ConvertToBool();
                    return new EnableObjectAction(target, activate);
                case "save_tutorial":
                    return new SaveTutorialAction(_storage, _saveLoadService); 
                case "set_reward":
                    minion = new DelayedObject(parameter);
                    var reward = int.Parse(argument);
                    return new SetRewardAction(minion, reward); 
                case "hand_off":
                    hand = _objectFinder.Hand;
                    return new HandDisableAction(hand);
                case "set_currency":
                    var currency = Enum.Parse<Currency>(argument);
                    value = int.Parse(parameter);
                    return new SetCurrencyAction(_storage, currency, value); 
                case "set_upgrade":
                    value = int.Parse(parameter);
                    if (argument == "General")
                    {
                        return new SetGeneralUpgradeAction(_storage, value); 
                    }
                    else
                    {
                        var type = Enum.Parse<ClassParent>(argument);
                        return new SetClassUpgradeAction(_storage, type, value); 
                    }
                default:
                    throw new ArgumentOutOfRangeException(name);
            }
        }

        private Character GetCharacter(string parameter)
        {
            var character = _characterConfig.Characters.FirstOrDefault((character1 => character1.Uid == parameter));
            if (character == null)
                return _characterConfig.Characters[0];
            return character;
        }
    }

    public enum RenderSpace
    {
        World,
        CanvasCamera,
        CanvasOverlay,
    }
}