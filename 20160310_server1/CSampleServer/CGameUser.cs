using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;


namespace CSampleServer
{
	using GameServer;

	/// <summary>
	/// 하나의 session객체를 나타낸다.
	/// </summary>
	class CGameUser : IPeer
	{
		CUserToken token;

		public CGameUser(CUserToken token)
		{
			this.token = token;
			this.token.set_peer(this);
		}

		void IPeer.on_message(Const<byte[]> buffer)
		{
			// ex)
			CPacket msg = new CPacket(buffer.Value, this);
			PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();
			Console.WriteLine("------------------------------------------------------");
			Console.WriteLine("protocol id " + protocol);
			switch (protocol)
			{
				case PROTOCOL.CHAT_MSG_REQ:
					{
						string text = msg.pop_string();
						Console.WriteLine(string.Format("INPUT1 COMMAND :  {0}", text));

                        ///이곳에 디비에 접근할 수 있는 코드를 넣어야함
                        ///메쏘드로 따로 구현을 해야겟다
                        ///odpconn.cs

                        SAccessDB.odpconn(text);


                        /// 아래 코드는 뭘까... 다시 send를 하나??
                        /// 클라이언트한테 응답을 주는가 보다.
                        /// 응답 코드 맞음.. 확인함..이 샘플 서버는 에코 서버임..
						CPacket response = CPacket.create((short)PROTOCOL.CHAT_MSG_ACK);
                        Console.WriteLine(SAccessDB.makexml());

                        /// xml string의 길이는 미리 알 수 없음으로
                        /// 1024 단위로 짤라서 보내야 함
                        /// while문을 만들자...ㅎ 
                        /// 세션이 연결되어 있으니.. 상관없지 않으려나? 
                        /// 
						response.push(SAccessDB.makexml());
						send(response);
					}
					break;
			}
		}

		void IPeer.on_removed()
		{
			Console.WriteLine("The client disconnected.");

			Program.remove_user(this);
		}

		public void send(CPacket msg)
		{
			this.token.send(msg);
		}

		void IPeer.disconnect()
		{
			this.token.socket.Disconnect(false);
		}

		void IPeer.process_user_operation(CPacket msg)
		{
		}
	}
}
