﻿using GameTogetherAPI.Models;
using GameTogetherAPI.Repository;

namespace GameTogetherAPI.Services {
    public class GameService : IGameService {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository) {
            _gameRepository = gameRepository;
        }

        /// <summary>
        /// Creates a new game in the database.
        /// </summary>
        /// <param name="game">The game object to add.</param>
        /// <returns>True if the game was successfully added; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the game owner does not exist.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<bool> CreateGameAsync(Game game) {
            try {
                return await _gameRepository.CreateGameAsync(game);
            }
            catch (InvalidOperationException) {
                throw;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while creating the game.", ex);
            }
        }

        /// <summary>
        /// Adds a user to a game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="userId">The ID of the user joining the game.</param>
        /// <returns>True if the user was successfully added; otherwise, false.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the game or user is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is already in the game.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<bool> JoinGameAsync(string gameId, string userId) {
            try {
                return await _gameRepository.JoinGameAsync(gameId, userId);
            }
            catch (KeyNotFoundException) {
                throw;
            }
            catch (InvalidOperationException) {
                throw;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while joining the game.", ex);
            }
        }

        /// <summary>
        /// Allows a user to leave a game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="userId">The ID of the user leaving the game.</param>
        /// <returns>True if the user successfully left the game; otherwise, false.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the game or user is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is not in the game.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<bool> LeaveGameAsync(string gameId, string userId) {
            try {
                return await _gameRepository.LeaveGameAsync(gameId, userId);
            }
            catch (KeyNotFoundException) {
                throw;
            }
            catch (InvalidOperationException) {
                throw;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while leaving the game.", ex);
            }
        }

        /// <summary>
        /// Retrieves all games.
        /// </summary>
        /// <returns>A list of game DTOs.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<IEnumerable<Game>> GetAllGamesAsync() {
            try {
                return await _gameRepository.GetAllGamesAsync();
            }
            catch (KeyNotFoundException) {
                throw;
            }
            catch (InvalidOperationException) {
                throw;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while getting all games.", ex);
            }
        }

        /// <summary>
        /// Retrieves a game by its ID.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>The game DTO if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the game is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<Game> GetGameByIdAsync(string gameId) {
            try {
                return await _gameRepository.GetGameByIdAsync(gameId);
            }
            catch (KeyNotFoundException) {
                throw;
            }
            catch (InvalidOperationException) {
                throw;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while fetching the specific game.", ex);
            }
        }
    }
}


