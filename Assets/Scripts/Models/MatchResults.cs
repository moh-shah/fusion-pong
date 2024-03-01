using Fusion;

namespace PhotoPong.Models
{
    public struct MatchResults : INetworkStruct
    {
        public WorldDirection winnerSide;
        public int winnerScore;
        public int loserScore;
        public int durationInSeconds;
    }
}