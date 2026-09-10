import {useState} from "react";

const TheExercice1 = () => {
  const [clicks, setClicks] = useState(0);
  const handleClick = () => {
    alert("Vous avez cliqué!")
    setClicks(clicks + 1);
  };
  return (
    <>
      <h2>Use state</h2>
      <p>Tu te souviens de useState? Va sur ce site web pour garder une trace du nombre de clics.</p>
      <div className="ex1">
        <button onClick={handleClick}>Clique-moi!</button>
        <p>Nombre de clics: {clicks}</p>
      </div>
    </>
  )
}

export default TheExercice1
