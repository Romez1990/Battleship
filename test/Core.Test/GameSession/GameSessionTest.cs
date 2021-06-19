using System.Collections.Immutable;
using System.Threading.Tasks;
using Core.Connection;
using Core.Field;
using Core.GameSession;
using NUnit.Framework;

namespace Core.Test.GameSession {
    [TestFixture]
    public class GameSessionTest {
        [SetUp]
        public async Task SetUp() {
            var randomShipPlacement = new RandomShipPlacement();

            var enemyConnector = new PlayerConnector((_, e) =>
                _enemySession = new(e.Socket, e.IsPlayerGoing));
            var connectionCode = await enemyConnector.CreateGame(new("", ""), _enemyShips = randomShipPlacement.GetShips());

            var playerConnector = new PlayerConnector((_, e) =>
                _playerSession = new(e.Socket, e.IsPlayerGoing));
            await playerConnector.ConnectToGame(new("", ""), randomShipPlacement.GetShips(),
                connectionCode.Code);
        }

        private Session _playerSession;
        private Session _enemySession;
        private ImmutableArray<Ship> _enemyShips;

        [Test]
        public async Task Collides_ReturnsTrue_WhenShipCollides1() {
            var shotResult = await _playerSession.Go(new(0, 0));

            Assert.IsFalse(shotResult.Hit);
        }

        [Test]
        public async Task Collides_ReturnsTrue_WhenShipCollides2() {
            var shipCoordinates = _enemyShips[0].Coordinates;

            var shotResult = await _playerSession.Go(shipCoordinates);

            Assert.IsTrue(shotResult.Hit);

            var question = shotResult.Question;
            var answerIndex = 0;
            var answerResult = await _playerSession.Answer(answerIndex);
        }
    }
}
