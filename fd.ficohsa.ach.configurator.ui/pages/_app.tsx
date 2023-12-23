import { SessionProvider } from "next-auth/react";
import type { AppProps } from "next/app";
import "bootstrap/dist/css/bootstrap.min.css";
import "../styles/globals.css";
import { AdminLayout } from "../components/admin-layout";
import type { ReactElement, ReactNode } from "react";
import type { NextPage } from "next";
import axios from "axios";
import { SWRConfig } from "swr";
import Head from "next/head";


export type NextPageWithLayout<P = {}, IP = P> = NextPage<P, IP> & {
  getLayout?: (page: ReactElement) => ReactNode;
};

type AppPropsWithLayout = AppProps & {
  Component: NextPageWithLayout;
};
const fetcher = (url: string) => axios.get(url).then((res) => res.data);
const options = { fetcher };
// Use the <SessionProvider> to improve performance and allow components that call
// `useSession()` anywhere in your application to access the `session` object.
export default function App({ Component, pageProps }: AppPropsWithLayout) {
  const getLayout =
    Component.getLayout ?? ((page) => <AdminLayout>{page}</AdminLayout>);

  return (
    <SessionProvider
      // Provider options are not required but can be useful in situations where
      // you have a short session maxAge time. Shown here with default values.
      session={pageProps.session}
    >
      <Head><meta 
  name    = "color-scheme" 
  content = "only light"/></Head>
      
      <SWRConfig value={options}>
        {getLayout(<Component {...pageProps} />)}
      </SWRConfig>
    </SessionProvider>
  );
}
