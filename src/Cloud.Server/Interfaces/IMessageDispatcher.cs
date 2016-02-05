namespace Cloud.Server.Interfaces
{
    public interface IMessageDispatcher
    {
        void SendMessage(object state);
    }
}