namespace Parameters
{
    public abstract class UnitParam : IUnitParam
    {
        private ParamType _paramType;

        protected ParamType ParamType => _paramType;

        public UnitParam(ParamType paramType)
        {
            _paramType = paramType;
        }

        public bool TryApplyModificator(IParamModificator modificator)
        {
            if (modificator.Parameter != _paramType)
                return false;

            ApplyModificator(modificator);
            return true;
        }

        protected abstract void ApplyModificator(IParamModificator modificator);
    }
}