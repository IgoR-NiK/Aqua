using Aqua.Core.Ioc;

namespace Aqua.Core.Services
{
    public interface INavigationModule : IResolvable
    {
        void Mapping(INavigationMapper navigationMapper);
    }
}