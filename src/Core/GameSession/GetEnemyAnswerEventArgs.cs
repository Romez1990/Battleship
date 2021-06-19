using System;

namespace Core.GameSession {
    public class GetEnemyAnswerEventArgs : EventArgs {
        public GetEnemyAnswerEventArgs(GetEnemyAnswer enemyAnswer) {
            Right = enemyAnswer.Right;
        }
        
        public bool Right { get; }
    }
}
