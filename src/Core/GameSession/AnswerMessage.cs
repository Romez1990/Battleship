using Core.Sockets;

namespace Core.GameSession {
    public record AnswerMessage : Message<AnswerData> {
        public AnswerMessage(int answerIndex) : base("answer", new(answerIndex)) { }

    }

    public record AnswerData(int AnswerIndex);
}
