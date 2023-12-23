import { getTable, getTitle, getWidget } from "../../lib/util";

export const dynamicHead =
  (resource: string) =>
  ({ field }: any) => {
    const widget = getWidget(resource, field);

    if (widget === "relation") {
      const table = getTable(resource, field);

      return <th key={field}>{getTitle("", table)}</th>;
    }

    return <th key={field}>{getTitle(resource, field)}</th>;
  };
