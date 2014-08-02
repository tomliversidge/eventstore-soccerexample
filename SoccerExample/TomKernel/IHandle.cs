namespace TomKernel
{
    public interface IHandle<T> where T : IMessage
    {
        void Handle(T args);
    }
}