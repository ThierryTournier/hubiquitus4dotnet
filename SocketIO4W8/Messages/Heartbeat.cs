

namespace SocketIOClient.Messages
{
    public class Heartbeat : Message
    {
        public static string HEARTBEAT = "2::";

        public Heartbeat()
        {
            this.MessageType = SocketIOMessageTypes.Heartbeat;
        }

        public override string Encoded
        {
            get
            {
                return HEARTBEAT;
            }
        }

    }
}
