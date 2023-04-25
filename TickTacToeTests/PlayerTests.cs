using System.Security.Cryptography.X509Certificates;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToeTests
{
    public class PlayerTests
    {
        const char DEFAULT_TOKEN = 'X';
        private readonly char[] VALID_TOKENS = new char[] { 'X', 'O' };

        Player _player;

        public PlayerTests() {
            _player = new Player("Test", DEFAULT_TOKEN);
        }

        [Fact]
        public void PlayerClassCanBeConstructed()
        {
            Assert.NotNull(_player);
        }

        [Fact]
        public void PlayerClassIsOfPlayerInterface()
        {
            Assert.IsAssignableFrom<IPlayer>(_player);
        }

        [Fact]
        public void PlayerClassAcceptsNameAndTokenParamsInConstructor()
        {
            _player = new Player(name:"Daniel",token:DEFAULT_TOKEN);
            Assert.NotNull(_player);
        }

        [Fact]
        public void PlayerHasName()
        {
            Assert.NotNull(_player.Name);
            Assert.NotEqual(0, _player.Name.Length);
        }

        [Fact]
        public void PlayerHasToken()
        {
            Assert.NotNull(_player.Token);
            Assert.Equal(DEFAULT_TOKEN, _player.Token);
        }

        [Fact]
        public void PlayerConstructorDisallowsInvalidToken()
        {
            for (char character = char.MinValue; character < char.MaxValue; character++)
            {
                var tokenIsValid = VALID_TOKENS.Contains(character);
                if (tokenIsValid) {
                    var newPlayer = new Player("PlayerName", character); // Make sure this works fine
                } 
                else
                {
                    Assert.Throws<ArgumentException>(() => new Player("PlayerName", character)); // Make sure this fails
                }    
            }
        }
    }
}
