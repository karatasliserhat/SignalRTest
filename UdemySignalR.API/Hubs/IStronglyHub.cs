using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public interface IStronglyHub
    {
        Task ReceiveProduct(Product p);
        Task Error(string errorName);

        Task ReceiveName(string name);
        Task ReceiveNames(List<string> Names);
        Task ReceiveMessageByGroup(string name, int teamId);
        Task ReceiveNamesByGroup(IEnumerable<Team> teams);
        Task ReceiveClientCount(int ClientCount);
    }
}
