using NetCoreServer;
using System;
using System.Net.Sockets;
using System.Threading;

namespace CardinalEngine {

    internal class WClient : WsClient {
        private NetworkHandler _networkHandler = new NetworkHandler();

        public WClient(string address, int port) : base(address, port) {
            _networkHandler.ClientSendData = SendData;
        }

        private void SendData(byte[] data) {
            SendBinaryAsync(data);
        }

        public static void Run() {
            WClient wClient = new WClient("127.0.0.1", 8080);
            wClient.ConnectAsync();
        }

        public void DisconnectAndStop() {
            _stop = true;
            CloseAsync(1000);
            while (IsConnected)
                Thread.Yield();
        }

        public override void OnWsConnecting(HttpRequest request) {
            request.SetBegin("GET", "/");
            request.SetHeader("Host", "localhost");
            request.SetHeader("Origin", "http://localhost");
            request.SetHeader("Upgrade", "websocket");
            request.SetHeader("Connection", "Upgrade");
            request.SetHeader("Sec-WebSocket-Key", Convert.ToBase64String(WsNonce));
            request.SetHeader("Sec-WebSocket-Protocol", "chat, superchat");
            request.SetHeader("Sec-WebSocket-Version", "13");
            request.SetBody();
        }

        public override void OnWsConnected(HttpResponse response) {
        }

        public override void OnWsDisconnected() {
        }

        public override void OnWsReceived(byte[] buffer, long offset, long size) {
            byte[] data = new byte[size];
            Array.Copy(buffer, offset, data, 0, size);
            _networkHandler.OnData(data);
        }

        protected override void OnDisconnected() {
            base.OnDisconnected();
        }

        protected override void OnError(SocketError error) {
        }

        private bool _stop;
    }
}