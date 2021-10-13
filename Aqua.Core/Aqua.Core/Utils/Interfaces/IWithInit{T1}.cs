namespace Aqua.Core.Utils
{
    public interface IWithInit<in TParam>
    {
        void Init(TParam param);
    }
}