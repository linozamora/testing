const schema: { [key: string]: any } = {
  Tabla: [
    {
      field: "IdTabla",
      type: "int",
    },
    {
      type: "string?",
      field: "Nombre",
      maxLength: "64",
      required: "true"
    },
    {
      field: "Descripcion",
      type: "string?",
      maxLength: "64",
      required: "true"
    },
    {
      field: "Estado",
      type: "string?",
      maxLength: "16",
      required: "true"
    },
    {
      field: "Catalogos",
      type: "never",
    },
    {
      type: "string?",
      field: "UsuarioCreacion",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      field: "UsuarioModificacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  HIS_DetalleTransferenciaACH: [
    {
      type: "Guid",
      field: "IdDetalleTransferenciaACH",
    },
    {
      field: "IdTransferenciaACH",
      type: "int",
    },
    {
      type: "Guid",
      field: "IdACHLote",
    },
    {
      type: "int",
      field: "IdBanco",
    },
    {
      field: "IdParticipanteIndirectoOrigen",
      type: "int",
    },
    {
      field: "IdParticipanteIndirectoDestino",
      type: "int",
    },
    {
      type: "string?",
      field: "OriginatorId",
    },
    {
      type: "string?",
      field: "OriginatorAccount",
    },
    {
      field: "AccountType",
      type: "int",
    },
    {
      field: "OriginatorName",
      type: "string?",
    },
    {
      field: "ReceivingId",
      type: "string?",
    },
    {
      field: "DestinationAccount",
      type: "string?",
    },
    {
      type: "int",
      field: "DestinationAccountType",
    },
    {
      type: "string?",
      field: "RecipientName",
    },
    {
      type: "string?",
      field: "InstructionIdentificacion",
    },
    {
      field: "CurrencyISO",
      type: "string?",
    },
    {
      type: "decimal",
      field: "Amount",
    },
    {
      type: "string?",
      field: "DetailId",
    },
    {
      field: "Status",
      type: "string?",
    },
    {
      field: "Results",
      type: "string?",
    },
    {
      type: "string?",
      field: "Description",
    },
    {
      type: "bool",
      field: "IsValid",
    },
    {
      type: "string?",
      field: "ReasonCode",
    },
    {
      type: "string?",
      field: "ValidationResult",
    },
    {
      type: "DateTime",
      field: "FechaAtencion",
    },
    {
      type: "bool",
      field: "Atendido",
    },
    {
      type: "int",
      field: "CantidadReintentos",
    },
    {
      field: "Topico",
      type: "string?",
    },
    {
      field: "NombrePaso",
      type: "string?",
    },
    {
      field: "EstadoPaso",
      type: "string?",
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      field: "FechaModificacion",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaHis",
    },
  ],
  HIS_TransferenciaACHLog: [
    {
      field: "IdTransferenciaACHLog",
      type: "int",
    },
    {
      type: "int",
      field: "IdTransferenciaACH",
    },
    {
      type: "string?",
      field: "Paso",
    },
    {
      type: "DateTime",
      field: "Iniciado",
    },
    {
      field: "EstadoFinal",
      type: "string?",
    },
    {
      field: "Finalizado",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaHis",
    },
  ],
  DetalleTransferenciaACH: [
    {
      field: "IdDetalleTransferenciaACH",
      type: "Guid",
    },
    {
      type: "int",
      field: "IdTransferenciaACH",
    },
    {
      type: "Guid",
      field: "IdACHLote",
    },
    {
      type: "int",
      field: "IdBanco",
    },
    {
      field: "IdParticipanteIndirectoOrigen",
      type: "int",
    },
    {
      field: "IdParticipanteIndirectoDestino",
      type: "int",
    },
    {
      type: "string?",
      field: "OriginatorId",
    },
    {
      field: "OriginatorAccount",
      type: "string?",
    },
    {
      type: "int",
      field: "AccountType",
    },
    {
      type: "string?",
      field: "OriginatorName",
    },
    {
      type: "string?",
      field: "ReceivingId",
    },
    {
      type: "string?",
      field: "DestinationAccount",
    },
    {
      field: "DestinationAccountType",
      type: "int",
    },
    {
      field: "RecipientName",
      type: "string?",
    },
    {
      field: "InstructionIdentificacion",
      type: "string?",
    },
    {
      field: "CurrencyISO",
      type: "string?",
    },
    {
      field: "Amount",
      type: "decimal",
    },
    {
      field: "DetailId",
      type: "string?",
    },
    {
      type: "string?",
      field: "Status",
    },
    {
      type: "string?",
      field: "Results",
    },
    {
      field: "Description",
      type: "string?",
    },
    {
      field: "IsValid",
      type: "bool",
    },
    {
      field: "ReasonCode",
      type: "string?",
    },
    {
      field: "ValidationResult",
      type: "string?",
    },
    {
      type: "DateTime",
      field: "FechaAtencion",
    },
    {
      field: "Atendido",
      type: "bool",
    },
    {
      field: "CantidadReintentos",
      type: "int",
    },
    {
      type: "string?",
      field: "EstadoRecuperacion",
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      field: "FechaModificacion",
      type: "DateTime",
    },
    {
      field: "TransferenciaACH",
      type: "TransferenciaACH?",
    },
  ],
  ParticipanteIndirecto: [
    {
      type: "int",
      field: "IdParticipanteIndirecto",
    },
    {
      field: "IdBanco",
      type: "int",
    },
    {
      field: "IdCanal",
      type: "int",
    },
    {
      type: "string?",
      field: "CodigoCoreBancario",
      maxLength: "16",
      required: "true"      
    },
    {
      field: "Nombre",
      type: "string?",
      maxLength: "128",
      required: "true"
    },
    {
      type: "string?",
      field: "Descripcion",
      maxLength: "128",
      required: "true"      
    },
/*    {
      field: "CodigoACHMonedaLocal",
      type: "string?",
      maxLength: "8"
    },
    {
      type: "string?",
      field: "CodigoACHMonedaExtranjera",
      maxLength: "8"
    },
*/    
    /*{
      field: "LongitudMin",
      type: "int",
    },
    {
      field: "LongitudMax",
      type: "int",
    },
    {
      type: "bool",
      field: "CompletaCeros",
    },*/
    {
      type: "bool",
      field: "PermiteAH",
    },
    {
      field: "PermiteCC",
      type: "bool",
    },
    {
      type: "bool",
      field: "PermiteTC",
    },
    {
      type: "bool",
      field: "PermitePTO",
    },
    {
      type: "bool",
      field: "ProcesaMasivo",
    },
/*    {
      field: "CuentaT24MonedaLocal",
      type: "string?",
    },
    {
      field: "CuentaT24MonedaExtranjera",
      type: "string?",
    },
*/
    {
      field: "Estado",
      type: "string?",
      maxLength: "16",
      required: "true"
    },
    {
      type: "int",
      field: "ahLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "ahLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "ahCompletaCeros",
    },
    {
      type: "string?",
      field: "ahMensajeError",
      maxLength: "128"
    },
    {
      type: "int",
      field: "ccLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "ccLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "ccCompletaCeros",
    },
    {
      type: "string?",
      field: "ccMensajeError",
      maxLength: "128"
    },
    {
      type: "int",
      field: "tcLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "tcLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "tcCompletaCeros",
    },
    {
      type: "string?",
      field: "tcMensajeError",
      maxLength: "128"
    },
    {
      type: "int",
      field: "ptoLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "ptoLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "ptoCompletaCeros",
    },
    {
      type: "string?",
      field: "ptoMensajeError",
      maxLength: "128"
    },
    {
      type: "string?",
      field: "CuentaCoreBancarioMonedaLocal",
      maxLength: "32"
    },
    {
      type: "string?",
      field: "CuentaCoreBancarioMonedaExtranjera",
      maxLength: "32"
    },
    {
      type: "string?",
      field: "CuentaInternaCoreBancarioMonedaLocal",
      maxLength: "32"
    },
    {
      type: "string?",
      field: "CuentaInternaCoreBancarioMonedaExtranjera",
      maxLength: "32"
    },
    {
      field: "URL",
      type: "string?",
      maxLength: "1024"
    },
    {
      type: "string?",
      field: "apiUser",
      maxLength: "64"
    },
    {
      type: "string?",
      field: "apiPassword",
      maxLength: "64"      
    },
    {
      field: "UsuarioCreacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      type: "string?",
      field: "UsuarioModificacion",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  HIS_TicketACH: [
    {
      type: "Guid",
      field: "IdTicketACH",
    },
    {
      type: "int",
      field: "IdCanal",
    },
    {
      type: "string?",
      field: "Estado",
    },
    {
      field: "FechaSolicitud",
      type: "DateTime",
    },
    {
      field: "FechaModificacion",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaHis",
    },
  ],
  Banco: [
    {
      field: "IdBanco",
      type: "int",
    },
    {
      type: "int",
      field: "IdPais",
    },
    {
      type: "string?",
      field: "NombreBanco",
      maxLength: "64",
      required: "true"
    },
    {
      type: "string?",
      field: "CodigoCoreBancario",
      maxLength: "16",
      required: "true"
    },
    {
      type: "string?",
      field: "CodigoACHMonedaLocal",
      maxLength: "16"
    },
    {
      type: "string?",
      field: "CodigoACHMonedaExtranjera",
      maxLength: "16"
    },
    {
      type: "int",
      field: "ahLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "ahLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "ahCompletaCeros",
    },
    {
      type: "string?",
      field: "ahMensajeError",
      maxLength: "128"
    },
    /*{
      type: "int",
      field: "LongitudMin",
    },
    {
      field: "LongitudMax",
      type: "int",
    },
    {
      type: "bool",
      field: "completaCeros",
    },*/
    {
      type: "int",
      field: "ccLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "ccLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "ccCompletaCeros",
    },
    {
      type: "string?",
      field: "ccMensajeError",
      maxLength: "128"
    },
    {
      type: "int",
      field: "tcLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "tcLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "tcCompletaCeros",
    },
    {
      type: "string?",
      field: "tcMensajeError",
      maxLength: "128"
    },
    {
      type: "int",
      field: "ptoLongitudMin",
      required: "true"
    },
    {
      type: "int",
      field: "ptoLongitudMax",
      required: "true"
    },
    {
      type: "bool",
      field: "ptoCompletaCeros",
    },
    {
      type: "string?",
      field: "ptoMensajeError",
      maxLength: "128"
    },
    {
      type: "bool",
      field: "PermiteAH",
    },
    {
      type: "bool",
      field: "PermiteCC",
    },
    {
      type: "bool",
      field: "PermiteTC",
    },
    {
      type: "bool",
      field: "PermitePTO",
    },
    {
      type: "bool",
      field: "Migrado",
    },
    {
      type: "string?",
      field: "Estado",
      maxLength: "16",
      required: "true"
    },
    {
      field: "UsuarioCreacion",
      type: "string?",
      maxLength: "64"
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      field: "UsuarioModificacion",
      type: "string?",
      maxLength: "64"
      
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  Parametro: [
    {
      type: "int",
      field: "IdParametro",
    },
    {
      type: "int",
      field: "IdPais",
    },
    {
      field: "Nombre",
      type: "string?",
      maxLength: "32",
      required: "true"
    },
    {
      field: "Descripcion",
      type: "string?",
      maxLength: "256",
      required: "true"
    },
    {
      type: "string?",
      field: "Valor",
      maxLength: "256",
      required: "true"
    },
    {
      field: "CifrarValor",
      type: "bool",
    },
    {
      type: "string?",
      field: "Estado",
      maxLength: "16",
      required: "true"
    },
    {
      field: "UsuarioCreacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      type: "string?",
      field: "UsuarioModificacion",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  HIS_ACHLoteRequest: [
    {
      type: "Guid",
      field: "IdACHLote",
    },
    {
      field: "IdCanal",
      type: "int",
    },
    {
      field: "TipoTransaccion",
      type: "string?",
    },
    {
      type: "string?",
      field: "TipoMensaje",
    },
    {
      type: "string?",
      field: "Estado",
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      field: "FechaFinalizacion",
      type: "DateTime",
    },
    {
      field: "FechaHis",
      type: "DateTime",
    },
  ],
  Moneda: [
    {
      type: "int",
      field: "IdMoneda",
    },
    {
      field: "IdPais",
      type: "int",
    },
    {
      type: "string?",
      field: "Nombre",
      maxLength: "32",
      required: "true"
    },
    {
      field: "Simbolo",
      type: "string?",
      maxLength: "4",
      required: "true"
    },
    {
      type: "string?",
      field: "CodigoISO",
      maxLength: "4",
      required: "true"
    },
    {
      field: "TipoCambio",
      type: "decimal",
    },
    {
      type: "string?",
      field: "Estado",
      maxLength: "16",
      required: "true"
    },
    {
      field: "UsuarioCreacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      field: "UsuarioModificacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  Pais: [
    {
      field: "IdPais",
      type: "int",
    },
    {
      field: "NombrePais",
      type: "string?",
      maxLength: "64",
      required: "true"
    },
    {
      field: "CodigolSO",
      type: "string?",
      maxLength: "4",
      required: "true"
    },
    {
      type: "string?",
      field: "Estado",
      maxLength: "16",
      required: "true"
    },
    {
      field: "UsuarioCreacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
    {
      field: "UsuarioModiticacion",
      type: "string?",
      maxLength: "64"
    },
  ],
  HIS_TransferenciaACH: [
    {
      field: "IdTransferenciaACH",
      type: "int",
    },
    {
      field: "IdACHLote",
      type: "Guid",
    },
    {
      type: "int",
      field: "IdBanco",
    },
    {
      type: "bool",
      field: "FueraLinea",
    },
    {
      type: "string?",
      field: "IdMensaje",
    },
    {
      field: "FechaAtencion",
      type: "DateTime",
    },
    {
      field: "Atendido",
      type: "bool",
    },
    {
      field: "CantidadReintentos",
      type: "int",
    },
    {
      type: "string?",
      field: "Topico",
    },
    {
      field: "NombrePaso",
      type: "string?",
    },
    {
      field: "EstadoPaso",
      type: "string?",
    },
    {
      field: "FechaInicioProceso",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaFinProceso",
    },
    {
      type: "DateTime",
      field: "FechaMaximaAtencion",
    },
    {
      field: "FechaProcesoPayExpedite",
      type: "DateTime",
    },
    {
      type: "string?",
      field: "ResultMessage",
    },
    {
      field: "ReasonCode",
      type: "string?",
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      field: "FechaModificacion",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaHis",
    },
  ],
  TransferenciaACH: [
    {
      type: "int",
      field: "IdTransferenciaACH",
    },
    {
      field: "IdACHLote",
      type: "Guid",
    },
    {
      type: "int",
      field: "IdBanco",
    },
    {
      type: "bool",
      field: "FueraLinea",
    },
    {
      type: "string?",
      field: "IdMensaje",
    },
    {
      field: "FechaAtencion",
      type: "DateTime",
    },
    {
      field: "Atendido",
      type: "bool",
    },
    {
      field: "CantidadReintentos",
      type: "int",
    },
    {
      field: "EstadoRecuperacion",
      type: "string?",
    },
    {
      type: "DateTime",
      field: "FechaInicioProceso",
    },
    {
      type: "DateTime",
      field: "FechaFinProceso",
    },
    {
      type: "DateTime",
      field: "FechaMaximaAtencion",
    },
    {
      field: "FechaProcesoPayExpedite",
      type: "DateTime",
    },
    {
      field: "ResultMessage",
      type: "string?",
    },
    {
      type: "string?",
      field: "ReasonCode",
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  TipoCambio: [
    {
      field: "IdTipoCambio",
      type: "int",
    },
    {
      type: "int",
      field: "IdPais",
    },
    {
      field: "Fecha",
      type: "DateTime",
    },
    {
      type: "decimal",
      field: "TipoDeCambio",
    },
    {
      field: "Estado",
      type: "string?",
    },
    {
      type: "string?",
      field: "UsuarioCreacion",
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
  ],
  TransferenciaACHLog: [
    {
      type: "int",
      field: "IdTransferenciaACHLog",
    },
    {
      field: "IdTransferenciaACH",
      type: "int",
    },
    {
      field: "Paso",
      type: "string?",
    },
    {
      type: "DateTime",
      field: "Iniciado",
    },
    {
      type: "string?",
      field: "EstadoFinal",
    },
    {
      type: "DateTime",
      field: "Finalizado",
    },
  ],
  HIS_DetalleTransferenciaACHLog: [
    {
      field: "IdDetalleTransferenciaACHLog",
      type: "int",
    },
    {
      field: "IdDetalleTransferenciaACH",
      type: "Guid",
    },
    {
      field: "Paso",
      type: "string?",
    },
    {
      type: "DateTime",
      field: "Iniciado",
    },
    {
      type: "string?",
      field: "EstadoFinal",
    },
    {
      type: "DateTime",
      field: "Finalizado",
    },
    {
      field: "FechaHis",
      type: "DateTime",
    },
  ],
  ACHLoteRequest: [
    {
      type: "Guid",
      field: "IdACHLote",
    },
    {
      field: "IdCanal",
      type: "int",
    },
    {
      field: "TipoTransaccion",
      type: "string?",
    },
    {
      type: "string?",
      field: "TipoMensaje",
    },
    {
      field: "Estado",
      type: "string?",
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaFinalizacion",
    },
  ],
  Canal: [
    {
      type: "int",
      field: "IdCanal",
    },
    {
      type: "int",
      field: "IdPais",
    },
    {
      type: "string?",
      field: "Nombre",
      maxLength: "64",
      required: "true"
    },
    {
      field: "TipoCanal",
      type: "string?",
      maxLength: "16",
      required: "true"
    },
    {
      field: "Usuario",
      type: "string?",
      maxLength: "32",
      required: "true"
    },
    {
      field: "Estado",
      type: "string?",
      maxLength: "16",
      required: "true"
    },
    {
      type: "string?",
      field: "UsuarioCreacion",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaCreacion",
    },
    {
      type: "string?",
      field: "UsuarioModificacion",
      maxLength: "64"
    },
    {
      field: "FechaModificacion",
      type: "DateTime",
    },
  ],
  TicketACH: [
    {
      type: "Guid",
      field: "IdTicketACH",
    },
    {
      type: "int",
      field: "IdCanal",
    },
    {
      type: "string?",
      field: "Estado",
    },
    {
      field: "FechaSolicitud",
      type: "DateTime",
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
  Catalogo: [
    {
      field: "IdCatalogo",
      type: "int",
    },
    {
      type: "int",
      field: "IdPais",
    },
    {
      type: "int",
      field: "IdTabla",
    },
    {
      field: "Codigo",
      type: "string?",
      maxLength: "32",
      required: "true"
    },
    {
      field: "Valor",
      type: "string?",
      maxLength: "256",
      required: "true"
    },
    {
      type: "string?",
      field: "Estado",
      maxLength: "16",
      required: "true"
    },
    {
      type: "string?",
      field: "UsuarioCreacion",
      maxLength: "64"
    },
    {
      field: "FechaCreacion",
      type: "DateTime",
    },
    {
      field: "UsuarioModificacion",
      type: "string?",
      maxLength: "64"
    },
    {
      type: "DateTime",
      field: "FechaModificacion",
    },
  ],
};

export default schema;
