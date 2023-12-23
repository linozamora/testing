import { NextPage } from "next";
import { Container, Row, Col, Form, Table, Button } from "react-bootstrap";
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Popover  from 'react-bootstrap/Popover';
import useSWR from "swr";
import axios from "axios";
import { useContext, useEffect, useState } from "react";
import { FicohsaContext } from "../../lib/context";
import { BankElement, TrantactionACH } from "../../lib/consulta";
import { banksFetcher } from "../../components/admin-layout";
import { CSVLink } from "react-csv";
import { LoadClaims } from "../../lib/util";
 

 

type TransactionsFetchArgs = {
  url: string;
  data: {
    countryId: number;
    bank: string | null;
    transactionType: "SALIENTE" | "ENTRANTE" | null;
    originatorAccount: string | null;
    destinationAccount: string | null;
    processedTime: string;
    currency: "HND" | "USD" | null;
    initialAmount: 0 | null;
    finalAmount: 15000000 | null;
    pageSize: number;
    page: number;
    userName: true;
  };
};

const transactionsFetcher = ({ url, data }: TransactionsFetchArgs) =>
  axios
    .post<TrantactionACH>(url, data, {
      headers: {
        Authorization:
          "basic ODMxNkY1NDJBNDgzNDk2QkI1MzFDQThCQUIxQzNDRDA6NERFNjA4RDM1QkM1NDE3REI0ODE1Mzk0Q0VBNzQwRjk=",
      },
    })
    .then((res) => res.data);

    
const transactionsFetcher2 = ({ url, data }: TransactionsFetchArgs) =>
axios
  .post<TrantactionACH>(url, data, {
    headers: {
      Authorization:
        "basic ODMxNkY1NDJBNDgzNDk2QkI1MzFDQThCQUIxQzNDRDA6NERFNjA4RDM1QkM1NDE3REI0ODE1Mzk0Q0VBNzQwRjk=",
    },
  })
  .then((res) => res.data);

  

const Consultar: NextPage = () => {
  const { country } = useContext(FicohsaContext);
  const [bank, setBank] = useState<string>("null");
  //const [banks, setBanks] = useState<BankElement[]>([]);
  const [transactionType, setTT] = useState<string>("null");
  const [originatorAccount, setOA] = useState<string>("");
  const [destinationAccount, setDA] = useState<string>("");
  const [processedTime, setPT] = useState<string>(new Date().toISOString());
  const [currency, setCurrency] = useState<string>("null");
  const [userName, setUserName] = useState<string>("userinterbanca");
  const [initialAmount, setInitialAmount] = useState<number>(0);
  const [finalAmount, setFinalAmount] = useState<number>(15000000);
  const [page, setPage] = useState<number>(1);

  const permisos = LoadClaims();


  const { data, isLoading, error } = useSWR(
    {
      url: `/api/Ach/GetTransaction`,
      data: {
        countryId: country,
        bank: bank === "null" ? null : bank,
        transactionType: transactionType === "null" ? null : transactionType,
        originatorAccount,
        destinationAccount,
        processedTime,
        currency: currency === "null" ? null : currency,
        initialAmount,
        finalAmount,
        pageSize: 20,
        page,
        userName,
      },
    },
    transactionsFetcher2
  );

  const { data:datat, isLoading: loadingt, error:errort } = useSWR(
    {
      url: `/api/Ach/GetTransaction`,
      data: {
        countryId: country,
        bank: bank === "null" ? null : bank,
        transactionType: transactionType === "null" ? null : transactionType,
        originatorAccount,
        destinationAccount,
        processedTime,
        currency: currency === "null" ? null : currency,
        initialAmount,
        finalAmount,
        pageSize: 10000,
        page:1,
        userName,
      },
    },
    transactionsFetcher
  );

   
  const {
    data: channels,
    isLoading: channelsLoading,
    error: channelsError,
  } = useSWR(`/api/Canal?idPais=${country}`);

  const {
    data: lstbanks,
    isLoading: banksLoading,
    error: banksError,
  } = useSWR(`/api/Banco?idPais=${country}`, banksFetcher);


  const popover = (
    <Popover id="popover-basic">
      <Popover.Body>
        <strong>Tipo de producto</strong> 
        <ul>
          <li>Cuenta Ahorro</li>
          <li>Cuenta Corriente</li>
          <li>Tarjeta de Crédito</li>
          <li>Préstamo</li>
        </ul>
      </Popover.Body>
    </Popover>
  );

  useEffect(() => {
    setPage(1);
  }, [
    bank,
    transactionType,
    originatorAccount,
    destinationAccount,
    processedTime,
    currency,
    initialAmount,
    finalAmount,
    userName,
  ]);

  const header = [
    {label:'Tipotransacción',key:'transactionType'},
    {label:'Tipomensaje',key:'messageType'},
    {label:'Fecha',key:'processedTime'},
    {label:'Banco',key:'bank'},
    {label:'Tipodecuenta',key:'accountTypeString'},
    {label:'Cuentaorigen',key:'originatorAccount'},
    {label:'Tipocuentadestino',key:'destinationAccountTypeString'},
    {label:'Cuentadestino',key:'destinationAccount'},
    {label:'Códigomoneda',key:'currencyIso'},
    {label:'Monto',key:'amount'},
    {label:'Códigorespuesta',key:'reasonCode'},
    {label:'Resultado',key:'results'},
    {label:'Estado',key:'statusString'},
    {label:'Referencia',key:'instructionIdentification'},
    {label:'Ticket ACH',key:'achTicket'},
    {label:'IDMensajeCeproban',key:'messageId'},
    {label:'IDTransacción',key:'transactionId'}
    ];
    

    if(permisos == undefined ||  !permisos.includes("Consulta")){
      return <p>no tiene permisos a esta opción..</p>;
    }

  return (
    <Container style={{ color: "black" }}>
      <Row>
        <h2>Consulta de transacciones</h2>
      </Row>
      <Row>
        <Col>
          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Banco</strong>
            </Form.Label>
            <Form.Select
              value={bank}
              onChange={(e) => {
                setBank(e.target.value);
              }}
            >
              {lstbanks &&
                lstbanks.map((_bank: any, i: number) => (
                  <option value={_bank.codigoCoreBancario} key={i}>
                    {_bank.nombreBanco}
                  </option>
                ))}
            </Form.Select>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Tipo de transacción</strong>
            </Form.Label>
            <Form.Select
              value={transactionType}
              onChange={(e) => {
                setTT(e.target.value);
              }}
            >
              <option value="null">Todos</option>
              <option value="ENTRANTE">Entrante</option>
              <option value="SALIENTE">Saliente</option>
            </Form.Select>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Moneda</strong>
            </Form.Label>
            <Form.Select
              value={currency}
              onChange={(e) => {
                setCurrency(e.target.value);
              }}
            >
              <option value="null">Todas</option>
              <option value="HNL">HNL</option>
              <option value="USD">USD</option>
            </Form.Select>
          </Form.Group>
        </Col>
        <Col>
          <Form.Group className="mb-3">
          <OverlayTrigger
      placement="right"
      delay={{ show: 250, hide: 400 }}
      overlay={popover}>
     <Form.Label>
              <strong>Cuenta origen</strong>
            </Form.Label>
    </OverlayTrigger>
            
            <Form.Control
              value={originatorAccount}
              onChange={(e) => {
                setOA(e.target.value);
              }}
            ></Form.Control>
          </Form.Group>
          <Form.Group className="mb-3">
          <OverlayTrigger
      placement="right"
      delay={{ show: 250, hide: 400 }}
      overlay={popover}>
     <Form.Label>
              <strong>Cuenta destino</strong>
            </Form.Label>
    </OverlayTrigger>
            <Form.Control
              value={destinationAccount}
              onChange={(e) => {
                setDA(e.target.value);
              }}
            ></Form.Control>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Monto mínimo</strong>
            </Form.Label>
            <Form.Control
              value={initialAmount}
              type="number"
              onChange={(e) => {
                setInitialAmount(parseInt(e.target.value));
              }}
            ></Form.Control>
          </Form.Group>
        </Col>
        <Col>
          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Monto máximo</strong>
            </Form.Label>
            <Form.Control
              value={finalAmount}
              type="number"
              onChange={(e) => {
                setFinalAmount(parseInt(e.target.value));
              }}
            ></Form.Control>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Fecha</strong>
            </Form.Label>
            <Form.Control
              value={new Date(processedTime).toISOString().split("T")[0]}
              type="date"
              onChange={(e) => {
                setPT(new Date(e.target.value).toISOString());
              }}
            ></Form.Control>
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>
              <strong>Canal</strong>
            </Form.Label>

            {
              <Form.Select
                value={userName}
                onChange={(e) => {
                  setUserName(e.target.value);
                }}
              >
                {channels &&
                  channels
                    /*.filter(
                      (c: any) =>
                        c.tipoCanal === "MASIVO" || c.tipoCanal === "INDIVIDUAL"
                    )*/
                    .map((channel: any, i: number) => (
                      <option value={channel.usuario} key={i}>
                        {channel.nombre}
                      </option>
                    ))}
              </Form.Select>
            }
          </Form.Group>
        </Col>
      </Row>
      {isLoading && (
        <Row>
          <p>Cargando..</p>
        </Row>
      )}
      {!isLoading && error && (
        <Row>
          <p>No hay transacciones</p>
        </Row>
      )}
      {data && (
        <>
          <Table responsive hover striped bordered size="sm">
            <thead>
              <tr>
                <th>Tipo transacción</th>
                <th>Tipo mensaje</th>
                <th>Fecha</th>
                <th>Banco</th>
                {/* <th>receivingBankId</th> */}
                <th>Tipo de cuenta</th>
                <th>Cuenta origen</th>
                <th>Tipo cuenta destino</th>
                <th>Cuenta destino</th>
                <th>Código moneda</th>
                <th>Monto</th>
                <th>Código respuesta</th>
                <th>Resultado</th>
                <th>Estado</th>
                <th>Referencia</th>
                <th>Ticket ACH</th>
                <th>ID Mensaje Ceproban</th>
                <th>ID Transacción</th>
              </tr>
            </thead>
            {data.details.map((transaction, i, _arr) => (
              <tr key={(page - 1) * 10 + i}>
                <td>{transaction.transactionType}</td>
                <td>{transaction.messageType}</td>
                <td>{transaction.processedTime.slice(0,10)}</td>
                <td>{transaction.bank}</td>
                {/* <td>{transaction.receivingBankId}</td> */}
                <td>{transaction.accountTypeString}</td>
                <td>{transaction.originatorAccount}</td>
                <td>{transaction.destinationAccountTypeString}</td>
                <td>{transaction.destinationAccount}</td>
                <td>{transaction.currencyIso}</td>
                <td>{transaction.amount}</td>
                <td>{transaction.reasonCode}</td>
                <td>{transaction.results}</td>
                <td>{transaction.statusString}</td>
                <td>{transaction.instructionIdentification}</td>
                <td>{transaction.achTicket}</td>
                <td>{transaction.messageId}</td>
                <td>{transaction.transactionId}</td>
              </tr>
            ))}
          </Table>

          <Button disabled={page === 1} onClick={() => setPage(page - 1)}>
            Previous
          </Button>
          <Button
            onClick={() => setPage(page + 1)}
            disabled={page === data.totalPages}
          >
            Next
          </Button>
          <CSVLink data={datat?.details ?? []}  filename={'Reporte Transacciones ACH '+ new Date().toLocaleTimeString() +'.csv'} headers={ header }>
              <Button  >
                Exportar
              </Button>
          </CSVLink>
           
          
          
        </>
      )}
    </Container>
  );
};

export default Consultar;
