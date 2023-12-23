import { Button, Table } from "react-bootstrap";
import _ from "lodash";
import { useContext, useEffect, useState } from "react";
import axios from "axios";
import { OMITTED_FIELDS_REGEX, translateErrors, initialize, DISABLED_FIELDS } from "../lib/util";
import { dynamicHead } from "./dynamic/head";
import { dynamicRemoteCell } from "./dynamic/remote-cell";
import { toast } from "react-toastify";
import { useSession } from "next-auth/react";
type ComponentProps = {
  row: any;
  resource: string;
  parent: any;
  schema: any;
};
export const RemoteResourceTable: React.FC<ComponentProps> = ({
  row,
  resource,
  parent,
  schema,
}) => {
  const sortedSchema = schema;

  const filteredSchema = sortedSchema.filter(({ field }: any) => {
    if (OMITTED_FIELDS_REGEX.test(field)) {
      return false;
    }

    if (DISABLED_FIELDS.test(field)) {
      return false;
    }

    if (field === `Id${resource}`) {
      return false;
    }

    if (field === `Id${parent}`) {
      return false;
    }

    return true;
  });

  const [items, setItems] = useState<any[]>([]);
  const [errors, setErrors] = useState<any[]>([]);
  const { data: session } = useSession();
  const fetchItems = async () => {
    const response = await axios.get(
      `/api/${resource}?id${parent}=${row[`id${parent}`]}`
    );
    setItems(response.data);
  };

  useEffect(() => {
    const fetchItems = async () => {
      const response = await axios.get(
        `/api/${resource}?id${parent}=${row[`id${parent}`]}`
      );
      setItems(response.data);
    };
    fetchItems();
  }, [resource, parent, row]);

  const save = async () => {
    const resourceId = `id${resource}`;
    const parentId = `id${parent}`;

    const insert = items.filter((item) => !item[resourceId]);
    const update = items.filter((item) => item[resourceId]);

    const promises = [
      ...insert.map((item) =>
        axios.post(`/api/${resource}`, { ...item, [parentId]: row[parentId] })
      ),
      ...update.map((item) =>
        axios.put(`/api/${resource}/${item[resourceId]}`, {...item,usuarioModificacion: session?.user?.name ?? "usuario no mapeado"})
      ),
    ];

    const handled = promises.map((promise) =>
      promise
        .then(() => null)
        .catch((error) => _.get(error, "response.data.errors"))
    );

    const errors = await Promise.all(handled);

    setErrors(errors.map(translateErrors));
    errors.every((e) => !e) && (await fetchItems());
    toast("Guardado", { type: "success" });
  };

  return (
    <>
      <Table style={{ overflowX: "auto" }} className="remote-table">
        <thead>
          <tr>{filteredSchema.map(dynamicHead(resource))}</tr>
        </thead>
        <tbody>
          {items.map((item, index) => {
            const setItem = (item: any) => {
              const newItems = [...items];
              newItems[index] = item;
              setItems(newItems);
            };

            const cellGenerator = dynamicRemoteCell(
              item,
              setItem,
              errors[index]
            );

            return <tr key={index}>{filteredSchema.map(cellGenerator)}</tr>;
          })}
        </tbody>
      </Table>

      <div style={{ textAlign: "right" }}>
        <Button
          onClick={() =>
            setItems([
              ...items,
              initialize(schema, resource, session?.user?.name || "usuario no mapeado"),
            ])
          }
        >
          Añadir {resource}
        </Button>{" "}
        <Button onClick={save}>Guardar</Button>
      </div>
    </>
  );
};
