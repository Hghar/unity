using UnityEngine;

namespace Realization.Configs
{
    [CreateAssetMenu(fileName = "New Character Skills", menuName = "Configs/CharacterSkills", order = 0)]
    public class CharacterSkills : ScriptableObject
    {
        //TODO in progress
        [SerializeField] private string _uid;
        [SerializeField] private string _actions;
    }
}