namespace Aqua.Core.Interfaces
{
    public interface IWithInit<in TParam>
    {
        void Init(TParam param);
    }
}