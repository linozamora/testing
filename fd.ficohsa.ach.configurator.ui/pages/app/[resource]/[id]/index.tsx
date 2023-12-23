import axios from "axios";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import SchemaData from "../../../../lib/schema";
import { firstLowerCase } from "../../../../lib/util";

const ViewResource = () => {
  const router = useRouter();
  const { id, resource } = router.query as {
    id: string;
    resource: string;
    [key: string]: string | string[];
  };
  const [item, setItem] = useState();

  useEffect(() => {
    const fetchItem = async () => {
      if (!id) return;
      const response = await axios.get(`/api/${resource}/${id}`);
      setItem(response.data);
    };
    fetchItem();
  }, [id, resource]);

  if (!id || !resource) return <p>cargando..</p>;

  const schema = SchemaData[resource];
  return (
    <Table>
      <thead>
        <th>Campo</th>
        <th>Valor</th>
      </thead>
      <tbody>
        {Object.entries(schema).map(([k, v]) => (
          <tr key={k}>
            <td>{k}</td>
            <td>{item && item[firstLowerCase(k)]}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};
export default ViewResource;
