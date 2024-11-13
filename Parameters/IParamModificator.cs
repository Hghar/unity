namespace Parameters
{
    public interface IParamModificator : IArithmeticAction
    {
        public ParamType Parameter { get; }
    }
}