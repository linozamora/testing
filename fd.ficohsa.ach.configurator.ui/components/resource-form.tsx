import _ from "lodash";
import { Form } from "react-bootstrap";
import { getTitle, OMITTED_FIELDS_REGEX } from "../lib/util";
import { dynamicField } from "./dynamic/field";
type ComponentProps = {
  item: any;
  setItem: any;
  resource: string;
  schema: any;
  showData: any;
  errors: any;
};
export const ResourceForm: React.FC<ComponentProps> = ({
  item,
  setItem,
  resource,
  schema,
  showData,
  errors,
}) => {
  const form = schema.map(({ field, type, maxLength ,required}: any) => {
    return {
      key: field,
      type,
      title: getTitle(resource, field),
      maxLength,
      required,
      readOnly: OMITTED_FIELDS_REGEX.test(field) || field === "TipoCambio",
    };
  });

  const [right, left] = _.partition(form, (item) =>
    OMITTED_FIELDS_REGEX.test(item.key)
  );
  const fieldGenerator = dynamicField(item, setItem, resource, errors);

  return (
    <>
      {showData &&
        left.map((field) => {
          return (
            <Form.Group key={field.key} className="mb-3">
              {fieldGenerator(field)}
              <Form.Text className="invalid-feedback">
                {errors[field.key] && errors[field.key][0]}
              </Form.Text>
            </Form.Group>
          );
        })}

      {!showData &&
        right.map((field) => {
          return (
            <Form.Group key={field.key} className="mb-3">
              {fieldGenerator(field)}
            </Form.Group>
          );
        })}
    </>
  );
};

export const getId = (item: any, resource: string) => item[`id${resource}`];
