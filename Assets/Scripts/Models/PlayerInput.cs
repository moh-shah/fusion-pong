using Fusion;

namespace PhotoPong.Models
{
    public struct PlayerInput : INetworkInput
    {
        public bool upKey;
        public bool downKey;
        public bool qKey;
        public bool wKey;
        public bool eKey;
    }
}