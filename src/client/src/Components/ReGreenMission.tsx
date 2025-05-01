import { FaRecycle, FaLeaf, FaGift, FaSprayCan, FaBeer, FaBoxOpen, FaTrashAlt, FaHandHoldingHeart } from 'react-icons/fa';

const iconStyle = {
    color: '#157f83',
    fontSize: '1.5em',
    marginRight: '8px',
  };
  
  const textColor = {
    color: '#157f83',
  };

  const ReGreenMission = () => {
    return (
      <div style={textColor}>
        <h2 style={textColor}><FaLeaf style={iconStyle} /> ReGreen Rules: Recycling with Rewardss</h2>
        <h3 style={textColor}><FaLeaf style={iconStyle} /> Why Recycling Matters</h3>
        <p>
        At ReGreenShop, we believe that every small action counts when it comes to protecting our planet. 
        The world is facing growing environmental challenges caused by excessive waste and pollution—especially 
        from materials like plastic, metal, and glass. By recycling, we reduce the demand for raw materials, save energy, 
        and lower greenhouse gas emissions.
        hat’s why we’re inviting you to join our ReGreen Mission—an initiative designed to promote sustainability 
        and reward eco-conscious behavior.
        </p>
  
        <h3 style={textColor}><FaRecycle style={iconStyle} /> Our ReGreen Mission</h3>
        <p>
        When receiving your ReGreenShop order, you now have the option to return recyclable items and earn reward points,  
        that can be exchanged for discounts.
        These items must be clean and sorted.
        </p>
        <h2><FaBoxOpen style={iconStyle} /> What You Can Return</h2>
        <ul style={{ listStyleType: 'none', paddingLeft: 0 }}>
        <li><FaSprayCan style={iconStyle} /> Recyclable Item – <strong>Points Awarded</strong></li>
          <li><FaSprayCan style={iconStyle} /> 1 plastic bottle – <strong>5 points</strong></li>
          <li><FaSprayCan style={iconStyle} /> 1 glass bottle – <strong>5 points</strong></li>
          <li><FaBeer style={iconStyle} /> 1 metal can – <strong>5 points</strong></li>
          <li><FaTrashAlt style={iconStyle} /> 1 delivery paper bag – <strong>1 point</strong></li>
          <li><FaBoxOpen style={iconStyle} /> 1 plastic freezer bag (~22×33cm) full of plastic caps – <strong>20 points</strong></li>
        </ul>


      <section style={{ marginTop: '1.5rem' }}>
        <h2><FaHandHoldingHeart style={{ marginRight: '0.5rem' }} /> Maximum Quantities per Order</h2>
        <p>To keep the process manageable and effective, each completed order can include:</p>
        <ol>
          <li>Up to <strong>20 bottles</strong> (plastic + glass combined)</li>
          <li>Up to <strong>10 cans</strong></li>
          <li><strong>1 bag</strong> with plastic caps</li>
          <li><strong>Unlimited</strong> paper bags</li>
        </ol>
      </section>

      <section style={{ marginTop: '1.5rem' }}>
        <h2><FaGift style={{ marginRight: '0.5rem' }} /> ReGreen Rewards: Turn Points into Saving</h2>
        <p>Earn discounts based on the points you collect:</p>
        <ol>
        <li><strong>Points</strong>Reward</li>
          <li><strong>250 points</strong> – 5 lv voucher</li>
          <li><strong>400 points</strong> – 10 lv voucher</li>
          <li><strong>700 points</strong> – 20 lv voucher</li>
        </ol>
      </section>
      <p>Your points will accumulate in your account and can be applied at checkout once you reach a reward threshol.:</p>
      <footer style={{ marginTop: '2rem', fontStyle: 'italic', textAlign: 'center', color: 'green' }}>
        💚 Join the ReGreen Movement. Recycle with your order and be rewarded.
        <p>Let’s build a cleaner future together. Your everyday choices can make a real difference. 
          Recycle with your order, earn rewards, and support a mission that matters.</p>
      </footer>
    </div>
  );
};

export default ReGreenMission;