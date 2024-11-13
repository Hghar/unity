namespace Realization.Installers.Abstract
{
    public interface IInstallablePool<TProduct>
    {
        void Add(TProduct newProduct);
    }
}