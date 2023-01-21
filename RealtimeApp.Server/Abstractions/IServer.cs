namespace RealtimeApp.Server.Abstractions;

internal interface IServer : IDataSender
{
    void Start();
}