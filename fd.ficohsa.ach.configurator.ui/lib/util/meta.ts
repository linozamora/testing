export const Titles: any = {
  UsuarioCreacion: "Usuario creación",
  FechaCreacion: "Fecha creación",
  UsuarioModificacion: "Usuario modificación",
  FechaModificacion: "Fecha modificación",
  CodigoACHMonedaLocal: "Código ACH moneda local",
  CodigoACHMonedaExtranjera: "Código ACH moneda extranjera",
  CodigoISO: "Código ISO",
  Descripcion: "Descripción",
  LongitudMin: "Longitud mínima",
  LongitudMax: "Longitud máxima",
  ParticipanteIndirecto: "Participante indirecto",
  Parametro: "Parámetro",
  Orquestacion: "Orquestación",
};

export const Meta = {
  Tabla: {
    Catalogos: {
      widget: "table",
      table: "Catalogo",
    },
  },
  ParticipanteIndirecto: {
    IdBanco: {
      widget: "relation",
      table: "Banco",
      label: "nombreBanco",
    },
    IdCanal: {
      widget: "relation",
      table: "Canal",
      label: "nombre",
    },
    ApiPassword: {
      widget: "password",
      encode: "CifrarValor",
    },
    Columns: [
      "Nombre",
      "IdBanco",
      "IdCanal",
      "Estado",
      "ApiPassword"
    ],
  },
  Canal: {
    TipoCanal: {
      widget: "enum",
      options: ["INDIRECTO", "MASIVO", "INDIVIDUAL", "CEPROBAN"],
    },
  },
  Parametro: {
    Valor: {
      widget: "password",
      encode: "CifrarValor",
    },
  },
};
