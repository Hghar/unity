using System;
using Battle;
using Firebase.Analytics;
using Model.Maps;
using UnityEngine;
using Zenject;

namespace Realization.General
{
    public class LoosingAnaliticPublisher : ILoosingAnaliticPublisher, IDisposable
    {
        // TODO: separate logic
        private const int StarvationEventValue = 0;
        private const string LoseMessagePrefix = "lose_";
        private const string InStarvationMarker = "in_starvation_";

        private IBattleContinuingFlag _battleContinuingFlag;
        private IRestarter _restarter;

        [Inject]
        private void Construct(IBattleContinuingFlag battleContinuingFlag, IRestarter restarter)
        {
            _battleContinuingFlag = battleContinuingFlag;
            _restarter = restarter;
            _restarter.Restarting += OnRestarting;
        }

        public void Publish(int level)
        {
            string message = LoseMessagePrefix;

            if (_battleContinuingFlag.Value)
                message += "in_battle_";
            else
                message += "for_another_reason";

            message += "_l" + level.ToString(); // TODO: use common code for level marking
            FirebaseAnalytics.LogEvent(message);
        }

        public void Dispose()
        {
            _restarter.Restarting -= OnRestarting;
        }

        private void OnRestarting()
        {
            string message = LoseMessagePrefix;
            message += "by_restart_";

            int level = PlayerPrefs.GetInt("level");
            message += "_l" + level.ToString(); // TODO: use common code for level marking
            FirebaseAnalytics.LogEvent(message);
        }
    }
}