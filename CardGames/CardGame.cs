﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CardGames
{
    public abstract class CardGame
    {
        private readonly List<Player> players = new List<Player>();

        /// <summary>
        /// Game status.
        /// </summary>
        public GameStatus Status { get; private set; }

        /// <summary>
        /// Players.
        /// </summary>
        public IReadOnlyList<Player> Players => players;

        /// <summary>
        /// Players number.
        /// </summary>
        public int PlayersNumber => players.Count;

        /// <summary>
        /// Players list as text.
        /// </summary>
        public string PlayersListText { get; private set; }

        /// <summary>
        /// Min players number.
        /// </summary>
        public abstract int MinPlayersNumber { get; }

        /// <summary>
        /// Max players number.
        /// </summary>
        public abstract int MaxPlayersNumber { get; }

        /// <summary>
        /// State changed event.
        /// </summary>
        public event EventHandler OnStateChanged;

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            if (!IsReadyToStart(out string text))
            {
                throw new Exception(text);
            }

            Status = GameStatus.Started;

            GameStarted();
        }

        /// <summary>
        /// Adds a player.
        /// </summary>
        public void AddPlayer(Player player)
        {
            if (!players.Contains(player))
            {
                if (IsAllowedToEnter(player))
                {
                    player.PropertyChanged += OnPlayerPropertyChanged;
                    players.Add(player);
                    DoPlayersListChanged();
                }
            }
        }

        /// <summary>
        /// Removes a player.
        /// </summary>
        public void RemovePlayer(Player player)
        {
            try
            {
                if (players.Contains(player))
                {
                    player.PropertyChanged -= OnPlayerPropertyChanged;
                    players.Remove(player);
                    DoPlayersListChanged();
                }
            }
            catch (Exception x)
            {
            }

            // TODO: check what happens if the game has started.
        }

        #region Virtual abstract members

        /// <summary>
        /// Check if the player is allowed to enter.
        /// </summary>
        protected virtual bool IsAllowedToEnter(Player player)
        {
            return PlayersNumber < MaxPlayersNumber;
        }

        /// <summary>
        /// Game started.
        /// </summary>
        protected abstract void GameStarted();

        #endregion

        /// <summary>
        /// Check if the game is ready to start.
        /// </summary>
        protected bool IsReadyToStart(out string text)
        {
            if (Status != GameStatus.Building)
            {
                text = "El juego debe estar en construcción para poder empezar.";
                return false;
            }

            if (PlayersNumber < MinPlayersNumber)
            {
                text = "No se alcanzó el número mínimo de jugadores.";
                return false;
            }

            text = string.Empty;
            return true;
        }

        private void OnPlayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DoPlayersListChanged();
        }

        private void DoPlayersListChanged()
        {
            PlayersListText = string.Join(",", players.Select(i => i.NickName));
            DoStateChanged();
        }

        private void DoStateChanged()
        {
            OnStateChanged?.Invoke(this, null);
        }
    }
}