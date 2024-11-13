using Realization.States.CharacterSheet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelingCoins : MonoBehaviour
{
    [SerializeField] private ClassLevelingUpConfig _classLevelingUpConfig;

    public int Golds = 0, Gems = 0, Tokkens = 0;

    public int[] ClassLevel;

    public int GlobalLevel;

    public enum idClass
    {
        Scout, Warrior, Mage, Priest
    }

    public enum idCoins
    {
        Golds, Gems, Tokkens
    }

    private void Awake()
    {
        Golds = 1000; // PlayerPrefs.GetInt("Golds", 0);
        Gems = 1000; // PlayerPrefs.GetInt("Gems", 0);
        Tokkens = 1000; // PlayerPrefs.GetInt("Tokkens", 0);

        ClassLevel = new int[4]
        {
            PlayerPrefs.GetInt("Scout", 0),
            PlayerPrefs.GetInt("Warrior", 0),
            PlayerPrefs.GetInt("Mage", 0),
            PlayerPrefs.GetInt("Priest", 0)
        };

        GlobalLevel = PlayerPrefs.GetInt("Global", 0);
    }

    public void ClassLevelUp(idClass id)
    {
        switch (id)
        {
            case idClass.Scout:
                PlayerPrefs.SetInt("Scout", PlayerPrefs.GetInt("Scout", 0) + 1);
                ClassLevel[0] += 1;
                break;
            case idClass.Warrior:
                PlayerPrefs.SetInt("Warror", PlayerPrefs.GetInt("Warrior", 0) + 1);
                ClassLevel[1] += 1;
                break;
            case idClass.Mage:
                PlayerPrefs.SetInt("Mage", PlayerPrefs.GetInt("Mage", 0) + 1);
                ClassLevel[2] += 1;
                break;
            case idClass.Priest:
                PlayerPrefs.SetInt("Priest", PlayerPrefs.GetInt("Priest", 0) + 1);
                ClassLevel[3] += 1;
                break;
        }
    }

    public void ClassLevelReset(idClass id)
    {
        int tokkens = 0;
        switch (id)
        {
            case idClass.Scout:
                for (int i = 0; i < ClassLevel[0]; i++)
                {
                    tokkens += (int)_classLevelingUpConfig.Configs[ClassLevel[0] - 1].TokenCost;
                }

                Tokkens += tokkens;

                PlayerPrefs.SetInt("Tokkens", Tokkens);

                PlayerPrefs.SetInt("Scout", 0);

                ClassLevel[0] = 0;
                break;
            case idClass.Warrior:
                for (int i = 0; i < ClassLevel[1]; i++)
                {
                    tokkens += (int)_classLevelingUpConfig.Configs[ClassLevel[1] - 1].TokenCost;
                }

                Tokkens += tokkens;
                PlayerPrefs.SetInt("Tokkens", Tokkens);

                PlayerPrefs.SetInt("Warror", 0);
                ClassLevel[1] = 0;
                break;
            case idClass.Mage:
                for (int i = 0; i < ClassLevel[2]; i++)
                {
                    tokkens += (int)_classLevelingUpConfig.Configs[ClassLevel[2] - 1].TokenCost;
                }

                Tokkens += tokkens;
                PlayerPrefs.SetInt("Tokkens", Tokkens);

                PlayerPrefs.SetInt("Mage", 0);
                ClassLevel[2] = 0;
                break;
            case idClass.Priest:
                for (int i = 0; i < ClassLevel[3]; i++)
                {
                    tokkens += (int)_classLevelingUpConfig.Configs[ClassLevel[3] - 1].TokenCost;
                }

                Tokkens += tokkens;
                PlayerPrefs.SetInt("Tokkens", Tokkens);

                PlayerPrefs.SetInt("Priest", 0);
                ClassLevel[3] = 0;
                break;
        }
    }

    public void GeneralLevelUp()
    {
        GlobalLevel += 1;

        PlayerPrefs.SetInt("Global", PlayerPrefs.GetInt("Global", 0) + 1);
    }

    public bool[] IsPay(PayCoins[] idCoins)
    {
        bool[] isRetern = new bool[3] { true, true, true };

        for (int i = 0; i < idCoins.Length; i++)
        {
            switch (idCoins[i].IdCoins)
            {
                case PayCoins.idCoins.Golds:
                    if (Golds < idCoins[i].Value)
                        isRetern[i] = false;
                    break;
                case PayCoins.idCoins.Gems:
                    if (Gems < idCoins[i].Value)
                        isRetern[i] = false;
                    break;
                case PayCoins.idCoins.Tokkens:
                    if (Tokkens < idCoins[i].Value)
                        isRetern[i] = false;
                    break;
            }
        }

        return isRetern;
    }
    public void Pay(PayCoins[] idCoins)
    {
        for(int i = 0; i < idCoins.Length; i++)
        {
            switch (idCoins[i].IdCoins)
            {
                case PayCoins.idCoins.Golds:
                    if(Golds - idCoins[i].Value >= 0)
                        Golds -= idCoins[i].Value;
                    break;
                case PayCoins.idCoins.Gems:
                    if (Gems - idCoins[i].Value >= 0)
                        Gems -= idCoins[i].Value;
                    break;
                case PayCoins.idCoins.Tokkens:
                    if (Tokkens - idCoins[i].Value >= 0)
                        Tokkens -= idCoins[i].Value;
                    break;
            }
        }
    }
}

public class PayCoins
{
    public enum idCoins
    {
        Golds, Gems, Tokkens
    }

    public idCoins IdCoins;

    public int Value;

    public PayCoins(idCoins idCoin, int value)
    {
        IdCoins = idCoin;
        Value = value;
    }
        
}
