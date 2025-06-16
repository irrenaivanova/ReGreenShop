import React, { useEffect, useState, useRef } from "react";
import { useAuth } from "../../context/AuthContext";
import {
  connectToChatHub,
  disconnectFromChatHub,
  sendMessage,
  getConnectedUsers,
} from "../../services/signalrService";

interface Message {
  senderId: string;
  senderName: string;
  text: string;
}

interface ConnectedUser {
  userId: string;
  userName: string;
}

const ADMIN_GUID = "2cd377ad-ffad-4c83-8455-07b7039733da";

const ChatWidget: React.FC = () => {
  const { user } = useAuth();
  const [input, setInput] = useState("");
  const [isOpen, setIsOpen] = useState(false);
  const [unreadCount, setUnreadCount] = useState(0);
  const [messagesMap, setMessagesMap] = useState<Record<string, Message[]>>({});
  const [connectedUsers, setConnectedUsers] = useState<ConnectedUser[]>([]);
  const [currentChatUserId, setCurrentChatUserId] = useState<string | null>(
    null
  );

  const [userMessages, setUserMessages] = useState<Message[]>([]);

  const [unreadAdminMap, setUnreadAdminMap] = useState<Record<string, number>>(
    {}
  );

  const messagesEndRef = useRef<HTMLDivElement>(null);
  const hasConnectedRef = useRef(false);
  const inactivityTimerRef = useRef<NodeJS.Timeout | null>(null);

  const isOpenRef = useRef(isOpen);
  useEffect(() => {
    isOpenRef.current = isOpen;
  }, [isOpen]);

  const unreadCountRef = useRef(unreadCount);
  useEffect(() => {
    unreadCountRef.current = unreadCount;
  }, [unreadCount]);

  // Admin connect to chat hub on mount
  useEffect(() => {
    if (!user?.isAdmin) return;

    if (user?.accessToken && !hasConnectedRef.current) {
      hasConnectedRef.current = true;

      connectToChatHub(user.accessToken, (senderId, senderName, text) => {
        const newMessage: Message = { senderId, senderName, text };

        setMessagesMap((prev) => {
          const updated = [...(prev[senderId] || []), newMessage];
          return { ...prev, [senderId]: updated };
        });

        setUnreadAdminMap((prev) => {
          if (senderId !== currentChatUserId) {
            return { ...prev, [senderId]: (prev[senderId] || 0) + 1 };
          }
          return prev;
        });

        setCurrentChatUserId((prev) => prev ?? senderId);
      });
    }

    return () => {
      if (user?.isAdmin) {
        disconnectFromChatHub();
        hasConnectedRef.current = false;
      }
    };
  }, [user]);

  useEffect(() => {
    if (!user?.isAdmin) return;

    const interval = setInterval(async () => {
      try {
        const users = await getConnectedUsers();
        setConnectedUsers(users);

        setMessagesMap((prevMessagesMap) => {
          const activeUserIds = new Set(users.map((u) => u.userId));
          const updated = Object.fromEntries(
            Object.entries(prevMessagesMap).filter(([userId]) =>
              activeUserIds.has(userId)
            )
          );
          return updated;
        });

        if (!currentChatUserId && users.length > 0) {
          setCurrentChatUserId(users[0].userId);
        }
      } catch (err) {
        console.error("Polling failed", err);
      }
    }, 2000);

    return () => clearInterval(interval);
  }, [user, currentChatUserId]);

  // Reset inactivity timer for users (not admin)
  const resetInactivityTimer = () => {
    if (inactivityTimerRef.current) clearTimeout(inactivityTimerRef.current);

    inactivityTimerRef.current = setTimeout(() => {
      if (!user?.isAdmin) {
        disconnectFromChatHub();
        hasConnectedRef.current = false;
        setIsOpen(false);
        if (unreadCountRef.current === 0) {
          setUserMessages([]);
        }
        console.log("Disconnected due to inactivity");
      }
    }, 0.2 * 60 * 1000); // 1 minute
  };

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    if (isOpen && !user?.isAdmin) {
      setUnreadCount(0);
    }
  }, [userMessages, isOpen]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messagesMap, currentChatUserId]);

  const toggleChat = () => {
    const willOpen = !isOpen;
    setIsOpen(willOpen);

    if (
      willOpen &&
      !user?.isAdmin &&
      user?.accessToken &&
      !hasConnectedRef.current
    ) {
      hasConnectedRef.current = true;

      connectToChatHub(user.accessToken, (senderId, senderName, text) => {
        const newMessage: Message = { senderId, senderName, text };

        setUserMessages((prev) => {
          const updated = [...prev, newMessage];
          if (!isOpenRef.current) {
            setUnreadCount((c) => c + 1);
          }
          return updated;
        });

        resetInactivityTimer();
      });
    }

    if (willOpen && !user?.isAdmin) {
      setUnreadCount(0);
      resetInactivityTimer();
    }

    if (!willOpen && !user?.isAdmin) {
      // If user closes chat manually, clear timer and disconnect
      if (inactivityTimerRef.current) clearTimeout(inactivityTimerRef.current);
      disconnectFromChatHub();
      hasConnectedRef.current = false;
    }
  };

  // Admin selects user chat - reset unread count for that user
  const handleUserClick = (userId: string) => {
    setCurrentChatUserId(userId);
    setUnreadAdminMap((prev) => {
      const updated = { ...prev };
      delete updated[userId];
      return updated;
    });
  };

  // Reset timer on input change
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInput(e.target.value);
    resetInactivityTimer(); // reset inactivity timer on typing
  };

  const handleSend = () => {
    if (!input.trim() || !user) return;

    if (user.isAdmin && currentChatUserId) {
      sendMessage(currentChatUserId, input);
      setMessagesMap((prev) => {
        const updated = [
          ...(prev[currentChatUserId] || []),
          { senderId: "Me", senderName: "Me", text: input },
        ];
        return { ...prev, [currentChatUserId]: updated };
      });
    } else {
      sendMessage(ADMIN_GUID, input);
      setUserMessages((prev) => [
        ...prev,
        { senderId: "Me", senderName: "Me", text: input },
      ]);
    }
    resetInactivityTimer();
    setInput("");
  };

  if (!user) return null;

  const bannerText = user.isAdmin
    ? "Admin Chat"
    : "Do you have a question? Contact us";

  return (
    <div
      style={{ position: "fixed", bottom: 70, right: 20, zIndex: 1050 }}
      className="shadow"
    >
      {/* Chat Banner */}
      <div
        role="button"
        onClick={toggleChat}
        className={`p-2 text-center bg-primary text-white rounded position-relative ${
          isOpen ? "rounded-bottom-0" : ""
        }`}
        style={{ cursor: "pointer", width: user.isAdmin ? 440 : 280 }}
      >
        {bannerText}

        {/* Admin unread red dot badge */}
        {user.isAdmin &&
          Object.values(unreadAdminMap).some((count) => count > 0) && (
            <span
              className="position-absolute top-0 start-100 translate-middle p-1 bg-danger rounded-circle"
              style={{ width: "12px", height: "12px", pointerEvents: "none" }}
            />
          )}

        {/* User unread count badge */}
        {!isOpen && unreadCount > 0 && !user.isAdmin && (
          <span
            className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
            style={{ fontSize: "0.7rem" }}
          >
            {unreadCount}
          </span>
        )}
      </div>

      {/* Chat Box */}
      {isOpen && (
        <div
          className="bg-white border rounded-bottom d-flex"
          style={{ width: user.isAdmin ? 440 : 280, height: 400 }}
        >
          {/* Admin Tab Sidebar */}
          {user.isAdmin && (
            <div className="border-end" style={{ width: 140 }}>
              <div className="bg-primary text-white text-center py-2">
                Users
              </div>
              {connectedUsers.map((u) => (
                <div
                  key={u.userId}
                  onClick={() => handleUserClick(u.userId)}
                  className={`p-2 border-bottom text-truncate d-flex justify-content-start align-items-center ${
                    u.userId === currentChatUserId ? "bg-light fw-bold" : ""
                  }`}
                  style={{ cursor: "pointer" }}
                >
                  {unreadAdminMap[u.userId] > 0 && (
                    <span className="badge bg-danger me-2">
                      {unreadAdminMap[u.userId]}
                    </span>
                  )}
                  <span>{u.userName}</span>
                </div>
              ))}
            </div>
          )}

          {/* Message Panel */}
          <div className="d-flex flex-column flex-grow-1">
            <div className="bg-light p-2 text-center border-bottom">
              {user.isAdmin
                ? connectedUsers.find((u) => u.userId === currentChatUserId)
                    ?.userName || "No user selected"
                : "Support"}
            </div>
            <div
              className="flex-grow-1 overflow-auto p-2"
              style={{ fontSize: "0.9rem" }}
            >
              {(user.isAdmin
                ? messagesMap[currentChatUserId ?? ""] || []
                : userMessages
              ).map((m, i) => (
                <div
                  key={i}
                  className={`mb-2 ${
                    m.senderId === "Me" ? "text-end" : "text-start"
                  }`}
                >
                  <b>{m.senderId === "Me" ? "Me" : m.senderName}:</b>{" "}
                  <span>{m.text}</span>
                </div>
              ))}
              <div ref={messagesEndRef} />
            </div>

            {/* Input */}
            <div className="p-2 border-top d-flex">
              <input
                type="text"
                className="form-control form-control-sm me-2"
                placeholder="Type your message..."
                value={input}
                onChange={handleInputChange}
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
        </div>
      )}
    </div>
  );
};

export default ChatWidget;
