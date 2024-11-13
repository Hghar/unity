using DG.Tweening;
using Infrastructure.CompositeDirector;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using Model.Composites.Savable;
using Model.Maps;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.General
{
    public class SceneCycle : MonoBehaviour
    {
        [SerializeField] private AstarPath _path;

        private Composite<IRepresentation> _representation;
        private Composite<ISavable> _savable;
        private IMap _map;
        private CompositeDirector _director;

        [Inject]
        private void Construct(CompositeDirector director, Composite<IRepresentation> representation,
            Composite<ISavable> savable, IMap map)
        {
            _director = director;
            _map = map;
            _savable = savable;
            _representation = representation;
            _map.Moved += ScanPath;
        }

        private void Start()
        {
            _path.Scan();

            //if(SceneManager.GetActiveScene().name == "FightTest")
                //Represent();

            if (!PlayerPrefs.HasKey("NoSaveMoney"))
            {
                PlayerPrefs.SetInt("NoSaveMoney", 1);
            }
        }

        private void OnDestroy()
        {
            _savable.Select().Do().Save();
            _map.Moved -= ScanPath;
            _director.Dispose();
            DOTween.KillAll();
        }

        private void ScanPath()
        {
            _path.Scan();
        }

        [Button]
        private void Represent()
        {
            _representation.Select().Do().Represent();
        }
    }
}