namespace Aqua.Core.Builders
{
    public interface IBuilder<out T, out TSelf>
        where T : new()
        where TSelf : IBuilder<T, TSelf>
    {
        public T Instance { get; }

        TSelf Clear();
    }
}