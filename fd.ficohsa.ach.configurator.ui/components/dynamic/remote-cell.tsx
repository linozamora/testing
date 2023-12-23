import { Form } from "react-bootstrap";
import { firstLowerCase , useLocalStorage, validateDisabledField} from "../../lib/util";
import { useContext, useEffect, useState } from "react";
import useSWR from "swr";
import axios from "axios";
 

export const dynamicRemoteCell =
  (row: any, setItem: any, errors: any) =>
  ({ field }: any) => {

    const country = localStorage.getItem("country");
 
    const fetchItems = async () => {
      const response = await axios.get(
        `/api/Banco?id${parent}=${row[`id${parent}`]}`
      );
    };
    const rowKey = firstLowerCase(field);
    


    if (field === "Estado") {
      return (
        <td key={field}>
          <Form.Select
            value={row[rowKey] === undefined ? "" : row[rowKey] || ""}
            onChange={(ev) => setItem({ ...row, [rowKey]: ev.target.value })}
          >
            <option>ACTIVO</option>
            <option>INACTIVO</option>
          </Form.Select>
        </td>
      );
    }

    return (
      <td key={field}>
        <Form.Control
          isInvalid={errors && errors[field]}
          type="text"
          value={row[rowKey] || ""}
          onChange={(ev) => setItem({ ...row, [rowKey]: ev.target.value })}
          disabled={validateDisabledField(field)}
          style={{display: !validateDisabledField(field) ? 'block' : 'none' }}
        />
        <Form.Text className="invalid-feedback">
          {errors && errors[field] && errors[field][0]}
        </Form.Text>
      </td>
    );
  };
