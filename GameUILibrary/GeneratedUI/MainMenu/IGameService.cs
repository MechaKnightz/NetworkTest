using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLibrary;

namespace GameUILibrary
{
    public interface IGameService
    {
        void Exit();
        bool Connect(string username, string password, string ip, string port);
        bool Register(string username, string password, string ip, string port);
        bool JoinRoom(string roomName);
        void ChangeResolution(int width, int heigh);
        Resolution GetResolution();
        List<Resolution> GetSupportedResolutions();
    }
}
