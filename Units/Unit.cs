using System.Collections.Generic;
using Parameters;
using UnityEngine;

namespace Units
{
    public class Unit : IUnit
    {
        private UnitParameters _unitParameters;

        public IUnitParameters Parameters => _unitParameters;

        public Unit(UnitParameters unitParameters)
        {
            _unitParameters = unitParameters;
        }

        public bool TryApplyModificators(IReadOnlyList<IParamModificator> paramsModificators)
        {
            // TODO: remeber commands
            return _unitParameters.TryApplyModificators(paramsModificators);
        }
    }
}