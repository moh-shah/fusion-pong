using Fusion;

namespace PhotoPong.Models
{
    public struct PlayerInput : INetworkInput
    {
        public bool up;
        public bool down;
    }
}