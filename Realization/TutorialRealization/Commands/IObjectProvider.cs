using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Realization.TutorialRealization.Commands
{
    public interface IObjectProvider<T>
    {
        string Name { get; }
        T Get();
        UniTask<T> GetAsync();
    }
}