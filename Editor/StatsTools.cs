using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Realization.GameStateMachine.States;
using Units;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class StatsTools : UnityEditor.Editor
    {
        private const string ProgressKey = "Progress";
        private static void SetLevel(ClassParent id, int number)
        {
            PlayerProgress playerProgress = LoadProgress() ?? CreateDefaultProgress();
            playerProgress.CoreUpgrades.Stats.SetLevel(id,number);
            SaveProgress(playerProgress);
            Debug.Log($"<color=green> Уровень <color=red> {number} </color> для  {id} был установлен </color>");

        }

        private static void SetupGeneralLevel(int level )
        {
            PlayerProgress playerProgress = LoadProgress() ?? CreateDefaultProgress();
            playerProgress.CoreUpgrades.CurrentGeneralLevel = level;
            SaveProgress(playerProgress);
            Debug.Log($"<color=green> Общий уровень <color=red>{level}</color> был установлен </color>");

        }

        private static void SaveProgress(PlayerProgress playerProgress) =>
                PlayerPrefs.SetString(ProgressKey, playerProgress.ToJson());

        private static PlayerProgress LoadProgress() =>
                PlayerPrefs.GetString(ProgressKey)?
                        .ToDeserialized<PlayerProgress>();

        private static PlayerProgress CreateDefaultProgress()
        {
            StaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            LoadProgressState.ProgressDefaultSittings defaultSittings =
                    new LoadProgressState.ProgressDefaultSittings(staticDataService);
            var playerProgress = defaultSittings.SetupDefault();
            Debug.Log(
                    "<color=red> Был создан новый прогресс </color>");

            return playerProgress;
        }

        
        //GENERAL
        [MenuItem("Tools/SetLevel/General/1")] public static void SetGeneralLevel1() => SetupGeneralLevel(1);
        [MenuItem("Tools/SetLevel/General/2")] public static void SetGeneralLevel2() => SetupGeneralLevel(2);
        [MenuItem("Tools/SetLevel/General/3")] public static void SetGeneralLevel3() => SetupGeneralLevel(3);
        [MenuItem("Tools/SetLevel/General/4")] public static void SetGeneralLevel4() => SetupGeneralLevel(4);
        [MenuItem("Tools/SetLevel/General/5")] public static void SetGeneralLevel5() => SetupGeneralLevel(5);
        [MenuItem("Tools/SetLevel/General/6")] public static void SetGeneralLevel6() => SetupGeneralLevel(6);
        [MenuItem("Tools/SetLevel/General/7")] public static void SetGeneralLevel7() => SetupGeneralLevel(7);
        [MenuItem("Tools/SetLevel/General/8")] public static void SetGeneralLevel8() => SetupGeneralLevel(8);
        [MenuItem("Tools/SetLevel/General/9")] public static void SetGeneralLevel9() => SetupGeneralLevel(9);
        [MenuItem("Tools/SetLevel/General/10")] public static void SetGeneralLevel10() => SetupGeneralLevel(10);
        
        //MAGE
        [MenuItem("Tools/SetLevel/Mage/1")] public static void SetMageLevel1() => SetLevel(ClassParent.Mage,1);
        [MenuItem("Tools/SetLevel/Mage/2")] public static void SetMageLevel2() => SetLevel(ClassParent.Mage,2);
        [MenuItem("Tools/SetLevel/Mage/3")] public static void SetMageLevel3() => SetLevel(ClassParent.Mage,3);
        [MenuItem("Tools/SetLevel/Mage/4")] public static void SetMageLevel4() => SetLevel(ClassParent.Mage,4);
        [MenuItem("Tools/SetLevel/Mage/5")] public static void SetMageLevel5() => SetLevel(ClassParent.Mage,5);
        [MenuItem("Tools/SetLevel/Mage/6")] public static void SetMageLevel6() => SetLevel(ClassParent.Mage,6);
        [MenuItem("Tools/SetLevel/Mage/7")] public static void SetMageLevel7() => SetLevel(ClassParent.Mage,7);
        [MenuItem("Tools/SetLevel/Mage/8")] public static void SetMageLevel8() => SetLevel(ClassParent.Mage,8);
        [MenuItem("Tools/SetLevel/Mage/9")] public static void SetMageLevel9() => SetLevel(ClassParent.Mage,9);
        [MenuItem("Tools/SetLevel/Mage/10")] public static void SetMageLevel10() => SetLevel(ClassParent.Mage,10);
        
        //PRIEST
        [MenuItem("Tools/SetLevel/Priest/1")] public static void SetPriestLevel1() => SetLevel(ClassParent.Priest,1);
        [MenuItem("Tools/SetLevel/Priest/2")] public static void SetPriestLevel2() => SetLevel(ClassParent.Priest,2);
        [MenuItem("Tools/SetLevel/Priest/3")] public static void SetPriestLevel3() => SetLevel(ClassParent.Priest,3);
        [MenuItem("Tools/SetLevel/Priest/4")] public static void SetPriestLevel4() => SetLevel(ClassParent.Priest,4);
        [MenuItem("Tools/SetLevel/Priest/5")] public static void SetPriestLevel5() => SetLevel(ClassParent.Priest,5);
        [MenuItem("Tools/SetLevel/Priest/6")] public static void SetPriestLevel6() => SetLevel(ClassParent.Priest,6);
        [MenuItem("Tools/SetLevel/Priest/7")] public static void SetPriestLevel7() => SetLevel(ClassParent.Priest,7);
        [MenuItem("Tools/SetLevel/Priest/8")] public static void SetPriestLevel8() => SetLevel(ClassParent.Priest,8);
        [MenuItem("Tools/SetLevel/Priest/9")] public static void SetPriestLevel9() => SetLevel(ClassParent.Priest,9);
        [MenuItem("Tools/SetLevel/Priest/10")] public static void SetPriestLevel10() => SetLevel(ClassParent.Priest,10);
        
        //SCOUT
        [MenuItem("Tools/SetLevel/Scout/1")] public static void SetScoutLevel1() => SetLevel(ClassParent.Scout,1);
        [MenuItem("Tools/SetLevel/Scout/2")] public static void SetScoutLevel2() => SetLevel(ClassParent.Scout,2);
        [MenuItem("Tools/SetLevel/Scout/3")] public static void SetScoutLevel3() => SetLevel(ClassParent.Scout,3);
        [MenuItem("Tools/SetLevel/Scout/4")] public static void SetScoutLevel4() => SetLevel(ClassParent.Scout,4);
        [MenuItem("Tools/SetLevel/Scout/5")] public static void SetScoutLevel5() => SetLevel(ClassParent.Scout,5);
        [MenuItem("Tools/SetLevel/Scout/6")] public static void SetScoutLevel6() => SetLevel(ClassParent.Scout,6);
        [MenuItem("Tools/SetLevel/Scout/7")] public static void SetScoutLevel7() => SetLevel(ClassParent.Scout,7);
        [MenuItem("Tools/SetLevel/Scout/8")] public static void SetScoutLevel8() => SetLevel(ClassParent.Scout,8);
        [MenuItem("Tools/SetLevel/Scout/9")] public static void SetScoutLevel9() => SetLevel(ClassParent.Scout,9);
        [MenuItem("Tools/SetLevel/Scout/10")] public static void SetScoutLevel10() => SetLevel(ClassParent.Scout,10);

        //WARRIOR
        [MenuItem("Tools/SetLevel/Warrior/1")] public static void SetWarriorLevel1() => SetLevel(ClassParent.Warrior,1);
        [MenuItem("Tools/SetLevel/Warrior/2")] public static void SetWarriorLevel2() => SetLevel(ClassParent.Warrior,2);
        [MenuItem("Tools/SetLevel/Warrior/3")] public static void SetWarriorLevel3() => SetLevel(ClassParent.Warrior,3);
        [MenuItem("Tools/SetLevel/Warrior/4")] public static void SetWarriorLevel4() => SetLevel(ClassParent.Warrior,4);
        [MenuItem("Tools/SetLevel/Warrior/5")] public static void SetWarriorLevel5() => SetLevel(ClassParent.Warrior,5);
        [MenuItem("Tools/SetLevel/Warrior/6")] public static void SetWarriorLevel6() => SetLevel(ClassParent.Warrior,6);
        [MenuItem("Tools/SetLevel/Warrior/7")] public static void SetWarriorLevel7() => SetLevel(ClassParent.Warrior,7);
        [MenuItem("Tools/SetLevel/Warrior/8")] public static void SetWarriorLevel8() => SetLevel(ClassParent.Warrior,8);
        [MenuItem("Tools/SetLevel/Warrior/9")] public static void SetWarriorLevel9() => SetLevel(ClassParent.Warrior,9);
        [MenuItem("Tools/SetLevel/Warrior/10")] public static void SetWarriorLevel10() => SetLevel(ClassParent.Warrior,10);
    }
}