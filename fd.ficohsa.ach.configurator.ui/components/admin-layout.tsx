import { Sidebar } from "./sidebar";
import useSWR from "swr";
import "bootstrap/dist/css/bootstrap.min.css";
import "react-toastify/dist/ReactToastify.css";
import { ToastContainer, toast } from "react-toastify";
import axios from "axios";
import { useLocalStorage } from "../lib/util";
import { FicohsaContext, FicohsaContextType } from "../lib/context";

type Country = { [key: string]: any };

async function countriesFetcher(url: string): Promise<Country[]> {
  const res = await axios.get(url);
  // If the status code is not in the range 200-299,
  // we still try to parse and throw it.
  if (res.status !== 200) {
    const error = new Error(
      "An error occurred while fetching the data."
    ) as any;
    // Attach extra info to the error object.
    error.info = await res.data;
    error.status = res.status;

    throw error;
  }

  let values: Country[] = res.data as Country[];
  return [{ idPais: 0, nombre: "No seleccionado" }, ...values];
}

export const useCountries = () => {
  const { data, error, isLoading } = useSWR<Country[]>(
    "/api/Pais",
    countriesFetcher
  );

  const countries = data || [];
  return { countries, error, isLoading };
};

export async function banksFetcher(url: string): Promise<any[]> {
  const res = await axios.get(url);
  // If the status code is not in the range 200-299,
  // we still try to parse and throw it.
  if (res.status !== 200) {
    const error = new Error(
      "An error occurred while fetching the data."
    ) as any;
    // Attach extra info to the error object.
    error.info = await res.data;
    error.status = res.status;

    throw error;
  }

  let values: any[] = res.data as any[];
  return [{ codigoCoreBancario: "null", nombreBanco: "Todos" }, ...values];
}

export const AdminLayout: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [country, setCountry] = useLocalStorage<number>("country", 0);
  const context: FicohsaContextType = {
    country,
    setCountry,
  };
  return (
    <FicohsaContext.Provider value={context}>
      <div>
        <Sidebar></Sidebar>
        <main className="main" style={{ backgroundColor: "white" }}>
          {children}
        </main>
      </div>
      <ToastContainer theme="colored" />
    </FicohsaContext.Provider>
  );
};
