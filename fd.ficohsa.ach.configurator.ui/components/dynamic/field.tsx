import { Form } from "react-bootstrap";
import moment from "moment";
import {
  firstLowerCase,
  getTitle,
  getWidget,
  getOptions,
  getTable,
  getLabel,
  getEncode,
  getRequiredField,
} from "../../lib/util";
import { Relation } from "../relation";
import { getId } from "../resource-form";
import { RemoteResourceTable } from "../remote-resource-table";
import SchemaData from "../../lib/schema";

export const dynamicField =
  (row: any, setRow: any, resource: any, errors: any) =>
  ({ key, type, title, readOnly, maxLength, required }:any) => {
    const camelCaseKey = firstLowerCase(key);
    
    if (key === "IdPais" || key === `Id${resource}`) {
      return null;
    }
    
    

    if (key === "Estado") {
      return (
        <>
          <Form.Label>{key}</Form.Label>
          <Form.Select
            value={row[camelCaseKey]}
            onChange={(ev) =>
              setRow({ ...row, [camelCaseKey]: ev.target.value })
            }
          >
            <option value={"ACTIVO"}>ACTIVO</option>
            <option value={"INACTIVO"}>INACTIVO</option>
          </Form.Select>
        </>
      );
    }

    if (type === "bool") {
      return (
        <Form.Check
          checked={row[camelCaseKey]}
          onChange={(ev) =>
            setRow({ ...row, [camelCaseKey]: ev.target.checked })
          }
          label={getTitle(resource, key)}
        />
      );
    }

    if (type === "DateTime") {
      return (
        <>
          <Form.Label>{title}</Form.Label>
          <Form.Control
            isInvalid={errors[key]}
            type="text"
            value={
              row[camelCaseKey]
                ? moment(row[camelCaseKey]).format("DD/MM/YY HH:MM")
                : ""
            }
            onChange={(ev) =>
              setRow({ ...row, [camelCaseKey]: ev.target.value })
            }
            readOnly={readOnly}
          />
        </>
      );
    }

    if (type === "space") {
      return <br />;
    }

    const widget = getWidget(resource, key);

    if (widget === "enum") {
      const options = getOptions(resource, key);

      return (
        <>
          <Form.Label>{key}</Form.Label>
          <Form.Select
            value={row[camelCaseKey]}
            onChange={(ev) =>
              setRow({ ...row, [camelCaseKey]: ev.target.value })
            }
          >
            {options.map((option:any) => (
              <option value={option} key={option}>
                {option}
              </option>
            ))}
          </Form.Select>
        </>
      );
    }

    if (widget === "relation") {
      const table = getTable(resource, key);
      const label = getLabel(resource, key);
      return (
        <Relation
          table={table}
          label={label}
          value={row[camelCaseKey]}
          onChange={(ev:any) => setRow({ ...row, [camelCaseKey]: ev.target.value })}
        ></Relation>
      );
    }

    if (type === "int") {
      const min = /Retraso|Reintentos/.test(key) ? 0 : 1;

      return (
        <>
          <Form.Label>{title}</Form.Label>
          <Form.Control
            isInvalid={errors[key]}
            type="number"
            min={min}
            max={999999}
            value={row[camelCaseKey]}
            onChange={(ev) =>
              setRow({ ...row, [camelCaseKey]: ev.target.value })
            }
            readOnly={readOnly}
            required={ required } 
            
          />
        </>
      );
    }

    if (widget === "table") {
      if (!getId(row, resource)) {
        return null;
      }

      const table = getTable(resource, key);
      return (
        <RemoteResourceTable
          resource={table}
          parent={resource}
          schema={SchemaData[table]}
          row={row}
        ></RemoteResourceTable>
      );
    }

    if (widget === "password") {
      const encode = getEncode(resource, key);
      const encodeKey = firstLowerCase(encode);

      return (
        <>
          <Form.Label>{title}</Form.Label>
          <Form.Control
            autoComplete="off"
            isInvalid={errors[key]}
            type={row[encodeKey] ? "password" : "text"}
            value={row[camelCaseKey]?.toString() || ""}
            onChange={(ev) =>
              setRow({ ...row, [camelCaseKey]: ev.target.value })
            }
            readOnly={readOnly}
           
          />
        </>
      );
    }

    return (
      <>
        <Form.Label>{title}</Form.Label>
        <Form.Control
          isInvalid={errors[key]}
          type={ key.toLowerCase().includes('password') ? "password" : "text"}
          value={row[camelCaseKey]?.toString() || ""}
          onChange={(ev) => setRow({ ...row, [camelCaseKey]: ev.target.value })}
          readOnly={readOnly} 
          required={ required } 
          maxLength={ maxLength }
          
        />
      </>
    );
  };
