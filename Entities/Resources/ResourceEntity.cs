using System;
using Infrastructure.CompositeDirector.Executors;
using Model.Composites.Representation;
using Model.Composites.Savable;
using Model.Economy;
using Model.Economy.Resources;
using TMPro;
using UnityEngine;

namespace Entities.Resources
{
    public class ResourceEntity : IRepresentation, ISavable
    {
        private readonly TMP_Text _view;
        private readonly IResource _resource;

        public event Action<IProcessExecutor> Disposed;

        public ResourceEntity(TMP_Text view, IResource resource)
        {
            _view = view;
            _resource = resource;
            _resource.ValueChanged += OnValueChanged;
        }

        public void Dispose()
        {
            _resource.ValueChanged -= OnValueChanged;
            _resource.Dispose();
            Disposed?.Invoke(this);
        }

        public void Represent()
        {
            if (_view == null)
                return;

            if (_resource.Currency == Currency.Gold)
                _view.text = $"<sprite=1>{_resource.Value}";
            else
                _view.text = $"<sprite=2>{_resource.Value}";
        }

        public void Save()
        {
            if (PlayerPrefs.GetInt("NoSaveMoney") == 0 && PlayerPrefs.GetInt("level") > 1)
            {
                PlayerPrefs.SetInt(_resource.Currency.ToString(), _resource.Value);
            }
        }

        private void OnValueChanged(int different)
        {
            Represent();
        }
    }
}