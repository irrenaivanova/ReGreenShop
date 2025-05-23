import { GiFairyWings } from "react-icons/gi";

interface LogoProps {
  size?: number;
  color?: string;
  style?: React.CSSProperties;
}

const Logo = ({ size = 100, color = "#157f83", style }: LogoProps) => {
  return (
    <div style={style}>
      <GiFairyWings size={size} color={color} />
    </div>
  );
};

export default Logo;
