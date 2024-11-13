using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.DevTools
{
    public class LevelDevTool : MonoBehaviour
    {
        [SerializeField] private ParamsMenu _paramsMenu;

        [SerializeField] private TMP_InputField _addPoints;
        [SerializeField] private TMP_Text _points;

        private void Awake()
        {
            _addPoints.onSubmit.AddListener(AddPoints);
        }

        private void Update()
        {
            _points.text = _paramsMenu.SelectedMinion.ReturnLevelPoint;
        }

        private void AddPoints(string input)
        {
            var value = float.Parse(input);
            _paramsMenu.SelectedMinion.AddLevelPoints(value);
        }
    }
}
