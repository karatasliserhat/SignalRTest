﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly AppDbContext _context;

        public SignalRHub(AppDbContext context)
        {
            _context = context;
        }


        private static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        public static int TeamCount { get; set; }


        public async Task SendProduct(Product product)
        {
            await Clients.All.SendAsync("ReceiveProduct",product);
        }
        public async Task SendName(string name)
        {
            if (Names.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Takım En  Fazla {TeamCount} Kişi olabilir");
            }
            else
            {
                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name);
            }

        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names);
        }

        //Groups Codes
        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task RemoveToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _context.Teams.Where(x => x.Name == teamName).FirstOrDefault();
            if (team != null)
            {
                team.Users.Add(new User { Name = name });
            }
            else
            {
                var newTeam = new Team { Name = teamName };
                newTeam.Users.Add(new User { Name = name });
                _context.Teams.Add(newTeam);

            }
            await _context.SaveChangesAsync();

            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup", name, team.Id);
        }
        public async Task GetNamesByGroup()
        {
            var teams = _context.Teams.Include(x => x.Users).Select(x => new
            {
                teamId = x.Id,
                users = x.Users.ToList()
            }).ToList();
            await Clients.All.SendAsync("ReceiveNamesByGroup", teams);
        }
        //Groups Codes End

        public override async Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
