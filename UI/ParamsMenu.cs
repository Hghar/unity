using System;
using AssetStore.HeroEditor.Common.CommonScripts;
using Fight;
using Fight.Attack;
using Parameters;
using Realization.States.CharacterSheet;
using TMPro;
using UI.Binding;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class
        ParamsMenu : MonoBehaviour, IPointerClickHandler,
            IParamsMenu // TODO: create in other classes item, name, etc. showers and separate hidding
    {
        [SerializeField] private CharacterConfig config;
        [SerializeField] private CloseAllPanel _closeAllPanel;
        //TODO: rework
        [Header("Sliders (Old)")] [SerializeField] private LimitedValueView _healthView;
        [SerializeField] private LimitedValueView _manaView;
        [SerializeField] private LimitedValueView _levelView;
        [SerializeField] private BindedIntSlider _experienceSlider;
        [SerializeField] private BindedIntText _levelText;

        [Header("Sliders (Denis)")] 
        [SerializeField] private Slider _healthViewSlider;
        [SerializeField] private Slider _manaViewSlider;
        [SerializeField] private Slider _levelViewSlider;

        [Header("Texts (Denis)")]
        [SerializeField] private TMP_Text Text_Level_Number;
        [SerializeField] private TMP_Text[] Text_Level_Slider;
        [SerializeField] private TMP_Text[] Text_Energy_Slider;
        [SerializeField] private TMP_Text[] Text_Health_Slider;
        [SerializeField] private TMP_Text Text_Class;
        [SerializeField] private TMP_Text Text_Might;
        [SerializeField] private TMP_Text Text_Skill;
        [SerializeField] private TMP_Text Text_Character_Name;
        [SerializeField] private string[] Characters_Names;
        [SerializeField] private Color32[] Character_Colors_Name;

        [Header("Sprites (Denis)")]
        [SerializeField] private Image Character_Head_Render;
        [SerializeField] private Image Character_Grade_Render;
        [SerializeField] private Image Character_Class_Render;
        [SerializeField] private Image Skils_Render;
        [SerializeField] private Sprite[] Character_Head_Sprites;
        [SerializeField] private Sprite[] Evil_Head_Sprites;
        [SerializeField] private Sprite[] Character_Grade_Sprites;
        [SerializeField] private Sprite[] Character_Class_Sprites;
        [SerializeField] private Sprite[] Skils_Sprites;

        [Header("Button_Sell(Denis)")]
        [SerializeField] private TMP_Text _text_Button;

        [Header("Button_Skill(Denis)")]
        [SerializeField] private Button _spellButton;
        [SerializeField] private GameObject _spellObject;
        [SerializeField] private TMP_Text _spellText;
        [SerializeField] private string[] _spellsDescription;

        [Header("Numeric")] [SerializeField] private BindedFloatText _damage;
        [SerializeField] private BindedFloatText _cooldown;
        [SerializeField] private BindedFloatText _armor;
        [SerializeField] private BindedFloatText _speed;

        [Header("Minion menu")] // TODO: separate minion params menu
        [SerializeField]
        private Image _item;

        [SerializeField] private Image _class;
        [SerializeField] private GameObject _itemMenuPart;
        [SerializeField] private MinionsClassIconsLibrary _classIconsLibrary;
        [Header("Enemy")] [SerializeField] private Sprite _enemy;

        [SerializeField] private InfoHint _infoHint;

        private IMinion _minion;

        private IHealth _health;
        private Level _level;
        private Energy _energy;

        private Vector2 _oldPositionMana, _oldPositionLevel;

        public event Action Destroying;

        public IMinion SelectedMinion => _minion;

        private void Start()
        {
            _oldPositionLevel = _levelViewSlider.transform.parent.localPosition;
            _oldPositionMana = _manaViewSlider.transform.parent.localPosition;
        }

        private void OnEnable()
        {
            _spellObject.SetActive(false);

            //_spellButton.onClick.AddListener(ClickSpellButton);

        }
        private void OnDisable()
        {
            _spellObject.SetActive(false);
            //_spellButton.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            TryUnbindMinion();
            Destroying?.Invoke();
        }

        public void ClickSpellButton()
        {
            _spellObject.SetActive(!_spellObject.activeSelf);
        }

        public void Bind(IUnit unit)
        {
            TryUnbindMinion();
            BindParams(unit.Parameters);

            if (unit is IMinion minion)
            {
                _minion = minion;
                ShowMinionMenu();
                BindMinion();

                _closeAllPanel.Inicialize(minion);
            }
            else
            {
                HideMinionMenu();
                _class.sprite = _enemy;
            }
        }

        private bool TryUnbindMinion()
        {
            if (SelectedMinion == null)
                return false;

            // _minion.ItemChanged -= OnItemChanged;
            // _minion.ClassChanged -= OnClassChanged;
            _minion = null;
            return true;
        }

        private void HideMinionMenu()
        {
            _itemMenuPart.SetActive(false);
        }

        private void ShowMinionMenu()
        {
            _itemMenuPart.SetActive(true);
        }

        public void Hide()
        {
            if (gameObject != null)
                gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            _infoHint.CloseOnButton();

            if (_minion.Fraction == Fight.Fractions.Fraction.Minions)
            {
                Character_Head_Render.sprite = Character_Head_Sprites[((int)_minion.Class * 5) + (int)_minion.Grade - 1];

                Text_Character_Name.text = Characters_Names[((int)_minion.Class * 5) + (int)_minion.Grade - 1];
            }
            else if (_minion.Fraction == Fight.Fractions.Fraction.Enemies)
            {
                if (_minion.Uid.Contains("Skeleton")) 
                {
                    if (_minion.IsBoss)
                    {
                        switch (_minion.Class)
                        {
                            case MinionClass.Cleric:
                                Character_Head_Render.sprite = Evil_Head_Sprites[16];
                                break;
                            case MinionClass.Gladiator:
                                Character_Head_Render.sprite = Evil_Head_Sprites[17];
                                break;
                        }
                    }
                    else
                    {
                        Character_Head_Render.sprite = Evil_Head_Sprites[((int)_minion.Class)];
                    }

                    Text_Character_Name.text = "Skeleton";
                }
                else if (_minion.Uid.Contains("Demon"))
                {
                    if (_minion.IsBoss)
                    {
                        switch (_minion.Class)
                        {
                            case MinionClass.Assassin:
                                Character_Head_Render.sprite = Evil_Head_Sprites[19];
                                break;
                            case MinionClass.Gladiator:
                                Character_Head_Render.sprite = Evil_Head_Sprites[18];
                                break;
                        }
                    }
                    else
                    {
                        Character_Head_Render.sprite = Evil_Head_Sprites[(8 + (int)_minion.Class)];
                    }

                    Text_Character_Name.text = "Demon";
                }
                else if (_minion.Uid.Contains("Mummy"))
                {
                    Character_Head_Render.sprite = Evil_Head_Sprites[((int)_minion.Class)];

                    Text_Character_Name.text = "Mummy";
                }
            }

            Character_Grade_Render.sprite = Character_Grade_Sprites[(int)_minion.Grade - 1];
            Character_Class_Render.sprite = Character_Class_Sprites[(int)_minion.Class];

            Skils_Render.sprite = Skils_Sprites[((int)_minion.Class * 5) + (int)_minion.Grade - 1];
            _spellText.text = _spellsDescription[((int)_minion.Class * 5) + (int)_minion.Grade - 1];

            Text_Character_Name.color = Character_Colors_Name[_minion.Grade - 1];

            //Debug.LogError((int)_minion.Class + " " + (((int)_minion.Class * 5) + (int)_minion.Grade - 1));

            Text_Level_Number.text = "Lvl " + (_level.level);
            Text_Class.text = _minion.Class.ConvertToString();

            Text_Might.text = MathF.Round(_minion.Might.PersonalMight).ToString();

            if(!_minion.IsMinion || (_minion.Fraction == Fight.Fractions.Fraction.Enemies && !_minion.IsBoss))
            {
                Skils_Render.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Skils_Render.transform.parent.gameObject.SetActive(true);
            }

            if(_minion.EnergyMaxValue <= 0)
            {
                _manaViewSlider.gameObject.SetActive(false);

                Text_Skill.gameObject.SetActive(true);

                //_levelViewSlider.transform.parent.localPosition = _oldPositionMana;
            }
            else
            {
                //_levelViewSlider.transform.parent.localPosition = _oldPositionLevel;

                _manaViewSlider.gameObject.SetActive(true);

                Text_Skill.gameObject.SetActive(false);

                Text_Energy_Slider[0].text = MathF.Round(_minion.EnergyValue).ToString();
                Text_Energy_Slider[2].text = MathF.Round(_minion.EnergyMaxValue).ToString();

                _manaViewSlider.value = _minion.EnergyValue / _minion.EnergyMaxValue;
            }

            Text_Level_Slider[0].text = MathF.Round(_level.Value).ToString();
            Text_Level_Slider[2].text = MathF.Round(_level.MaxValue).ToString();

            if (_minion.Level.level < 5)
            {
                _levelViewSlider.value = _level.Value / _level.MaxValue;
            }
            else
            {
                Text_Level_Slider[1].text = "MAX";

                Text_Level_Slider[0].text = "";
                Text_Level_Slider[2].text = "";
            }

            string sale = "";

            switch (_minion.Level.level) 
            {
                case 1:
                    sale = config.CharacterStore[(int)_minion.Grade - 1].Sell_1.ToString();
                    break;
                case 2:
                    sale = config.CharacterStore[(int)_minion.Grade - 1].Sell_2.ToString();
                    break;
                case 3:
                    sale = config.CharacterStore[(int)_minion.Grade - 1].Sell_3.ToString();
                    break;
                case 4:
                    sale = config.CharacterStore[(int)_minion.Grade - 1].Sell_4.ToString();
                    break;
                case 5:
                    sale = config.CharacterStore[(int)_minion.Grade - 1].Sell_5.ToString();
                    break;
            }

            _text_Button.text = sale;
        }

        private void FixedUpdate()
        {
            if (gameObject.activeSelf)
            {
                Text_Level_Number.text = "Lvl " + (_level.level);

                Text_Might.text = MathF.Round(_minion.Might.PersonalMight).ToString();

                if(_manaViewSlider.gameObject.activeSelf)
                {
                    Text_Energy_Slider[0].text = MathF.Round(_minion.EnergyValue).ToString();
                    Text_Energy_Slider[2].text = MathF.Round(_minion.EnergyMaxValue).ToString();

                    _manaViewSlider.value = _minion.EnergyValue / _minion.EnergyMaxValue;
                }

                Text_Level_Slider[0].text = MathF.Round(_level.Value).ToString();
                Text_Level_Slider[1].text = "/";
                Text_Level_Slider[2].text = MathF.Round(_level.MaxValue).ToString();

                if (_minion.Level.level < 5)
                {
                    _levelViewSlider.value = _level.Value / _level._maxLevelPoints[_level.level];
                }
                else
                {
                    Text_Level_Slider[1].text = "MAX";

                    Text_Level_Slider[0].text = "";
                    Text_Level_Slider[2].text = "";
                }
            }
        }

        private void BindParams(IUnitParameters unitParameters)
        {
            _health = unitParameters.Health;
            _energy = unitParameters.Energy;
            _level = unitParameters.Level;

            _healthView.SetPublisher(unitParameters.Health);
            //_manaView.SetPublisher(unitParameters.Energy); //TODO: uncomment when mana will be created
            //_levelView.SetPublisher(unitParameters.Level);
            // _experienceSlider.SetPublisher(unitParams.Experience); TODO: uncomment when Experience will be created
            //_level.SetPublisher(unitParameters.Level); //TODO: uncomment when Experience will be created

             _damage.SetPublisher(unitParameters.Damage);
             _cooldown.SetPublisher(unitParameters.Cooldown);
             _armor.SetPublisher(unitParameters.Armor);
            //_speed.SetPublisher(unitParameters.Speed);
        }

        private void BindMinion()
        {
            BindItem();
            BindClass();
        }

        private void BindClass()
        {
            ShowClass();
            // _minion.ClassChanged += OnClassChanged;
        }

        private void OnClassChanged(bool shouldAnounce)
        {
            ShowClass();
        }

        private void BindItem()
        {
            ShowItem();
            // _minion.ItemChanged += OnItemChanged;
        }

        private void OnItemChanged()
        {
            ShowItem();
        }

        private void ShowItem()
        {
            //_minion.TryGetItemIcon(out Sprite itemSprite);
            //_item.sprite = itemSprite;
            //_itemMenuPart.SetActive(_item.sprite != null);
        }

        private void ShowClass()
        {
            // if (_classIconsLibrary.TryFindIcon(_minion.Class, out Sprite iconSprite) == false)
            //     Debug.LogError($"Icon for class {_minion.Class} wasn't found in {name}");
            //
            // Class.sprite = iconSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}