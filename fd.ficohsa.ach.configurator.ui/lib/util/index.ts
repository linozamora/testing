import _ from "lodash";
import { useState } from "react";
import { Meta, Titles } from "./meta";
import useSWR from "swr";
import { useSession } from "next-auth/react";



export const loadUser = () => {
  const token = localStorage.getItem("token");

  if (token) {
    return JSON.parse(window.atob(token.split(".")[1]));
  } else {
    return null;
  }
};

export const loadCountry = () => {
  const country = localStorage.getItem("country");

  if (country) {
    return parseInt(country);
  } else {
    return null;
  }
};

export const LoadClaims = () => {
const { data: session } = useSession();
const  {  
  data: claims,
  isLoading: channelsLoading,
  error: channelsError,
} = useSWR(`/api/Seguridad?user=${session?.user?.email}`);


return claims;
};

export const OMITTED_FIELDS_REGEX =
  /FechaCreacion|FechaModificacion|UsuarioCreacion|UsuarioModificacion/;

export const COUNTRY_FIELDS_REGEX =
  /Banco|Canal|Moneda|ParticipanteIndirecto|Catalogo|Parametro/;

export const DISABLED_FIELDS =
  /IdPais/;

export const REQUIRED_FIELDS =
  /ahLongitudMin|ahLongitudMax|ccLongitudMin|ccLongitudMax|tcLongitudMin|tcLongitudMax|ptoLongitudMin|ptoLongitudMax|apiUser|apiPassword/;

export const firstLowerCase = (str: string) =>
  str === "URL" ? "url" : str[0].toLowerCase() + str.substr(1);

export const titleCase = (str: string) =>
  `${str[0].toUpperCase()}${str.slice(1)}`;

export const getTitle = (_: any, key: string) => {
  const keyy = titleCase(key);
  return Titles[keyy] || keyy.match(/[A-Z]+[a-z]*/g)?.join(" ");
};

export const getRequiredField = (str: string) => 
REQUIRED_FIELDS.test(str) ? true : false;

export const validateDisabledField = (str: string) =>
DISABLED_FIELDS.test(str)  ? true :false;

export const getWidget = (resource: string, key: string) =>
  _.get<any, string>(Meta, `${resource}.${key}.widget`);

export const getTable = (resource: string, key: string) =>
  _.get<any, string>(Meta, `${resource}.${key}.table`);

export const getLabel = (resource: string, key: string) =>
  _.get<any, string>(Meta, `${resource}.${key}.label`);

export const getOptions = (resource: string, key: string) =>
  _.get<any, string>(Meta, `${resource}.${key}.options`, []);

export const getColumns: (resource: string) => any[] = (resource) =>
  _.get<any, string>(Meta, `${resource}.Columns`) as unknown as any[];

export const getEncode = (resource: string, key: string) =>
  _.get<any, string>(Meta, `${resource}.${key}.encode`);

export const initialize = (
  schema: { [key: string]: any }[],
  resource: string,
  usuario: string
) => {
  const item: any = {
    usuarioCreacion: usuario,
  };

  for (const { field, type, maxLength} of schema) {
    const key = firstLowerCase(field);
    
    if (type === "int") {
      item[key] = 0;
    }

    if (field === "LongitudMin") {
      item.longitudMin = 1;
    }

    if (field === "LongitudMax") {
      item.longitudMax = 1000;
    }

    if (field === "Estado") {
      item.estado = "ACTIVO";
    }

    if (field === "IdPais") {
      item[key] = loadCountry();
    }

    const widget = getWidget(resource, field);

    if (widget === "enum") {
      const options = getOptions(resource, field);
      item[key] = options[0];
    }
  }

  return item;
};

export const translateErrors = (errors: any) => {
  return _.mapValues(errors, (messages, field) =>
    messages.map((m: string) => {
      if (/required/.test(m)) {
        return `Este campo es requerido`;
      }

      if (/length/.test(m)) {
        const [, length] = m.match(/'(\d+)'/) ?? [null, -1];
        return `La longitud máxima es de ${length}`;
      }

      return m;
    })
  );
};

export const extractErrors = (error: any) => {
  return translateErrors(_.get<any, string>(error, "response.data.errors", {}));
};

// Hook
export function useLocalStorage<T>(
  key: string,
  initialValue: T
): [T, (newValue: T) => void] {
  // State to store our value
  // Pass initial state function to useState so logic is only executed once
  const [storedValue, setStoredValue] = useState<T>(() => {
    if (typeof window === "undefined") {
      return initialValue;
    }
    try {
      // Get from local storage by key
      const item = window.localStorage.getItem(key);
      // Parse stored json or if none return initialValue
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      // If error also return initialValue
      console.log(error);
      return initialValue;
    }
  });
  // Return a wrapped version of useState's setter function that ...
  // ... persists the new value to localStorage.
  const setValue = (value: T) => {
    try {
      // Allow value to be a function so we have same API as useState
      const valueToStore =
        value instanceof Function ? value(storedValue) : value;
      // Save state
      setStoredValue(valueToStore);
      // Save to local storage
      if (typeof window !== "undefined") {
        window.localStorage.setItem(key, JSON.stringify(valueToStore));
      }
    } catch (error) {
      // A more advanced implementation would handle the error case
      console.log(error);
    }
  };
  return [storedValue, setValue];
}
