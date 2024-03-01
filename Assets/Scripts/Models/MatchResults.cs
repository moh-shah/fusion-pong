using Fusion;

namespace PhotoPong.Models
{
    public struct MatchResults : INetworkStruct
    {
        public bool draw;
        public WorldDirection winnerSide;
        public int winnerScore;
        public int loserScore;
        public int durationInSeconds;
    }
}