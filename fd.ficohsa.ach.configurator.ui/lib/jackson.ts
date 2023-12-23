import jackson, {
  type IConnectionAPIController,
  type IOAuthController,
  type JacksonOption,
} from "@boxyhq/saml-jackson";

const samlAudience = "configurator-ach";
const samlPath = "/api/auth/saml/acs";

const opts: JacksonOption = {
  externalUrl: `${process.env.NEXTAUTH_URL}/`,
  samlAudience,
  idpEnabled: true,
  samlPath,
  certs: {
    publicKey: `${process.env.JACKSON_IDP_PUBLIC_CERT}`,
    privateKey: ``,
  },
  idpDiscoveryPath: `${process.env.JACKSON_IDP_DISCOVERY_PATH}`,
  db: {
    engine: "sql",
    type: "postgres",
    url: `${process.env.POSTGRES_CONNSTR}`,
  },
};

let oauthController: IOAuthController;
let connection: IConnectionAPIController;

const g = global as any;

export default async function init() {
  if (!g.oauthController) {
    const ret = await jackson(opts);

    oauthController = ret.oauthController;
    connection = ret.connectionAPIController;
    const connections = await connection.getConnections({
      product: "ach-configurator",
      tenant: "ach",
    });
    if (connections.length === 0) {
      await connection.createSAMLConnection({
        tenant: "ach",
        product: "ach-configurator",
        rawMetadata: `${process.env.JACKSON_METADATA_XML}`,
        redirectUrl: [`${process.env.NEXTAUTH_URL}/*`],
        defaultRedirectUrl: `${process.env.NEXTAUTH_URL}/app`,
      });
    }
    g.oauthController = oauthController;
  } else {
    oauthController = g.oauthController;
  }

  return {
    oauthController,
  };
}
