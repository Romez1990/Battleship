using System;
using System.Collections.Immutable;
using System.Linq;
using System.Windows;
using Core.Field;
using Core.GameSession;
using Core.PlayerData;
using Websocket.Client;
using WpfApp.GameBattlefield;
using WpfApp.Toolkit;

namespace WpfApp.GameSession {
    public class GameSessionViewModel : ViewModel {
        public GameSessionViewModel(WebsocketClient socket, Player player, Player enemy,
            ImmutableArray<Ship> ships, bool isPlayerGoing) {
            _session = new(socket, isPlayerGoing, OnGetEnemyShot, OnGetEnemyAnswer, OnPlayerTurn);
            Player = player;
            Enemy = enemy;
            Ships = ships;
            _score = new();
            PlayerBattlefield = new(ships);
            EnemyBattlefield = new(Enumerable.Empty<Ship>());
            EnemyCanvasClick = new(OnEnemyCanvasClick);
            CheckAnswer = new(OnCheckAnswer);
            Answer = new(OnAnswer);
        }

        private readonly Session _session;

        public RelayCommand EnemyCanvasClick { get; }

        public Player Player { get; }
        public Player Enemy { get; }
        public ImmutableArray<Ship> Ships { get; }

        private Score _score;

        public Score Score {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        public string WhoIsGoing => _session.IsPlayerGoing ? "Ваш ход" : "Противник ходит";

        public Battlefield PlayerBattlefield { get; }
        public Battlefield EnemyBattlefield { get; }

        public int EnemyCanvasPositionX { get; set; }
        public int EnemyCanvasPositionY { get; set; }


        private Visibility _showQuestion = Visibility.Hidden;

        public Visibility ShowQuestion {
            get => _showQuestion;
            set => SetProperty(ref _showQuestion, value);
        }

        private Question _currentQuestion;

        public Question CurrentQuestion {
            get => _currentQuestion;
            set => SetProperty(ref _currentQuestion, value);
        }

        private string _answer1;

        public string Answer1 {
            get => _answer1;
            set => SetProperty(ref _answer1, value);
        }

        private string _answer2;

        public string Answer2 {
            get => _answer2;
            set => SetProperty(ref _answer2, value);
        }

        private string _answer3;

        public string Answer3 {
            get => _answer3;
            set => SetProperty(ref _answer3, value);
        }

        private string _answer4;

        public string Answer4 {
            get => _answer4;
            set => SetProperty(ref _answer4, value);
        }

        private void OnEnemyCanvasClick() {
            if (!_session.IsPlayerGoing || ShowQuestion == Visibility.Visible) return;

            PlayerBattlefield.CalculateCoordinates(new(EnemyCanvasPositionX, EnemyCanvasPositionY))
                .IfSome(async coordinates => {
                    if (EnemyBattlefield.CrossAlreadyExists(coordinates)) return;
                    var shotResult = await _session.Go(coordinates);
                    OnPropertyChanged(nameof(WhoIsGoing));

                    EnemyBattlefield.AddCross(coordinates);

                    if (shotResult.Hit) {
                        Score = Score.AddPointToPlayer();

                        ShowQuestion = Visibility.Visible;
                        CurrentQuestion = shotResult.Question;
                        Answer1 = CurrentQuestion.Answers[0];
                        Answer2 = CurrentQuestion.Answers[1];
                        Answer3 = CurrentQuestion.Answers[2];
                        Answer4 = CurrentQuestion.Answers[3];
                    }

                    if (shotResult.Destroyed) {
                        EnemyBattlefield.AddShip(shotResult.DestroyedShip);
                    }
                });
        }

        private int _answerIndex;

        public RelayCommand<string> CheckAnswer { get; }

        private void OnCheckAnswer(string answerIndex) =>
            _answerIndex = int.Parse(answerIndex);

        public RelayCommand Answer { get; }

        private async void OnAnswer() {
            var result = await _session.Answer(_answerIndex);
            if (result.Right) {
                Score = Score.AddPointToPlayer();
            }

            ShowQuestion = Visibility.Hidden;
        }

        private void OnGetEnemyShot(object sender, GetEnemyShotEventArgs e) {
            PlayerBattlefield.AddCross(e.Coordinates);
            if (e.Hit) {
                Score = Score.AddPointToEnemy();
            }
        }

        private void OnGetEnemyAnswer(object sender, GetEnemyAnswerEventArgs e) {
            if (e.Right) {
                Score = Score.AddPointToEnemy();
            }
        }

        private void OnPlayerTurn(object sender, EventArgs e) {
            OnPropertyChanged(nameof(WhoIsGoing));
        }
    }
}
