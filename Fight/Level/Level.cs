using Realization.Configs;
using Realization.States.CharacterSheet;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static bool WorkingFeature = true;
    
    [SerializeField] private LevelEffects _levelEffects;
    [SerializeField] private ControlBars _controlBars;
    [SerializeField] private Constants _constantsConfig;
    [SerializeField] private CharacterConfig characterConfig;
    public int level = 1;
    private IMinion _minion;

    private float _nowLevelPoints = 0;
    public float[] _maxLevelPoints = new float[5];

    private string LevelPoints;

    private int startLevel = 1;

    public float Value => _nowLevelPoints;
    public float MaxValue => _maxLevelPoints[level-1];

    private void Awake()
    {
        _constantsConfig = characterConfig.Constants;
    }

    private void Start()
    {
        //Inicializade();

        _minion ??= this.transform.parent.GetComponent<IMinion>();
    }

    private void Update()
    {
        _controlBars.SetLevelActive(WorkingFeature);
        //LevelPoints = "Level " + (level) + " Now Level Point " + _nowLevelPoints + " Max Level Points" + _maxLevelPoints[level - 1];
    }

    public void Inicializade(IMinion minion, int startLevel)
    {
        _minion = minion;

        for (int i = 0; i < _maxLevelPoints.Length - 1; i++)
        {
            float _maxPoints = 99999;

            switch (i)
            {
                case 0:
                    _maxPoints = _constantsConfig.LvlUpExperienceBoundaries.x;
                    break;
                case 1:
                    _maxPoints = _constantsConfig.LvlUpExperienceBoundaries.y;
                    break;
                case 2:
                    _maxPoints = _constantsConfig.LvlUpExperienceBoundaries.z;
                    break;
                case 3:
                    _maxPoints = _constantsConfig.LvlUpExperienceBoundaries.w;
                    break;
            }

            _maxLevelPoints[i] = _maxPoints;
        }

        for (int i = 1; i < startLevel; i++)
        {
            LevelUp();
        }
    }

    public void CheatAddPoint(float points)
    {
        AddPoints(points);
    }

    private IMinion[] RemoveUnits(IMinion[] units)
    {
        List<IMinion> minions = new List<IMinion>(units);

        minions.RemoveAll(s => s.Level.level == 5);

        minions.RemoveAll(s => s.IsMinion == false);

        return minions.ToArray();
    }

    public void AddPoints(IMinion minion, IMinion[] units)
    {
        if(WorkingFeature == false)
            return;
        
        units = RemoveUnits(units);

        Debug.Log("Dead " + minion.Fraction + "/" + minion.Class + " / Grade: " + minion.Grade);

        switch (minion.Grade)
        {
            case 1:
                AddPoints(ReturnBattleLevelPool(minion, _constantsConfig.BattleExperiencePoolGrade1) / units.Length);
                break;
            case 2:
                AddPoints(ReturnBattleLevelPool(minion, _constantsConfig.BattleExperiencePoolGrade2) / units.Length);
                break;
            case 3:
                AddPoints(ReturnBattleLevelPool(minion, _constantsConfig.BattleExperiencePoolGrade3) / units.Length);
                break;
            case 4:
                AddPoints(ReturnBattleLevelPool(minion, _constantsConfig.BattleExperiencePoolGrade4) / units.Length);
                break;
            case 5:
                AddPoints(ReturnBattleLevelPool(minion, _constantsConfig.BattleExperiencePoolGrade5) / units.Length);
                break;
        }
    }

    private float ReturnBattleLevelPool(IMinion minion, Vector5 vector5)
    {
        switch (minion.Level.level)
        {
            case 1:
                return vector5.Level_1;
            case 2:
                return vector5.Level_2;
            case 3:
                return vector5.Level_3;
            case 4:
                return vector5.Level_4;
            case 5:
                return vector5.Level_5;
        }

        return 99999;
    }
    private void AddPoints(float point)
    {
        if(WorkingFeature == false)
            return;
        
        point = Mathf.Round(point);

        Debug.Log("Add level point " + point + " Now Level Point " + _nowLevelPoints);

        if (level < 5)
        {
            _nowLevelPoints += point;

            _levelEffects.AddExpEffect((int)point);
        }

        while(_nowLevelPoints >= _maxLevelPoints[level - 1])
        {
            if (level < 5)
            {
                _nowLevelPoints = _nowLevelPoints - _maxLevelPoints[level - 1];

                LevelUp();
            }
            else
            {
                break;
            }
        }

        LevelPoints = ("Level " + (level) + " Now Level Point " + _nowLevelPoints + " Max Level Points" + _maxLevelPoints[level - 1]);
    }

    public void LevelUp()
    {
        if(WorkingFeature == false)
            return;
        
        if (_minion == null)
        {
            _minion ??= this.transform.parent.GetComponent<IMinion>();
        }

        _levelEffects.LevelUpEffect();

        level += 1;

        if (_minion != null)
        {
            _controlBars.UpdateLevelNumber(level);

            _minion.LevelUp(_constantsConfig);
        }

        if (level == 5)
        {
            _nowLevelPoints = 0;
        }

        LevelPoints = ("Level " + (level) + " Now Level Point " + _nowLevelPoints + " Max Level Points" +
                       _maxLevelPoints[level - 1]);
    }

    public string LevelIntoText()
    {
        return LevelPoints;
    }
}
