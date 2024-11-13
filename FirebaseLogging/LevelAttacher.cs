using UnityEngine;

namespace FirebaseLogging
{
    public static class LevelAttacher
    {
        public static string AttachLevel(string message)
        {
            int currentLevel = PlayerPrefs.GetInt("level"); // TODO: use properties for prefs
            return message + '_' + EventNames.LevelIndex + currentLevel;
        }
    }
}