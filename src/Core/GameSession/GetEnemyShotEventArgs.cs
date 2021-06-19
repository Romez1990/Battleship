using System;
using Core.Geometry;

namespace Core.GameSession {
    public class GetEnemyShotEventArgs : EventArgs {
        public GetEnemyShotEventArgs(GetEnemyShot enemyShot) {
            Coordinates = enemyShot.Coordinates;
            Hit = enemyShot.Hit;
        }

        public Vector Coordinates { get; }
        public bool Hit { get; }
    }
}
