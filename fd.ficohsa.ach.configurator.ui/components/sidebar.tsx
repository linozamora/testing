import Image from "next/image";
import Link from "next/link";
import { useContext } from "react";
import { FicohsaContext } from "../lib/context";
import logoImage from "../public/Logo-Ficohsa-01.jpg";
import { useCountries } from "./admin-layout";
import { useSession } from "next-auth/react";
import { LoadClaims } from "../lib/util";

export const Sidebar: React.FC = () => {
  const { country } = useContext(FicohsaContext);
  const { countries, isLoading, error } = useCountries();
  const { data: session } = useSession();
  const permisos = LoadClaims();

  if(permisos == undefined){
    return <p>no tiene permisos a esta opción..</p>;
  }
  
  return (
    <nav
      className="nav"
      style={{ justifyContent: "center", alignContent: "space-around" }}
    >
      <Image
        src={logoImage}
        alt="Ficohsa"
        height={100}
      ></Image>
      <ul style={{ width: "100%" }}>
        <li className="navHeader">
          <a>Configuración por país</a>
        </li>
        <li>
          <Link href="/app/pais">
            País:{" "}
            {isLoading
              ? "Cargando.."
              : error
              ? error.message
              : countries
              ? countries.find((c) => c.idPais === country)?.nombre
              : "Error desconocido"}
          </Link>
        </li>
        <li>
          <Link href={
            permisos.includes("Banco") ? "/app/Banco" : "#" 
            }>Banco</Link>
        </li>
        <li>
          <Link href={
            permisos.includes("Moneda") ? "/app/Moneda" : "#" 
            }>Moneda</Link>
        </li>
        <li>
          <Link href={
            permisos.includes("Canal") ? "/app/Canal" : "#" 
            }>Canales</Link>
        </li>
        <li>
          <Link href={
            permisos.includes("ParticipanteIndirecto") ? "/app/ParticipanteIndirecto" : "#" 
            }>
            Participantes Indirectos
          </Link>
        </li>
        <li>
          <Link href={
            permisos.includes("Catalogos") ? "/app/Tabla": "#" 
            }>Catálogos</Link>
        </li>
        <li>
          <Link href={
            permisos.includes("Parametros") ? "/app/Parametro": "#" 
            }>Parámetros</Link>
        </li>
        <li className="navHeader">
          <a>Consultas</a>
        </li>
        <li>
          <Link href={
            permisos.includes("Consulta") ? "/app/consulta": "#" 
            }>Consulta Transacciones</Link>
        </li>
        <li>
          <Link href="/" onClick={() => {}}>
            Cerrar sesión
          </Link>
        </li>
      </ul>
    </nav>
  );
};
