import axios from "axios";
import Link from "next/link";
import { useRouter } from "next/router";
import { Button, Container, Row } from "react-bootstrap";
import { COUNTRY_FIELDS_REGEX, LoadClaims, titleCase } from "../../../lib/util";
import SchemaData from "../../../lib/schema";
import { ResourceTable } from "../../../components/resource-table";
import { useSession } from "next-auth/react";
import type { NextPage } from "next";
import useSWR from "swr";
import { useContext } from "react";
import { FicohsaContext } from "../../../lib/context";

const Resource: NextPage = () => {
  const router = useRouter();
  const { data: session } = useSession();
  const { country } = useContext(FicohsaContext);
  const { resource: rawResource } = router.query as {
    resource: string;
    [key: string]: string | string[];
  };
  const resource = rawResource ? titleCase(rawResource) : "";
  const schema = SchemaData[resource];
  const query = COUNTRY_FIELDS_REGEX.test(resource) ? `?idPais=${country}` : "";
  const permisos = LoadClaims();

  const {
    data: items,
    mutate,
    isLoading,
    error,
  } = useSWR(`/api/${resource}` + query);

  if (!schema) {
    return <p>Cargando...</p>;
  }

  if (!session) {
    return <Link href="/login">No autorizado</Link>;
  }

  const deleteItem = (id: string) => async () => {
    await axios.delete(`/api/${resource}/${id}`);
    mutate();
  };

  if(permisos == undefined ||  !permisos.includes("Listar " + router.query.resource as string)){
    return <p>no tiene permisos a esta opción..</p>;
  }
  
  return (
    <Container>
      <h2>{resource}</h2>
      {error && <h4>{error.message}</h4>}
      {isLoading && <h3>cargando...</h3>}
      {items && (
        <ResourceTable
          items={items}
          deleteItem={deleteItem}
          user={session.user}
          resource={resource}
          schema={schema}
        />
      )}
      <Row style={{ textAlign: "right" }}>
        <Link href={`/app/${resource}/new`}>
          {  permisos.includes("Boton Nuevo " + resource) && <Button>Nuevo {resource}</Button>}
        </Link>
      </Row>
    </Container>
  );
};
export default Resource;
