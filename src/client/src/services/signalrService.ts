import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../Constants/baseUrl";

let connection: signalR.HubConnection | null = null;

export function connectToChatHub(
  token: string,
  onMessageReceived: (
    senderId: string,
    senderName: string,
    text: string
  ) => void
) {
  if (connection) return;

  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}/chatHub`, {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();

  connection.on("ReceiveMessage", onMessageReceived);

  connection
    .start()
    .then(() => console.log("SignalR connected"))
    .catch((err) => console.error("SignalR connection failed:", err));
}

export function disconnectFromChatHub() {
  if (connection) {
    connection
      .stop()
      .then(() => {
        console.log("SignalR disconnected");
        connection = null;
      })
      .catch((err) => console.error("Error disconnecting SignalR:", err));
  }
}

export function sendMessage(receiverId: string, message: string) {
  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    connection
      .invoke("SendMessage", receiverId, message)
      .catch((e) => console.error("SendMessage failed:", e));
  } else {
    console.warn(
      "Connection is null or not connected. State =",
      connection?.state
    );
  }
}
