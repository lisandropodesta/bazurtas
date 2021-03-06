﻿namespace CardGames.Bazas
{
    public class BazasScoreboardRoundPlayer
    {
        public byte? Bid { get; set; }

        public byte? Bazas { get; set; }

        public byte? Score { get; set; }

        public byte PrevScore { get; set; }

        public string CurrScore => (Score ?? PrevScore).ToString();

        public override string ToString()
        {
            return (Bazas.HasValue ? Bazas.ToString() + "/" + Bid : string.Empty) + (Score.HasValue ? "  " + Score : string.Empty);
        }
    }
}
