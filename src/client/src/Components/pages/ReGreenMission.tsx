import { CgShoppingBag } from "react-icons/cg";
import {
  FaRecycle,
  FaLeaf,
  FaGift,
  FaBeer,
  FaHandHoldingHeart,
  FaWineBottle,
} from "react-icons/fa";
import { IoBagHandleSharp } from "react-icons/io5";
import { PiBeerBottleFill } from "react-icons/pi";
import { TbTruckReturn } from "react-icons/tb";

const iconStyle = {
  color: "#157f83",
  fontSize: "1.5em",
  marginRight: "8px",
};

const listIconStyle = {
  color: "#f0ad4e",
  fontSize: "1.5em",
  marginRight: "8px",
};

const ReGreenMission = () => {
  return (
    <div
      className="card border-primary mx-auto my-4"
      style={{
        maxWidth: "900px",
        textAlign: "center",
        padding: "2rem",
        borderRadius: "20px",
      }}
    >
      <div style={{ color: "black" }}>
        <h2
          className="text-primary"
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            gap: "0.5rem",
            marginBottom: "1.5rem",
            borderBottom: "1px solid #f0ad4e", // yellow underline
            paddingBottom: "0.25rem", // some space between text and underline
          }}
        >
          ReGreen Rules: Recycling with Rewards
        </h2>

        <h3
          className="text-primary"
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            gap: "0.5rem",
            marginBottom: "0.5rem",
          }}
        >
          <FaLeaf style={iconStyle} /> Why Recycling Matters
        </h3>
        <p style={{ marginBottom: "1.5rem" }}>
          At ReGreenShop, we believe that every small action counts when it
          comes to protecting our planet. The world is facing growing
          environmental challenges caused by excessive waste and
          pollution—especially from materials like plastic, metal, and glass. By
          recycling, we reduce the demand for raw materials, save energy, and
          lower greenhouse gas emissions. That’s why we’re inviting you to join
          our ReGreen Mission— an initiative designed to promote sustainability
          and reward eco-conscious behavior.
        </p>

        <hr style={{ borderColor: "#f0ad4e", marginBottom: "1.5rem" }} />

        <h3
          className="text-primary"
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            gap: "0.5rem",
            marginBottom: "0.5rem",
          }}
        >
          <FaRecycle style={iconStyle} /> Our ReGreen Mission
        </h3>
        <p style={{ marginBottom: "1.5rem" }}>
          When receiving your ReGreenShop order, you now have the option to
          return recyclable items and earn reward points that can be exchanged
          for discounts. These items must be clean and sorted.
        </p>

        <hr style={{ borderColor: "#f0ad4e", marginBottom: "1.5rem" }} />

        <h3
          className="text-primary"
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            gap: "0.5rem",
            marginTop: "2rem",
            marginBottom: "1rem",
          }}
        >
          <TbTruckReturn style={iconStyle} /> What You Can Return
        </h3>
        <ul
          style={{
            listStyleType: "none",
            paddingLeft: 0,
            textAlign: "center",
            maxWidth: "600px",
            margin: "0 auto 2rem",
          }}
        >
          <li style={{ marginBottom: "0.5rem" }}>
            <PiBeerBottleFill style={listIconStyle} />1 Plastic bottle –{" "}
            <strong>5 points</strong>
          </li>
          <li style={{ marginBottom: "0.5rem" }}>
            <FaWineBottle style={listIconStyle} />1 Glass bottle –{" "}
            <strong>5 points</strong>
          </li>
          <li style={{ marginBottom: "0.5rem" }}>
            <FaBeer style={listIconStyle} />1 Metal can –{" "}
            <strong>5 points</strong>
          </li>
          <li style={{ marginBottom: "0.5rem" }}>
            <IoBagHandleSharp style={listIconStyle} />1 Delivery paper bag –{" "}
            <strong>1 point</strong>
          </li>
          <li style={{ marginBottom: "0.5rem" }}>
            <CgShoppingBag style={listIconStyle} />1 Plastic freezer bag
            (~22×33cm) full of plastic caps – <strong>20 points</strong>
          </li>
        </ul>

        <hr style={{ borderColor: "#f0ad4e", marginBottom: "2rem" }} />

        <section style={{ marginBottom: "2rem" }}>
          <h3
            className="text-primary"
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: "0.5rem",
              marginBottom: "0.5rem",
            }}
          >
            <FaHandHoldingHeart
              style={{
                marginRight: "0.5rem",
                color: "#157f83",
                fontSize: "1.5em",
              }}
            />{" "}
            Maximum Quantities per Order
          </h3>
          <p style={{ marginBottom: "1rem" }}>
            To keep the process manageable and effective, each completed order
            can include:
          </p>
          <ul
            style={{
              maxWidth: "600px",
              margin: "0 auto",
              textAlign: "center",
              paddingLeft: 0,
              listStyleType: "none",
            }}
          >
            <li>
              Up to <strong>20 bottles</strong> (plastic + glass combined)
            </li>
            <li>
              Up to <strong>10 cans</strong>
            </li>
            <li>
              <strong>1 bag</strong> with plastic caps
            </li>
            <li>
              <strong>Unlimited</strong> paper bags
            </li>
          </ul>
        </section>

        <hr style={{ borderColor: "#f0ad4e", marginBottom: "2rem" }} />

        <section style={{ marginBottom: "2rem" }}>
          <h3
            className="text-primary"
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: "0.5rem",
              marginBottom: "0.5rem",
            }}
          >
            <FaGift
              style={{
                marginRight: "0.5rem",
                color: "#157f83",
                fontSize: "1.5em",
              }}
            />{" "}
            ReGreen Rewards: Turn Points into Savings
          </h3>
          <p style={{ marginBottom: "1rem" }}>
            Earn discounts based on the points you collect:
          </p>
          <ol
            style={{
              maxWidth: "600px",
              margin: "0 auto",
              textAlign: "center",
              paddingLeft: 0,
              listStyleType: "none",
            }}
          >
            <li>
              <strong>Points</strong> Reward
            </li>
            <li>
              <strong>250 points</strong> – 5 lv voucher
            </li>
            <li>
              <strong>400 points</strong> – 10 lv voucher
            </li>
            <li>
              <strong>700 points</strong> – 20 lv voucher
            </li>
          </ol>
        </section>

        <hr style={{ borderColor: "#f0ad4e", marginBottom: "1.5rem" }} />

        <p className="text-black" style={{ marginBottom: "1.5rem" }}>
          Your points will accumulate in your account and can be applied at
          checkout once you reach a reward threshold.
        </p>

        <footer
          className="text-black"
          style={{
            fontStyle: "italic",
            textAlign: "center",
          }}
        >
          💚 Join the ReGreen Movement. Recycle with your order and be rewarded.
          <p>
            Let’s build a cleaner future together. Your everyday choices can
            make a real difference. Recycle with your order, earn rewards, and
            support a mission that matters.
          </p>
        </footer>
      </div>
    </div>
  );
};

export default ReGreenMission;
