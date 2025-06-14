import React, { useEffect, useState, useRef } from "react";
import { useAuth } from "../../context/AuthContext";
import { connectToChatHub, sendMessage } from "../../services/signalrService";

interface Message {
  senderId: string;
  text: string;
}

const ADMIN_GUID = "2cd377ad-ffad-4c83-8455-07b7039733da";

const ChatWidget: React.FC = () => {
  const { user } = useAuth();
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [currentChatUserId, setCurrentChatUserId] = useState<string | null>(
    null
  );
  const [isOpen, setIsOpen] = useState(false);
  const [unreadCount, setUnreadCount] = useState(0);

  const messagesEndRef = useRef<HTMLDivElement>(null);
  const hasConnectedRef = useRef(false); // Prevent duplicate subscription

  useEffect(() => {
    if (!user?.accessToken || hasConnectedRef.current) return;

    hasConnectedRef.current = true;

    connectToChatHub(user.accessToken, (senderId, text) => {
      setMessages((prev) => {
        const updated = [...prev, { senderId, text }];

        if (!isOpen) {
          setUnreadCount((count) => count + 1);
        }
        return updated;
      });

      if (user.isAdmin) {
        setCurrentChatUserId(senderId);
      }
    });
  }, [user]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });

    if (isOpen) {
      setUnreadCount(0); // Mark as read when opened
    }
  }, [messages, isOpen]);

  const handleSend = () => {
    if (!input.trim() || !user) return;

    const receiverId = user.isAdmin ? currentChatUserId! : ADMIN_GUID;

    sendMessage(receiverId, input);
    setMessages((prev) => [...prev, { senderId: "Me", text: input }]);
    setInput("");
  };

  const toggleChat = () => {
    const willOpen = !isOpen;
    setIsOpen(willOpen);

    if (willOpen) {
      setUnreadCount(0);
    }
  };

  if (!user) return null;

  const bannerText = user.isAdmin
    ? "Admin Chat"
    : "Do you have a question? Contact us";

  return (
    <div
      style={{ position: "fixed", bottom: 20, right: 20, zIndex: 1050 }}
      className="shadow"
    >
      <div
        role="button"
        onClick={toggleChat}
        className={`p-2 text-center bg-primary text-white rounded position-relative ${
          isOpen ? "rounded-bottom-0" : ""
        }`}
        style={{ cursor: "pointer", width: 280 }}
      >
        {bannerText}
        {!isOpen && unreadCount > 0 && (
          <span
            className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
            style={{ fontSize: "0.7rem" }}
          >
            {unreadCount}
          </span>
        )}
      </div>

      {isOpen && (
        <div
          className="bg-white border rounded-bottom d-flex flex-column"
          style={{ width: 280, height: 400 }}
        >
          <div
            className="flex-grow-1 overflow-auto p-3"
            style={{ fontSize: "0.9rem" }}
          >
            {messages.length === 0 && (
              <div className="text-muted text-center mt-3">
                No messages yet.
              </div>
            )}
            {messages.map((m, i) => (
              <div
                key={i}
                className={`mb-2 ${
                  m.senderId === "Me" ? "text-end" : "text-start"
                }`}
              >
                <b>{m.senderId === "Me" ? "Me" : m.senderId}:</b>{" "}
                <span>{m.text}</span>
              </div>
            ))}
            <div ref={messagesEndRef} />
          </div>
          <div className="p-2 border-top d-flex">
            <input
              type="text"
              className="form-control form-control-sm me-2"
              placeholder="Type your message..."
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={(e) => e.key === "Enter" && handleSend()}
            />
            <button
              className="btn btn-sm btn-primary"
              onClick={handleSend}
              disabled={!input.trim()}
            >
              Send
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default ChatWidget;
