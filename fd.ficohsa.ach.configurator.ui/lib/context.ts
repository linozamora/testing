import { createContext } from "react";

export type FicohsaContextType = {
  country: number;
  setCountry: (newValue: number) => void;
};

export const FicohsaContext = createContext<FicohsaContextType>(
  {} as FicohsaContextType
);
