import { firstLowerCase, getLabel, getTable, getWidget } from "../../lib/util";

export const dynamicCell =
  (row: any, resource: any) =>
  ({ field }: { field: any }) => {
    const widget = getWidget(resource, field);

    if (widget === "relation") {
      const table = getTable(resource, field);
      const label = getLabel(resource, field);

      return (
        <td key={field}>
          {row[firstLowerCase(table)] && row[firstLowerCase(table)][label]}
        </td>
      );
    }

    return <td key={field}>
      {row.cifrarValor == true && field == "Valor" ? "********" :  row[firstLowerCase(field)] }
      </td>;
  };
