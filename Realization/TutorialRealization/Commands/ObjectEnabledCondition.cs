using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Helpers;
using Model.Economy;
using Plugins.Ship.Sheets.StepSheet.Commands.Conditions;
using Realization.TutorialRealization.Helpers;
using Units;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class ObjectEnabledCondition : ICondition
    {
        private readonly IObjectProvider<GameObject> _obj;
        private bool _enabled;
        private GameObject _obj1;
        private UniTask<bool> _uniTaskFalse;
        private UniTask<bool> _uniTaskTrue;
        private int _lastObjects;

        public ObjectEnabledCondition(IObjectProvider<GameObject> obj, bool enabled)
        {
            _enabled = enabled;
            _obj = obj;
            _uniTaskFalse = new UniTask<bool>(false);
            _uniTaskTrue = new UniTask<bool>(true);
        }

        public UniTask<bool> Met()
        {
            if(_obj1 == null && _lastObjects != SceneObjectPool.Instance.Objects.Count)
            {
                _obj1 = _obj.Get();
                
                _lastObjects = SceneObjectPool.Instance.Objects.Count;
            }
            
            if (_obj1 == null)
            {
                return _uniTaskFalse;
            }

            if (_obj1.gameObject.activeInHierarchy == _enabled)
            {
                return _uniTaskTrue;
            }

            return _uniTaskFalse;
        }
    }
    
    public class UnitOnCellCondition : ICondition
    {
        private IObjectProvider<GameObject> _unit;
        private Vector2Int _position;

        public UnitOnCellCondition(IObjectProvider<GameObject> unit, Vector2Int position)
        {
            _position = position;
            _unit = unit;
        }

        public UniTask<bool> Met()
        {
            if (_unit.Get() == null)
                return new UniTask<bool>(false);

            return new UniTask<bool>(_unit.Get().GetComponent<IMinion>().Position == _position);
        }
    }
    
    public class UnitMovedCondition : ICondition
    {
        private DelayedObject _unit;
        private Vector2Int _position;
        private bool _endTask;

        public UnitMovedCondition(DelayedObject unit, Vector2Int position)
        {
            _position = position;
            _unit = unit;
        }

        public async UniTask<bool> Met()
        {
            var minionObject = await _unit.GetAsync();
            var minion = minionObject.GetComponent<IMinion>();
            minion.Dragged += EndWaiting;
            minion.Disposed += EndWaiting;
            while (_endTask == false)
            {
                await UniTask.WaitForFixedUpdate();
            }

            minion.Position = _position;
            minion.UpdateWorldPosition(MoveType.Instantly);
            return true;
        }

        private void EndWaiting()
        {
            _endTask = true;
        }
    }
    
    public class GetKeyCondition : ICondition
    {
        private IStorage _storage;
        private int _value;

        public GetKeyCondition(IStorage storage, int value)
        {
            _value = value;
            _storage = storage;
        }

        public UniTask<bool> Met()
        {
            return new UniTask<bool>(_storage.PlayerProgress.TutorialData.Key == _value);
        }
    }
}