namespace Parameters
{
    public interface IUnitParam
    {
        bool TryApplyModificator(IParamModificator modificator);
    }
}