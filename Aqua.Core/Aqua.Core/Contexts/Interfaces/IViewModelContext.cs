namespace Aqua.Core.Contexts
{
    public interface IViewModelContext : IContext
    {
        string Title { get; set; }
    }
}