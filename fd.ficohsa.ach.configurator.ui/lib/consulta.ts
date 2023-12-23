export interface TrantactionACH {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  details: Detail[];
  //banks: BankElement[];
}

export interface BankElement {
  codigoCoreBancario: string;
  nombreBanco: string;
}

export interface Detail {
  transactionType: string;
  messageType: string;
  processedTime: string;
  bank: string;
  receivingBankId: string;
  accountType: number;
  accountTypeString: string;
  originatorAccount: string;
  destinationAccountType: number;
  destinationAccountTypeString: string;
  destinationAccount: string;
  currencyIso: string;
  amount: number;
  reasonCode: string;
  results: string;
  status: number;
  statusString: string;
  instructionIdentification: string;
  messageId: string;
  transactionId: string;
  achTicket: string;
}
