import Link from "next/link";
import { Button, Table } from "react-bootstrap";
import { getColumns, LoadClaims, OMITTED_FIELDS_REGEX } from "../lib/util";
import { dynamicCell } from "./dynamic/cell";
import { dynamicHead } from "./dynamic/head";
import { getId } from "./resource-form";

type ComponentProps = {
  items: any;
  deleteItem: any;
  user: any;
  resource: any;
  schema: any;
};
export const ResourceTable: React.FC<ComponentProps> = ({
  items,
  deleteItem,
  user,
  resource,
  schema,
}) => {
  const isAdmin = user?.role === "Admin";
  const columns = getColumns(resource);
  const permisos = LoadClaims();
  let finalSchema: any;

  if (columns) {
    const schemaMap = Object.fromEntries(
      schema.map((item: any) => [item.field, item])
    );
    finalSchema = columns.map((column) => schemaMap[column]).filter(Boolean);
  } else {
    finalSchema = schema.filter(({ field, type }: any) => {
      if (OMITTED_FIELDS_REGEX.test(field)) {
        return false;
      }

      if (
        field === "IdPais" ||
        field === `Id${resource}` ||
        field === "Catalogos"
      ) {
        return false;
      }

      if (field === "LongitudMin" || field === "LongitudMax") {
        return false;
      }

      if (type === "bool" || type === "space") {
        return false;
      }

      return true;
    });
  }

  if(permisos == undefined){
    return <p>no tiene permisos a esta opción..</p>;
  }

  return (
    <Table
      style={{ overflowX: "auto" }}
      striped
      bordered
      hover
      size="sm"
      responsive
    >
      <thead>
        <tr>
          {finalSchema.map(dynamicHead(resource))}
          <th key="Actions" style={{ textAlign: "right" }}>
            Actions
          </th>
        </tr>
      </thead>
      <tbody>
        {items.map((item: any, index: number) => {
          return (
            <tr key={index}>
              {finalSchema.map(dynamicCell(item, resource))}
              <td
                style={{
                  textAlign: "right",
                  display: "flex",
                  justifyContent: "flex-end",
                }}
              >
                {false && (
                  <Link
                    href={`/app/${resource}/${getId(item, resource)}`}
                    style={{ marginRight: 5 }}
                  >
                    <Button>Ver</Button>
                  </Link>
                )}{" "}
                { permisos.includes("Boton Editar " + resource) ?
                  <Link  href={`/app/${resource}/${getId(item, resource)}/edit`}>
                    <Button >Editar</Button>
                  </Link> 
                  :
                  <span>
                    <Button disabled={true} variant="info">Editar</Button>
                  </span> 
                }{"   "}
                {/* {(
                  <Button
                  disabled = {
                      permisos.includes("Boton Eliminar " + resource) ? false : true
                    }
                    
                    onClick={deleteItem(getId(item, resource))}
                  >
                    Delete
                  </Button>
                )} */}
              </td>
            </tr>
          );
        })}
      </tbody>
    </Table>
  );
};
