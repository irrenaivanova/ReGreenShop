import { FaHeart, FaRegHeart } from "react-icons/fa";

interface Props {
  isLiked: boolean;
  onToggle: () => void;
}

const LikeButton = ({ isLiked, onToggle }: Props) => (
  <span
    onClick={onToggle}
    style={{ cursor: "pointer", position: "absolute", top: 8, right: 8 }}
  >
    {isLiked ? (
      <FaHeart className="text-primary" />
    ) : (
      <FaRegHeart className="text-dark" />
    )}
  </span>
);

export default LikeButton;
