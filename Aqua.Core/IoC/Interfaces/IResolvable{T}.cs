using Aqua.Core.Utils;

namespace Aqua.Core.IoC
{
    public interface IResolvable<in TParam> : IResolvable, IWithInit<TParam>
    {
    }
}