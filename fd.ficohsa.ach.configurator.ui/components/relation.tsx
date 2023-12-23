import { useContext } from "react";
import { Form } from "react-bootstrap";
import { COUNTRY_FIELDS_REGEX } from "../lib/util";
import useSWR from "swr";
import { FicohsaContext } from "../lib/context";

type ComponentProps = {
  table: any;
  label: any;
  value: any;
  onChange: any;
};

export const Relation: React.FC<ComponentProps> = ({
  table,
  label,
  value,
  onChange,
}) => {
  const { country } = useContext(FicohsaContext);
  const query = COUNTRY_FIELDS_REGEX.test(table) ? `?idPais=${country}` : "";
  const idKey = `id${table}`;
  const { data: options, isLoading, error } = useSWR(`/api/${table}` + query);

  

  return (
    <Form.Group className="mb-3">
      <Form.Label>{table}</Form.Label>
      <Form.Select
        value={ value || "" }
        onChange={onChange}
        placeholder={
          isLoading
            ? "loading"
            : (error && error.message) || "Error desconocido"
        }
        required
      ><option value="">Seleccione una opción</option>
        {options && options.map((item: any, i: number) => (
          <option value={item[idKey]} key={item[idKey]}>
            {item[label]}
          </option>
        ))}
      </Form.Select>
    </Form.Group>
  );
};
