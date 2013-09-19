using SocketIOClient.Messages;
using System;

namespace SocketIOClient
{
	public class MessageEventArgs : EventArgs
	{
		public IMessage Message { get; private set; }

		public MessageEventArgs(IMessage msg)
			: base()
		{
			this.Message = msg;
		}
	}
}
