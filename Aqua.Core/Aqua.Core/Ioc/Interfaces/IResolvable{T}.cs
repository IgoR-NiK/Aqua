using Aqua.Core.Utils;

namespace Aqua.Core.Ioc
{
    public interface IResolvable<in TParam> : IResolvable, IWithInit<TParam>
    {
    }
}