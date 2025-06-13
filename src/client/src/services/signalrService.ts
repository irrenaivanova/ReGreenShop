// services/signalrService.ts
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

  // 1) subscribe before start
  connection.on("ReceiveMessage", (senderId, text) => {
    console.log("🔔 ReceiveMessage fired:", { senderId, text });
    onReceive(senderId, text);
  });

  // 2) log lifecycle
  connection.onreconnecting((error) =>
    console.warn("SignalR reconnecting", error)
  );
  connection.onreconnected((id) =>
    console.log("SignalR reconnected, connectionId:", id)
  );
  connection.onclose((error) => console.error("SignalR closed", error));

  // 3) start
  try {
    await connection.start();
    console.log(
      "✅ SignalR connected with connectionId=",
      connection.connectionId
    );
  } catch (err) {
    console.error("❌ SignalR Connection Error:", err);
  }
};

export const sendMessage = (receiverId: string, message: string) => {
  console.log("➤ Invoking SendMessage:", { receiverId, message });
  if (connection?.state === signalR.HubConnectionState.Connected) {
    connection
      .invoke("SendMessage", receiverId, message)
      .catch((err) => console.error("❌ SendMessage failed:", err));
  } else {
    console.warn("⚠️ Cannot send, connection state:", connection?.state);
  }
};
