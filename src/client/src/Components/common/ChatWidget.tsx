import React, { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { connectToChatHub, sendMessage } from "../../services/signalrService";

interface Message {
  senderId: string;
  text: string;
}

const ChatWidget: React.FC = () => {
  const { user } = useAuth();
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [currentChatUserId, setCurrentChatUserId] = useState<string | null>(
    null
  );

  useEffect(() => {
    console.log("🛰️ ChatWidget mount, user:", user);
    if (!user?.accessToken) return;

    connectToChatHub(user.accessToken, (senderId, text) => {
      setMessages((m) => [...m, { senderId, text }]);

      // If admin, track who’s talking so replies go back
      if (user.isAdmin) {
        setCurrentChatUserId(senderId);
        console.log("🔑 Admin chatting with:", senderId);
      }
    });
  }, [user]);

  const handleSend = () => {
    if (!input.trim() || !user) return;

    // If I’m admin, send to tracked user; else send to admin’s GUID
    const receiverId = user.isAdmin
      ? currentChatUserId!
      : user.isAdmin === false
      ? /* your admin GUID here */ "2cd377ad-ffad-4c83-8455-07b7039733da"
      : "";

    console.log("📤 handleSend →", receiverId, input);
    sendMessage(receiverId, input);

    setMessages((m) => [...m, { senderId: "Me", text: input }]);
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
        {messages.map((m, i) => (
          <div key={i}>
            <b>{m.senderId}:</b> {m.text}
          </div>
        ))}
      </div>
      <div style={{ padding: 10, borderTop: "1px solid #ccc" }}>
        <input
          style={{ width: "70%", marginRight: 5 }}
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleSend()}
        />
        <button onClick={handleSend}>Send</button>
      </div>
    </div>
  );
};

export default ChatWidget;
