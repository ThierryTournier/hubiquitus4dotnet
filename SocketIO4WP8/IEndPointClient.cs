using System;

namespace SocketIOClient
{
	public interface IEndPointClient
	{
		void On(string eventName, Action<SocketIOClient.Messages.IMessage> action);
		void Emit(string eventName, dynamic payload, Action<dynamic> callBack = null);

		void Send(SocketIOClient.Messages.IMessage msg);
	}
}
