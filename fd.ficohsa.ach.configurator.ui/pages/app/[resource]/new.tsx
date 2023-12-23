import axios from "axios";
import Link from "next/link";
import { useRouter } from "next/router";
import { FormEvent, useCallback, useContext, useEffect, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { ResourceForm } from "../../../components/resource-form";
import {
  initialize,
  COUNTRY_FIELDS_REGEX,
  extractErrors,
  titleCase,
  LoadClaims,
} from "../../../lib/util";
import SchemaData from "../../../lib/schema";
import type { NextPage } from "next";
import { FicohsaContext } from "../../../lib/context";
import { useSession } from "next-auth/react";
import { toast } from "react-toastify";

const NewResource: NextPage = () => {
  const router = useRouter();
  const { country } = useContext(FicohsaContext);
  const [item, setItem] = useState<any>({});
  const [schema, setSchema] = useState<any>({});
  const [errors, setErrors] = useState({});
  const { data: session } = useSession();
  const permisos = LoadClaims();

  useEffect(() => {
    setSchema(SchemaData[(router.query.resource as string) || ""]);
  }, [router.query.resource]);

  useEffect(() => {
    if (schema && schema.length && session && session.user) {
      setItem(
        initialize(
          schema,
          titleCase(router.query.resource as string),
          session.user.name || "usuario no mapeado"
        )
      );
    }
  }, [schema.length, router.query.resource]);
  const onSubmit = useCallback(
    async (ev: FormEvent) => {
      ev.preventDefault();

      try {
        const body = COUNTRY_FIELDS_REGEX.test(
          titleCase(router.query.resource as string)
        )
          ? { ...item, idPais: country }
          : item;
        await axios.post(
          `/api/${titleCase(router.query.resource as string)}`,
          body
        );
        toast("Guardado", { type: "success" });
        router.back();
      } catch (error: any) {

        if(error.response.data != "")
        toast("Error al guardar registro " + error.response.data , { type: "error" });
        
        if (
          error.response.data.includes &&
          error.response.data.includes(
            `Violation of UNIQUE KEY constraint`
          )
        ) {
          toast("No puede ingresar valores duplicados", { type: "error" });
        } else {
          setErrors(extractErrors(error));
        }
        router.back();
      }
    },
    [router.query.resource, schema.length, country, item]
  );

  if (
    !router.query.resource ||
    !schema ||
    schema.length === undefined ||
    !item.usuarioCreacion 
  )
    return <p>cargando..</p>;

  if(!permisos.includes(router.query.resource as string)){
    return <p>no tiene permisos a esta opción..</p>;
  }
    

  return (
    <Container>
      <Form onSubmit={onSubmit}>
        <ResourceForm
          item={item}
          setItem={setItem}
          resource={titleCase(router.query.resource as string)}
          schema={schema}
          errors={errors}
          showData
        ></ResourceForm>
        { permisos.includes("Boton Guardar " + (router.query.resource as string)) && <Button type="submit">Guardar</Button>}
      </Form>
    </Container>
  );
};

export default NewResource;
