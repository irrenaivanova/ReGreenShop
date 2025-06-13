import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../Constants/baseUrl";

let connection: signalR.HubConnection;

export const connectToChatHub = async (
  accessToken: string,
  onReceive: (senderId: string, message: string) => void
) => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}/chathub`, {
      accessTokenFactory: () => accessToken,
    })
    .withAutomaticReconnect()
    .build();

  // Subscribe before start
  connection.on("ReceiveMessage", (senderId, text) => {
    console.log("🔔 ReceiveMessage:", senderId, text);
    onReceive(senderId, text);
  });

  // Lifecycle logs
  connection.onreconnecting((e) => console.warn("SignalR reconnecting", e));
  connection.onreconnected((id) => console.log("SignalR reconnected:", id));
  connection.onclose((e) => console.error("SignalR closed", e));

  try {
    await connection.start();
    console.log("✅ SignalR Connected, id=", connection.connectionId);
  } catch (err) {
    console.error("❌ SignalR Connection Error:", err);
  }
};

export const sendMessage = (receiverId: string, message: string) => {
  console.log("➤ sendMessage to", receiverId, message);
  if (connection.state === signalR.HubConnectionState.Connected) {
    connection
      .invoke("SendMessage", receiverId, message)
      .catch((e) => console.error("❌ SendMessage failed:", e));
  } else {
    console.warn("⚠️ sendMessage skipped, state=", connection.state);
  }
};
