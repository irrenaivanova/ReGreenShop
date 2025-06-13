// components/ChatWidget.tsx
import React, { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { connectToChatHub, sendMessage } from "../../services/signalrService";

interface Message {
  senderId: string;
  text: string;
}

const ADMIN_USER_ID = "2cd377ad-ffad-4c83-8455-07b7039733da";

const ChatWidget: React.FC = () => {
  const { user } = useAuth();
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [currentChatUserId, setCurrentChatUserId] = useState<string | null>(
    null
  );

  useEffect(() => {
    console.log("🛰️ ChatWidget mounting, user:", user);
    if (!user?.accessToken) return;

    connectToChatHub(user.accessToken, (senderId, text) => {
      setMessages((prev) => [...prev, { senderId, text }]);
      if (user.isAdmin) {
        setCurrentChatUserId(senderId);
        console.log("🔑 Admin chat now with user:", senderId);
      }
    });
  }, [user]);

  const handleSend = () => {
    console.log("🖊️ handleSend called. input:", input);
    if (!input.trim() || !user) return;

    const receiverId = user.isAdmin ? currentChatUserId! : ADMIN_USER_ID;
    console.log("📤 Sending to receiverId:", receiverId);
    sendMessage(receiverId, input);
    setMessages((prev) => [...prev, { senderId: "Me", text: input }]);
    setInput("");
  };

  return (
    <div
      style={{
        position: "fixed",
        bottom: 20,
        right: 20,
        width: 300,
        height: 400,
        border: "1px solid #ccc",
        borderRadius: 10,
        background: "#fff",
        display: "flex",
        flexDirection: "column",
        zIndex: 9999,
      }}
    >
      <div style={{ flex: 1, overflowY: "auto", padding: 10 }}>
        {messages.map((msg, i) => (
          <div key={i}>
            <b>{msg.senderId}:</b> {msg.text}
          </div>
        ))}
      </div>
      <div style={{ padding: 10, borderTop: "1px solid #ccc" }}>
        <input
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleSend()}
          style={{ width: "70%", marginRight: 5 }}
        />
        <button onClick={handleSend}>Send</button>
      </div>
    </div>
  );
};

export default ChatWidget;
