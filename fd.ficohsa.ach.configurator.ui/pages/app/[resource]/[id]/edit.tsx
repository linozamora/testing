import axios from "axios";
import { FormEvent, useContext, useEffect, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { getId, ResourceForm } from "../../../../components/resource-form";
import { COUNTRY_FIELDS_REGEX, extractErrors, initialize,LoadClaims,titleCase } from "../../../../lib/util";
import SchemaData from "../../../../lib/schema";
import { useRouter } from "next/router";
import { toast } from "react-toastify";
import { FicohsaContext } from "../../../../lib/context";
import { useSession } from "next-auth/react";

const EditResource = () => {
  const router = useRouter();
  const { id, resource } = router.query as {
    id: string;
    resource: string;
    [key: string]: string | string[];
  };
  const schema = SchemaData[resource as string];
  const { country } = useContext(FicohsaContext);
  const [item, setItem] = useState({});
  const [showData, setShowData] = useState(true);
  const [errors, setErrors] = useState({});
  const { data: session } = useSession();
  const permisos = LoadClaims();

  useEffect(() => {
    const fetchItem = async () => {
      if (!id) return;
      const response = await axios.get(`/api/${resource}/${id}`);
      setItem(response.data);
    };
    fetchItem();
  }, [id, resource]);

 
  const onSubmit = async (ev: FormEvent) => {
    ev.preventDefault();

    try {
      const body = COUNTRY_FIELDS_REGEX.test(resource)
        ? { ...item, idPais: country, usuarioModificacion: session?.user?.name ?? "usuario no mapeado" }
        : item;
      await axios.put(`/api/${resource}/${getId(item, resource)}`, body);
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
    }
  };

  if (!schema) {
    return <p>Cargando...</p>;
  }

  if(permisos == undefined){
    return <p>no tiene permisos a esta opción..</p>;
  }
  
  return (
    <Container>
      <div className="tab">
        <button
          type="button"
          onClick={() => setShowData(!showData)}
          className={showData ? "tab-btn" : "dark"}
        >
          Datos
        </button>
        <button
          type="button"
          onClick={() => setShowData(!showData)}
          className={!showData ? "tab-btn" : "dark"}
        >
          Auditoría
        </button>
      </div>
      <Form onSubmit={onSubmit}>
        <ResourceForm
          item={item}
          setItem={setItem}
          resource={resource}
          schema={schema}
          showData={showData}
          errors={errors}
        ></ResourceForm>
        {showData && permisos.includes("Editar " + resource) && <Button type="submit">Guardar</Button>}
      </Form>
    </Container>
  );
};

export default EditResource;
