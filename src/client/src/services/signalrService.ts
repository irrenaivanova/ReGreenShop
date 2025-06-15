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
  if (connection) {
    // Only start if disconnected
    if (connection.state === signalR.HubConnectionState.Disconnected) {
      return connection.start().catch((err) => {
        console.error("Failed to start connection:", err);
      });
    }
    // Already connected or connecting
    return Promise.resolve();
  }

  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}/chatHub`, {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();

  connection.on("ReceiveMessage", (senderId, senderName, text) => {
    onMessageReceived(senderId, senderName, text);
  });

  return connection.start().catch((err) => {
    console.error("Failed to start connection:", err);
  });
}

export function disconnectFromChatHub() {
  if (connection) {
    connection.stop();
    connection = null;
  }
}

export function sendMessage(receiverId: string, message: string) {
  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    connection.invoke("SendMessage", receiverId, message).catch((e) => {
      console.error("SendMessage failed:", e);
    });
  } else {
    console.warn(
      "Connection is null or not connected. State =",
      connection?.state
    );
  }
}

export const getConnectedUsers = async (): Promise<
  { userId: string; userName: string }[]
> => {
  if (!connection) throw new Error("Connection not established");

  try {
    const users = await connection.invoke("GetConnectedUsers");
    return users;
  } catch (err) {
    console.error("Error getting connected users:", err);
    return [];
  }
};
