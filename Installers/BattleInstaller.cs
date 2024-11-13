using Battle;
using Realization.General;
using Units.Ai;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BattleInstaller : MonoInstaller
    {
        [SerializeField] private StartBattleButton _startBattleButton;
        [SerializeField] private EnemySpawner _enemySpawner;

        private void OnValidate()
        {
            if (_startBattleButton == null)
                _startBattleButton = FindObjectOfType<StartBattleButton>();

            if (_enemySpawner == null)
                _enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        private void OnDestroy()
        {
            IBattleStartPublisher battleStarter = Container.Resolve<IBattleStartPublisher>();
            if (battleStarter != null)
                ((BattleStarter) battleStarter).Dispose();

            IBattleFinishPublisher battleFinisher = Container.Resolve<IBattleFinishPublisher>();
            if (battleFinisher != null)
                ((BattleFinisher) battleFinisher).Dispose(); // TODO: create common code

            IEnemiesPool enemiesPool = Container.Resolve<IEnemiesPool>();
            if (enemiesPool != null)
                ((EnemiesPool) enemiesPool).Dispose();

            IMinionsAiPool minionsAiPool = Container.Resolve<IMinionsAiPool>();
            if (minionsAiPool != null)
                ((MinionsAiPool) minionsAiPool).Dispose();

            IBattleContinuingFlag battleContinuingFlag = Container.Resolve<IBattleContinuingFlag>();
            if (battleContinuingFlag != null)
                ((BattleContinuingFlag) battleContinuingFlag).Dispose();

            ILoosingAnaliticPublisher loosingAnaliticPublisher = Container.Resolve<ILoosingAnaliticPublisher>();
            if (loosingAnaliticPublisher != null)
                ((LoosingAnaliticPublisher) loosingAnaliticPublisher).Dispose();
        }

        public override void InstallBindings()
        {
            Container.Bind<IStartBattleButton>().FromInstance(_startBattleButton).AsSingle();
            Container.Bind<IEnemiesSpawnedPublisher>().FromInstance(_enemySpawner).AsSingle();

            Container.Bind<IBattleStartPublisher>().To<BattleStarter>().AsSingle();
            Container.Bind<IBattleFinishPublisher>().To<BattleFinisher>().AsSingle();
            Container.Bind<IBattleContinuingFlag>().To<BattleContinuingFlag>().AsSingle();
            Container.Bind<ILoosingAnaliticPublisher>().To<LoosingAnaliticPublisher>().AsSingle();

            EnemiesPool enemiesPool = new EnemiesPool();
            Container.Bind<IEnemiesPool>().FromInstance(enemiesPool).AsSingle();
            Container.Bind<IAllEnemiesDeadPublisher>().FromInstance(enemiesPool).AsSingle();

            MinionsAiPool minionsAiPool = new MinionsAiPool();
            Container.Bind<IMinionsAiPool>().FromInstance(minionsAiPool).AsSingle();
            Container.Bind<IMinionsSetPositionsPublisher>().FromInstance(minionsAiPool).AsSingle();
        }
    }
}