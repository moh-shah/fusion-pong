using Fusion;
using PhotoPong.Managers;

namespace PhotoPong.Models
{
    public struct MatchResults : INetworkStruct
    {
        public WorldDirection winnerSide;
        public int winnerScore;
        public int loserScore;
    }
}