import { NextPage } from "next";
import { useContext } from "react";
import { Form } from "react-bootstrap";
import { useCountries } from "../../components/admin-layout";
import { FicohsaContext } from "../../lib/context";

const Pais: NextPage = () => {
  const { country, setCountry } = useContext(FicohsaContext);

  const { countries, isLoading, error } = useCountries();

  return (
    <div>
      <Form.Group className="mb-3">
        <Form.Label>
          <strong>Seleccionar país</strong>
        </Form.Label>
        {isLoading ? (
          <p>Cargando...</p>
        ) : error ? (
          <p>{JSON.stringify(error)}</p>
        ) : countries ? (
          <Form.Select
            value={country}
            onChange={(e) => {
              setCountry(parseInt(e.target.value));
            }}
          >
            {countries.map((item, index) => (
              <option value={item.idPais} key={item.idPais}>
                {item.nombre}
              </option>
            ))}
          </Form.Select>
        ) : (
          <p>{error}</p>
        )}
      </Form.Group>
    </div>
  );
};

export default Pais;
